using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MovimentoPlayer : MonoBehaviour
{
    [Header("=== MOVIMENTO ===")]
    public float velocidade = 6.0f;
    public float velocidadeCorrer = 10.0f;
    public float velocidadeAgachado = 3.0f;

    [Header("=== SALTO ===")]
    public float forcaSalto = 8.0f;
    public LayerMask camadaChao = 1; // Default layer
    public float distanciaChao = 0.9f;

    [Header("=== AGACHAR ===")]
    public Transform cameraTransform; // Arraste a CÂMERA aqui
    public float alturaAgachadoMultiplicador = 0.5f;
    public float baixarCameraAgachado = 0.8f;

    [Header("=== FISICA ===")]
    public float atritoChao = 5f;
    public float rotacaoVelocidade = 10f;

    // Privadas
    private Rigidbody rb;
    private CapsuleCollider capsule;

    private Vector2 inputs;
    private bool querSaltar;
    private bool estaAgachado;
    private bool estaACorrer;

    private float alturaNormal;
    private Vector3 centroNormal;
    private Vector3 posicaoCameraNormal;

    [SerializeField] private float crouchFix = 0.25f;

    void Start()
    {
        // Components
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        // Configurar Rigidbody
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Salvar valores normais
        alturaNormal = capsule.height;
        centroNormal = capsule.center;


        if (cameraTransform != null)

        {
            posicaoCameraNormal = cameraTransform.localPosition;
        }

        Debug.Log("✅ Player Controller iniciado!");
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // === INPUTS ===
        // WASD
        float moveX = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);
        float moveZ = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);
        inputs = new Vector2(moveX, moveZ).normalized;

        // Controles
        estaACorrer = Keyboard.current.leftShiftKey.isPressed && inputs.magnitude > 0.1f;
        estaAgachado = Keyboard.current.leftCtrlKey.isPressed;

        // Salto
        if (Keyboard.current.spaceKey.wasPressedThisFrame
        && !estaAgachado
        && Mathf.Abs(rb.linearVelocity.y) < 0.05f)

        {
            querSaltar = true;
        }

        // Agachamento
        AtualizarAgachamento();
    }

    void FixedUpdate()
    {
        if (cameraTransform == null) return;

        // === DIREÇÃO BASEADA NA CÂMERA ===
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0; right.y = 0;
        forward.Normalize(); right.Normalize();

        Vector3 direcaoDesejada = (forward * inputs.y + right * inputs.x).normalized;

        // === VELOCIDADE ===
        float velAtual = estaAgachado ? velocidadeAgachado :
                        (estaACorrer ? velocidadeCorrer : velocidade);

        Vector3 velocidadeAlvo = direcaoDesejada * velAtual;
        velocidadeAlvo.y = rb.linearVelocity.y;

        // Aplicar movimento com atrito
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, velocidadeAlvo, Time.fixedDeltaTime * atritoChao);

        // === ROTAÇÃO ===
        if (direcaoDesejada.magnitude > 0.1f)
        {
            Quaternion rotAlvo = Quaternion.LookRotation(direcaoDesejada);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotAlvo, Time.fixedDeltaTime * rotacaoVelocidade);
        }

        // === SALTO ===
        if (querSaltar)
        {
            rb.linearVelocity = new Vector3(
    rb.linearVelocity.x,
    rb.linearVelocity.y,
    rb.linearVelocity.z
);
            rb.AddForce(Vector3.up * forcaSalto, ForceMode.Impulse);
            querSaltar = false;
        }
    }

    bool EstaNoChao()
    {
        Vector3 origem = transform.position + capsule.center + Vector3.up * (capsule.height * 0.5f - 0.1f);
        float distancia = distanciaChao + 0.1f;

        bool noChao = Physics.Raycast(origem, Vector3.down, distancia, camadaChao);

        // Debug visual
        Debug.DrawRay(origem, Vector3.down * distancia, noChao ? Color.green : Color.red, 0.1f);

        return noChao;
    }

    
void AtualizarAgachamento()
    {
        // Altura alvo
        float alvoAltura = estaAgachado ? alturaNormal * alturaAgachadoMultiplicador : alturaNormal;

        // Suavizar altura
        capsule.height = /*Mathf.Lerp(capsule.height, alvoAltura, Time.deltaTime * 12f)*/  alvoAltura;

        // Ajustar centro
        float diff = (alturaNormal - capsule.height) * crouchFix;
        capsule.center = Vector3.down * diff;

        // === CÂMERA ===
        if (cameraTransform != null)
        {
            Vector3 posAlvo = estaAgachado
                ? posicaoCameraNormal + Vector3.down * baixarCameraAgachado
                : posicaoCameraNormal;

            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                posAlvo,
                Time.deltaTime * 12f
            );
        }
    }

    // Debug info
    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.Label(new Rect(10, 10, 300, 20), $"Vel: {rb.linearVelocity.magnitude:F1} | NoChao: {EstaNoChao()} | Agachado: {estaAgachado}");
    }
}