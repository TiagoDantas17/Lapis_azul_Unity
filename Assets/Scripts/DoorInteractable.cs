using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteractable : MonoBehaviour
{
    [Header("Porta")]
    public Transform porta; // Aqui metes o DoorPivot_Cela
    public float anguloAberta = 90f;
    public float velocidadeAbrir = 3f;

    [Header("Chave")]
    public bool precisaChave = true;
    public string nomeChave = "Key";

    [Header("UI")]
    public GameObject textoInteracao;
    public GameObject textoTrancada;

    private bool playerInside = false;
    private bool portaAberta = false;
    private Inventory inventory;

    private Quaternion rotacaoFechada;
    private Quaternion rotacaoAberta;

    private void Start()
    {
        if (porta == null)
        {
            porta = transform;
        }

        rotacaoFechada = porta.rotation;

        rotacaoAberta = Quaternion.Euler(
            porta.eulerAngles.x,
            porta.eulerAngles.y + anguloAberta,
            porta.eulerAngles.z
        );

        if (textoInteracao != null)
        {
            textoInteracao.SetActive(false);
        }

        if (textoTrancada != null)
        {
            textoTrancada.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInside && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TentarAbrirPorta();
        }

        Quaternion rotacaoAlvo = portaAberta ? rotacaoAberta : rotacaoFechada;

        porta.rotation = Quaternion.Lerp(
            porta.rotation,
            rotacaoAlvo,
            Time.deltaTime * velocidadeAbrir
        );
    }

    private void TentarAbrirPorta()
    {
        if (precisaChave)
        {
            if (inventory == null || !inventory.HasItem(nomeChave))
            {
                MostrarTrancada();
                return;
            }
        }

        portaAberta = !portaAberta;

        if (textoTrancada != null)
        {
            textoTrancada.SetActive(false);
        }
    }

    private void MostrarTrancada()
    {
        if (textoTrancada != null)
        {
            textoTrancada.SetActive(true);
            Invoke(nameof(EsconderTextoTrancada), 2f);
        }
    }

    private void EsconderTextoTrancada()
    {
        if (textoTrancada != null)
        {
            textoTrancada.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            inventory = other.GetComponent<Inventory>();

            if (textoInteracao != null)
            {
                textoInteracao.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (textoInteracao != null)
            {
                textoInteracao.SetActive(false);
            }

            if (textoTrancada != null)
            {
                textoTrancada.SetActive(false);
            }
        }
    }
}