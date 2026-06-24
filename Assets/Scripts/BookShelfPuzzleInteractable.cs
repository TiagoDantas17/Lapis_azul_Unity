using UnityEngine;
using UnityEngine.InputSystem;

public class BookShelfPuzzleInteractable : MonoBehaviour
{
    [Header("Livro necessário")]
    public string livroNecessario = "Livro_Censurado";
    public bool exigirLivro = true;

    [Header("Puzzle")]
    public GameObject puzzlePanel;
    public BookshelfOrderPuzzle puzzleManager;

    [Header("UI")]
    public GameObject interactText;
    public GameObject faltaLivroText;

    private bool playerInside;
    private Inventory inventory;
    private MovimentoPlayer playerMovement;
    private ControleCamera cameraControl;

    private void Start()
    {
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (interactText != null)
            interactText.SetActive(false);

        if (faltaLivroText != null)
            faltaLivroText.SetActive(false);
    }

    private void Update()
    {
        if (!playerInside) return;
        if (Keyboard.current == null) return;

        if (puzzlePanel != null && puzzlePanel.activeSelf)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                FecharPuzzle();
            }

            return;
        }

        if (interactText != null)
            interactText.SetActive(true);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TentarAbrirPuzzle();
        }
    }

    private void TentarAbrirPuzzle()
    {
        if (inventory == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                inventory = player.GetComponent<Inventory>();
        }

        if (inventory == null)
        {
            Debug.LogWarning("Não encontrei o Inventory no Player.");
            return;
        }

        if (exigirLivro && !inventory.HasItem(livroNecessario))
        {
            Debug.Log("Falta o livro necessário: " + livroNecessario);

            if (faltaLivroText != null)
            {
                faltaLivroText.SetActive(true);
                Invoke(nameof(EsconderFaltaLivro), 2f);
            }

            return;
        }

        AbrirPuzzle();
    }

    private void AbrirPuzzle()
    {
        if (puzzlePanel == null)
        {
            Debug.LogWarning("Falta ligar o Puzzle Panel no BookShelfPuzzleInteractable.");
            return;
        }

        puzzlePanel.SetActive(true);

        if (interactText != null)
            interactText.SetActive(false);

        if (faltaLivroText != null)
            faltaLivroText.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (cameraControl != null)
            cameraControl.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (puzzleManager == null)
            puzzleManager = puzzlePanel.GetComponent<BookshelfOrderPuzzle>();

        if (puzzleManager != null)
            puzzleManager.IniciarPuzzle(inventory, this);
        else
            Debug.LogWarning("Falta ligar o Puzzle Manager no BookShelfPuzzleInteractable.");
    }

    public void FecharPuzzle()
    {
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (cameraControl != null)
            cameraControl.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void EsconderFaltaLivro()
    {
        if (faltaLivroText != null)
            faltaLivroText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            inventory = other.GetComponent<Inventory>();
            playerMovement = other.GetComponent<MovimentoPlayer>();

            if (Camera.main != null)
                cameraControl = Camera.main.GetComponent<ControleCamera>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (interactText != null)
                interactText.SetActive(false);

            if (faltaLivroText != null)
                faltaLivroText.SetActive(false);
        }
    }
}