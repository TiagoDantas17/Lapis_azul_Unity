using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MovimentoPlayer : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 6.0f;
    public float velocidadeCorrer = 10.0f;
    public float velocidadeAgachado = 3.0f;

    [Header("Salto")]
    public float forcaSalto = 6.0f;
    public LayerMask camadaChao;
    public float distanciaChao = 0.2f;

    [Header("Câmara")]
    public Transform cameraTransform; // Main Camera
    public Transform cameraParaAgachar; // opcional, pode ser a PlayerView/câmara se for filha do Player
    public float baixarCameraAgachado = 0.5f;

    [Header("Agachar")]
    public float alturaAgachadoPercentagem = 0.55f;

    private Rigidbody rb;
    private CapsuleCollider capsule;

    private Vector2 inputs;
    private bool querSaltar;
    private bool estaAgachado;
    private bool estaACorrer;

    private float alturaNormal;
    private Vector3 centroNormal;
    private float alturaAgachado;

    private Vector3 posicaoCameraNormal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;

        alturaNormal = capsule.height;
        centroNormal = capsule.center;
        alturaAgachado = alturaNormal * alturaAgachadoPercentagem;

        if (cameraParaAgachar != null)
        {
            posicaoCameraNormal = cameraParaAgachar.localPosition;
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        float moveX = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);
        float moveZ = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);

        inputs = new Vector2(moveX, moveZ).normalized;

        bool shiftPressionado =
            Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.rightShiftKey.isPressed;

        bool ctrlPressionado =
            Keyboard.current.leftCtrlKey.isPressed ||
            Keyboard.current.rightCtrlKey.isPressed;

        estaAgachado = ctrlPressionado;
        estaACorrer = shiftPressionado && !estaAgachado;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && EstaNoChao() && !estaAgachado)
        {
            querSaltar = true;
        }

        AtualizarAgachamento();
    }

    void FixedUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 direcaoDesejada = forward * inputs.y + right * inputs.x;

        float velocidadeAtual = velocidade;

        if (estaAgachado)
        {
            velocidadeAtual = velocidadeAgachado;
        }
        else if (estaACorrer)
        {
            velocidadeAtual = velocidadeCorrer;
        }

        rb.MovePosition(rb.position + direcaoDesejada * velocidadeAtual * Time.fixedDeltaTime);

        if (direcaoDesejada != Vector3.zero)
        {
            Quaternion rotacao = Quaternion.LookRotation(direcaoDesejada);
            rb.MoveRotation(rotacao);
        }

        if (querSaltar)
        {
            rb.AddForce(Vector3.up * forcaSalto, ForceMode.Impulse);
            querSaltar = false;
        }
    }

    bool EstaNoChao()
    {
        Vector3 origem = transform.position + Vector3.up * 0.1f;
        float tamanhoRaio = (capsule.height / 2f) + distanciaChao;

        return Physics.Raycast(origem, Vector3.down, tamanhoRaio, camadaChao);
    }

    void AtualizarAgachamento()
    {
        float alturaAlvo = estaAgachado ? alturaAgachado : alturaNormal;

        capsule.height = Mathf.Lerp(capsule.height, alturaAlvo, Time.deltaTime * 10f);

        float diferencaAltura = alturaNormal - capsule.height;
        capsule.center = centroNormal - new Vector3(0, diferencaAltura / 2f, 0);

        if (cameraParaAgachar != null)
        {
            Vector3 posicaoAlvo = posicaoCameraNormal;

            if (estaAgachado)
            {
                posicaoAlvo += Vector3.down * baixarCameraAgachado;
            }

            cameraParaAgachar.localPosition = Vector3.Lerp(
                cameraParaAgachar.localPosition,
                posicaoAlvo,
                Time.deltaTime * 10f
            );
        }
    }
}