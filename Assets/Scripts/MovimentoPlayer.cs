using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovimentoPlayer : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 4.5f;
    public float velocidadeCorrer = 7.0f;
    public float velocidadeAgachado = 2.2f;

    [Header("Salto")]
    public float forcaSalto = 5.5f;
    public LayerMask camadaChao;
    public float distanciaChao = 0.15f;
    public float tempoEntreSaltos = 0.25f;

    [Header("Câmara")]
    public Transform cameraTransform;

    [Header("Animações")]
    public Animator animator;

    [Header("Interação")]
    public Key teclaInteragir = Key.E;
    public float tempoBloqueadoAoInteragir = 0.6f;

    private Rigidbody rb;
    private Vector2 inputs;

    private bool querSaltar;
    private bool estaAgachado;
    private bool estaACorrer;
    private bool estaAInteragir;

    private bool podeSaltar = true;
    private float tempoUltimoSalto;
    private float timerInteracao;

    public bool EstaAgachado
    {
        get { return estaAgachado; }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (estaAInteragir)
        {
            timerInteracao -= Time.deltaTime;

            if (timerInteracao <= 0f)
            {
                estaAInteragir = false;
            }
        }

        LerInputMovimento();
        LerInputAgachar();
        LerInputInteragir();
        LerInputSaltar();

        bool noChao = EstaNoChao();

        AtualizarAnimacoes(noChao);
    }

    private void FixedUpdate()
    {
        if (cameraTransform == null) return;

        if (estaAInteragir)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        Vector3 frenteCamera = cameraTransform.forward;
        Vector3 direitaCamera = cameraTransform.right;

        frenteCamera.y = 0f;
        direitaCamera.y = 0f;

        frenteCamera.Normalize();
        direitaCamera.Normalize();

        Vector3 direcaoMovimento =
            frenteCamera * inputs.y +
            direitaCamera * inputs.x;

        direcaoMovimento.Normalize();

        float velocidadeAtual = velocidade;

        if (estaAgachado)
        {
            velocidadeAtual = velocidadeAgachado;
        }
        else if (estaACorrer)
        {
            velocidadeAtual = velocidadeCorrer;
        }

        Vector3 deslocamento =
            direcaoMovimento * velocidadeAtual * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + deslocamento);

        if (direcaoMovimento != Vector3.zero)
        {
            Quaternion rotacaoDesejada = Quaternion.LookRotation(direcaoMovimento);

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    rotacaoDesejada,
                    Time.fixedDeltaTime * 12f
                )
            );
        }

        if (querSaltar)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * forcaSalto, ForceMode.Impulse);
            querSaltar = false;
        }
    }

    private void LerInputMovimento()
    {
        if (estaAInteragir)
        {
            inputs = Vector2.zero;
            estaACorrer = false;
            return;
        }

        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current.dKey.isPressed) moveX += 1f;
        if (Keyboard.current.aKey.isPressed) moveX -= 1f;
        if (Keyboard.current.wKey.isPressed) moveZ += 1f;
        if (Keyboard.current.sKey.isPressed) moveZ -= 1f;

        inputs = new Vector2(moveX, moveZ).normalized;

        estaACorrer =
            !estaAgachado &&
            (Keyboard.current.leftShiftKey.isPressed ||
             Keyboard.current.rightShiftKey.isPressed);
    }

    private void LerInputAgachar()
    {
        if (estaAInteragir) return;

        if (Keyboard.current.leftCtrlKey.wasPressedThisFrame ||
            Keyboard.current.rightCtrlKey.wasPressedThisFrame)
        {
            estaAgachado = !estaAgachado;
        }
    }

    private void LerInputInteragir()
    {
        if (estaAInteragir) return;

        if (Keyboard.current[teclaInteragir].wasPressedThisFrame)
        {
            FazerAnimacaoInteracao();
        }
    }

    private void LerInputSaltar()
    {
        if (estaAInteragir) return;

        bool noChao = EstaNoChao();

        if (noChao && Time.time > tempoUltimoSalto + tempoEntreSaltos)
        {
            podeSaltar = true;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && noChao && podeSaltar && !estaAgachado)
        {
            querSaltar = true;
            podeSaltar = false;
            tempoUltimoSalto = Time.time;

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    public void FazerAnimacaoInteracao()
    {
        if (animator == null) return;

        estaAInteragir = true;
        timerInteracao = tempoBloqueadoAoInteragir;

        inputs = Vector2.zero;
        estaACorrer = false;

        if (estaAgachado)
        {
            animator.ResetTrigger("InteractStanding");
            animator.SetTrigger("InteractCrouching");
        }
        else
        {
            animator.ResetTrigger("InteractCrouching");
            animator.SetTrigger("InteractStanding");
        }
    }

    private bool EstaNoChao()
    {
        return Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            distanciaChao + 0.2f,
            camadaChao
        );
    }

    private void AtualizarAnimacoes(bool noChao)
    {
        if (animator == null) return;

        float speed = inputs.magnitude;

        if (estaACorrer && speed > 0.1f && !estaAgachado)
        {
            speed = 2f;
        }

        if (estaAInteragir)
        {
            speed = 0f;
        }

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsCrouching", estaAgachado);
        animator.SetBool("IsGrounded", noChao);
    }
}