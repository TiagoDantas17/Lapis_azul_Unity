using UnityEngine;
using UnityEngine.InputSystem;


public class ControleCamera : MonoBehaviour
{
    public Transform alvo;
    public float sensibilidade = 2.0f;
    public float distanciaMax = 5.0f;
    public LayerMask camadasObstaculos; // No Inspector, selecione "Default"


    private float rotacaoX = 0f;
    private float rotacaoY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (alvo == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * sensibilidade * 0.1f;
        rotacaoY += mouseDelta.x;
        rotacaoX -= mouseDelta.y;
        rotacaoX = Mathf.Clamp(rotacaoX, -20f, 60f);

        Quaternion rotacao = Quaternion.Euler(rotacaoX, rotacaoY, 0);
        Vector3 direcao = rotacao * Vector3.forward;

        // POSIÇÃO IDEAL
        Vector3 posicaoDesejada = alvo.position - (direcao * distanciaMax);

        // CHECAR COLISÃO (Para não atravessar)
        RaycastHit hit;
        if (Physics.SphereCast(alvo.position, 0.2f, -direcao, out hit, distanciaMax, camadasObstaculos))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = posicaoDesejada;
        }

        transform.LookAt(alvo.position + Vector3.up);
    }
}