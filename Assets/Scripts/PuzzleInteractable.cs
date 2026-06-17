using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleInteractable : MonoBehaviour
{
    [Header("Puzzle")]
    public GameObject puzzlePanel;

    [Header("UI")]
    public GameObject interactText;

    [Header("Player")]
    public MovimentoPlayer playerMovement;

    private bool playerInside;

    void Start()
    {
        if (interactText != null)
        {
            interactText.SetActive(false);
        }

        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside)
        {
            if (interactText != null && puzzlePanel != null && !puzzlePanel.activeSelf)
            {
                interactText.SetActive(true);
            }

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                OpenPuzzle();
            }
        }
        else
        {
            if (interactText != null)
            {
                interactText.SetActive(false);
            }
        }

        if (puzzlePanel != null && puzzlePanel.activeSelf)
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                ClosePuzzle();
            }
        }
    }

    void OpenPuzzle()
    {
        if (puzzlePanel == null)
        {
            Debug.LogWarning("Falta ligar o Puzzle Panel no PuzzleInteractable.");
            return;
        }

        puzzlePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        else
        {
            Debug.LogWarning("Falta ligar o Player Movement no PuzzleInteractable.");
        }

        if (interactText != null)
        {
            interactText.SetActive(false);
        }
    }

    void ClosePuzzle()
    {
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (playerMovement == null)
            {
                playerMovement = other.GetComponent<MovimentoPlayer>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (interactText != null)
            {
                interactText.SetActive(false);
            }
        }
    }
}