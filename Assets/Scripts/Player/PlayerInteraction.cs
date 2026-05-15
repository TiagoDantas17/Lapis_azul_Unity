using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cameraPlayer;
    public float distanciaInteracao = 3f;
    public LayerMask layerInteracao;

    private IInteractable objetoAtual;

    void Update()
    {
        VerificarInteracao();

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            objetoAtual?.Interagir();
        }
    }

    void VerificarInteracao()
    {
        Ray ray = new Ray(cameraPlayer.transform.position, cameraPlayer.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distanciaInteracao, layerInteracao))
        {
            objetoAtual = hit.collider.GetComponent<IInteractable>();

            if (objetoAtual != null)
            {
                Debug.Log(objetoAtual.GetNomeInteracao());
            }
        }
        else
        {
            objetoAtual = null;
        }
    }
}