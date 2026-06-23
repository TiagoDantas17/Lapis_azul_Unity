using UnityEngine;
using UnityEngine.InputSystem;

public class BookShelfPuzzleInteractable : MonoBehaviour
{
    public static bool PuzzleAberto { get; private set; }

    [Header("Livro necessário")]
    public string livroNecessario = "Livro_Censurado";

    [Header("Puzzle")]
    public GameObject puzzlePanel;
    public BookshelfOrderPuzzle puzzleManager;

    [Header("UI")]
    public GameObject interactText;
    public GameObject faltaLivroText;

    [Header("Player")]
    public MovimentoPlayer playerMovement;
    public ControleCamera cameraControl;

    private bool playerInside = false;
    private Inventory inventory;

    private void Start()
    {
        if (interactText != null)
            interactText.SetActive(false);

        if (faltaLivroText != null)
            faltaLivroText.SetActive(false);

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);
    }

    private void Update()
    {
        if (!playerInside) return;

        if (puzzlePanel != null && puzzlePanel.activeSelf)
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                FecharPuzzle();
            }

            return;
        }

        if (interactText != null)
        {
            interactText.SetActive(true);
        }

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TentarAbrirPuzzle();
        }
    }

    private void TentarAbrirPuzzle()
    {
        if (inventory == null)
        {
            Debug.LogWarning("O Player não tem Inventory.");
            return;
        }

        if (!inventory.HasItem(livroNecessario))
        {
            MostrarFaltaLivro();
            return;
        }

        AbrirPuzzle();
    }

    private void AbrirPuzzle()
    {
        PuzzleAberto = true;

        if (puzzlePanel != null)
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

        if (puzzleManager != null)
            puzzleManager.IniciarPuzzle(inventory, this);
    }

    public void FecharPuzzle()
    {
        PuzzleAberto = false;

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (cameraControl != null)
            cameraControl.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void MostrarFaltaLivro()
    {
        if (faltaLivroText != null)
        {
            faltaLivroText.SetActive(true);
            Invoke(nameof(EsconderFaltaLivro), 2f);
        }
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

            if (playerMovement == null)
                playerMovement = other.GetComponent<MovimentoPlayer>();

            if (cameraControl == null && Camera.main != null)
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