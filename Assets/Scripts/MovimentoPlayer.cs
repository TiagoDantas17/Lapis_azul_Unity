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
    public float distanciaChao = 0.3f;

    [Header("Câmara")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector2 inputs;

    private bool querSaltar;
    private bool estaAgachado;
    private bool estaACorrer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current.dKey.isPressed) moveX += 1f;
        if (Keyboard.current.aKey.isPressed) moveX -= 1f;
        if (Keyboard.current.wKey.isPressed) moveZ += 1f;
        if (Keyboard.current.sKey.isPressed) moveZ -= 1f;

        inputs = new Vector2(moveX, moveZ).normalized;

        estaACorrer =
            Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.rightShiftKey.isPressed;

        estaAgachado =
            Keyboard.current.leftCtrlKey.isPressed ||
            Keyboard.current.rightCtrlKey.isPressed;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && EstaNoChao() && !estaAgachado)
        {
            querSaltar = true;
        }
    }

    private void FixedUpdate()
    {
        if (cameraTransform == null) return;

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
            rb.AddForce(Vector3.up * forcaSalto, ForceMode.Impulse);
            querSaltar = false;
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
}