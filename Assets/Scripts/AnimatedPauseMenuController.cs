using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AnimatedPauseMenuController : MonoBehaviour
{
    public static bool JogoPausado { get; private set; }

    [Header("Painéis")]
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public GameObject trophiesPanel;

    [Header("Cenas")]
    public string nomeCenaMenu = "MainMenu";

    private void Start()
    {
        Time.timeScale = 1f;
        JogoPausado = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        if (trophiesPanel != null)
            trophiesPanel.SetActive(false);

        BloquearRato();
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (JogoPausado)
            {
                Continuacao();
            }
            else
            {
                AbrirPausa();
            }
        }
    }

    public void AbrirPausa()
    {
        JogoPausado = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        if (trophiesPanel != null)
            trophiesPanel.SetActive(false);

        DesbloquearRato();
    }

    public void Continuacao()
    {
        JogoPausado = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        if (trophiesPanel != null)
            trophiesPanel.SetActive(false);

        BloquearRato();
    }

    public void AbrirOpcoes()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(true);

        if (trophiesPanel != null)
            trophiesPanel.SetActive(false);
    }

    public void AbrirTrofeus()
    {
        if (trophiesPanel != null)
            trophiesPanel.SetActive(true);

        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    public void VoltarAoMenuPausa()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);

        if (trophiesPanel != null)
            trophiesPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void Sair()
    {
        Time.timeScale = 1f;
        JogoPausado = false;

        SceneManager.LoadScene(nomeCenaMenu);
    }

    public void SairDoJogoMesmo()
    {
        Time.timeScale = 1f;
        JogoPausado = false;

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void BloquearRato()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void DesbloquearRato()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}