using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentoPlayer : MonoBehaviour
{
    public float velocidade = 6.0f;
    public Transform cameraTransform; // Arraste a Main Camera para aqui
    private Rigidbody rb;
    private Vector2 inputs;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            float moveX = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);
            float moveZ = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);
            inputs = new Vector2(moveX, moveZ).normalized;
        }
    }

    void FixedUpdate()
    {
        // Pega a direção da câmera mas ignora a inclinação (Y)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calcula a direção baseada na visão da câmera
        Vector3 direcaoDesejada = (forward * inputs.y + right * inputs.x);

        rb.MovePosition(rb.position + direcaoDesejada * velocidade * Time.fixedDeltaTime);

        // Faz o boneco olhar para onde se move
        if (direcaoDesejada != Vector3.zero)
        {
            transform.forward = direcaoDesejada;
        }
    }
}