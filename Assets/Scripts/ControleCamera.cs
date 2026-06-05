using UnityEngine;
using UnityEngine.InputSystem;

public class ControleCamera : MonoBehaviour
{
    [Header("Alvo da Câmara")]
    public Transform alvo;

    [Header("Mouse")]
    public float sensibilidade = 2.0f;

    [Header("Estilo Ombro / Resident Evil")]
    public float distancia = 2.4f;
    public float alturaExtra = 1.15f;
    public float offsetOmbro = 0.45f;
    public float olharParaFrente = 3.5f;

    [Header("Limites")]
    public float limiteBaixo = -25f;
    public float limiteCima = 45f;

    [Header("Suavidade")]
    public float suavidadePosicao = 18f;
    public float suavidadeRotacao = 18f;

    [Header("Colisão")]
    public LayerMask camadasObstaculos;
    public float raioColisao = 0.15f;
    public float afastarDaParede = 0.05f;

    private float rotacaoX = 10f;
    private float rotacaoY = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        // Se o jogo estiver pausado, a câmara não mexe
        if (PauseMenuController.JogoPausado) return;

        if (alvo == null) return;
        if (Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        rotacaoY += mouseDelta.x * sensibilidade * 0.1f;
        rotacaoX -= mouseDelta.y * sensibilidade * 0.1f;

        rotacaoX = Mathf.Clamp(rotacaoX, limiteBaixo, limiteCima);

        Quaternion rotacaoCompleta = Quaternion.Euler(rotacaoX, rotacaoY, 0f);
        Quaternion rotacaoHorizontal = Quaternion.Euler(0f, rotacaoY, 0f);

        Vector3 frenteHorizontal = rotacaoHorizontal * Vector3.forward;
        Vector3 direitaHorizontal = rotacaoHorizontal * Vector3.right;
        Vector3 frenteOlhar = rotacaoCompleta * Vector3.forward;

        Vector3 pontoAlvo = alvo.position + Vector3.up * alturaExtra;

        Vector3 posicaoDesejada =
            pontoAlvo
            - frenteHorizontal * distancia
            + direitaHorizontal * offsetOmbro;

        Vector3 pontoMira =
            pontoAlvo
            + frenteOlhar * olharParaFrente;

        Vector3 direcaoCamera = posicaoDesejada - pontoAlvo;
        float distanciaCamera = direcaoCamera.magnitude;

        if (camadasObstaculos.value != 0)
        {
            if (Physics.SphereCast(
                pontoAlvo,
                raioColisao,
                direcaoCamera.normalized,
                out RaycastHit hit,
                distanciaCamera,
                camadasObstaculos))
            {
                posicaoDesejada = hit.point - direcaoCamera.normalized * afastarDaParede;
            }
        }

        transform.position = Vector3.Lerp(
            transform.position,
            posicaoDesejada,
            Time.deltaTime * suavidadePosicao
        );

        Quaternion rotacaoDesejada = Quaternion.LookRotation(pontoMira - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotacaoDesejada,
            Time.deltaTime * suavidadeRotacao
        );
    }
}