using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    public static bool JogoPausado { get; private set; }

    [Header("Painéis")]
    public GameObject pausePanel;

    [Header("Cenas")]
    public string nomeCenaMenu = "MainMenu";

    private void Start()
    {
        JogoPausado = false;

        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (JogoPausado)
            {
                ContinuarJogo();
            }
            else
            {
                PausarJogo();
            }
        }
    }

    public void PausarJogo()
    {
        JogoPausado = true;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinuarJogo()
    {
        JogoPausado = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void VoltarMenuPrincipal()
    {
        Time.timeScale = 1f;
        JogoPausado = false;

        SceneManager.LoadScene(nomeCenaMenu);
    }

    public void SairDoJogo()
    {
        Time.timeScale = 1f;
        JogoPausado = false;

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
