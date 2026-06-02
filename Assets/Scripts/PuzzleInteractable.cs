using UnityEngine;

public class PuzzleInteractable : MonoBehaviour
{
    public GameObject puzzlePanel;
    public GameObject interactText;
    public MovimentoPlayer playerMovement;

    private bool playerInside;

    void Start()
    {
        if (interactText != null)
            interactText.SetActive(false);
    }

    void Update()
    {
        if (playerInside)
        {
            if (interactText != null)
                interactText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenPuzzle();
            }
        }
        else
        {
            if (interactText != null)
                interactText.SetActive(false);
        }

        if (puzzlePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle();
        }
    }

    void OpenPuzzle()
    {
        puzzlePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerMovement.enabled = false;

        if (interactText != null)
            interactText.SetActive(false);
    }

    void ClosePuzzle()
    {
        puzzlePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerMovement.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (interactText != null)
                interactText.SetActive(false);
        }
    }
}