using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuController : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject optionsPanel;
    public GameObject mainMenuButtons;

    [Header("Gameplay")]
    public TMP_Dropdown dropdownDificuldade;
    public Slider sliderSensibilidade;

    [Header("Áudio")]
    public Slider sliderVolumeGeral;
    public Slider sliderMusica;
    public Slider sliderEfeitos;

    [Header("Gráficos")]
    public TMP_Dropdown dropdownQualidade;
    public TMP_Dropdown dropdownResolucao;
    public Toggle toggleEcraInteiro;

    private Resolution[] resolucoes;

    private void Start()
    {
        PrepararDropdownDificuldade();
        PrepararDropdownQualidade();
        PrepararDropdownResolucao();

        CarregarOpcoes();

        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    private void PrepararDropdownDificuldade()
    {
        if (dropdownDificuldade == null) return;

        dropdownDificuldade.ClearOptions();

        dropdownDificuldade.AddOptions(new List<string>
        {
            "Fácil",
            "Normal",
            "Difícil"
        });
    }

    private void PrepararDropdownQualidade()
    {
        if (dropdownQualidade == null) return;

        dropdownQualidade.ClearOptions();

        List<string> opcoesQualidade = new List<string>(QualitySettings.names);
        dropdownQualidade.AddOptions(opcoesQualidade);
        dropdownQualidade.value = QualitySettings.GetQualityLevel();
    }

    private void PrepararDropdownResolucao()
    {
        if (dropdownResolucao == null) return;

        resolucoes = Screen.resolutions;

        dropdownResolucao.ClearOptions();

        List<string> opcoesResolucao = new List<string>();

        int resolucaoAtualIndex = 0;

        for (int i = 0; i < resolucoes.Length; i++)
        {
            string opcao = resolucoes[i].width + " x " + resolucoes[i].height;
            opcoesResolucao.Add(opcao);

            if (resolucoes[i].width == Screen.currentResolution.width &&
                resolucoes[i].height == Screen.currentResolution.height)
            {
                resolucaoAtualIndex = i;
            }
        }

        dropdownResolucao.AddOptions(opcoesResolucao);
        dropdownResolucao.value = resolucaoAtualIndex;
        dropdownResolucao.RefreshShownValue();
    }

    public void AbrirOpcoes()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }

        if (mainMenuButtons != null)
        {
            mainMenuButtons.SetActive(false);
        }
    }

    public void FecharOpcoes()
    {
        GuardarOpcoes();

        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }

        if (mainMenuButtons != null)
        {
            mainMenuButtons.SetActive(true);
        }
    }

    public void GuardarOpcoes()
    {
        if (dropdownDificuldade != null)
            PlayerPrefs.SetInt("Dificuldade", dropdownDificuldade.value);

        if (sliderSensibilidade != null)
            PlayerPrefs.SetFloat("Sensibilidade", sliderSensibilidade.value);

        if (sliderVolumeGeral != null)
            PlayerPrefs.SetFloat("VolumeGeral", sliderVolumeGeral.value);

        if (sliderMusica != null)
            PlayerPrefs.SetFloat("VolumeMusica", sliderMusica.value);

        if (sliderEfeitos != null)
            PlayerPrefs.SetFloat("VolumeEfeitos", sliderEfeitos.value);

        if (dropdownQualidade != null)
            PlayerPrefs.SetInt("Qualidade", dropdownQualidade.value);

        if (dropdownResolucao != null)
            PlayerPrefs.SetInt("Resolucao", dropdownResolucao.value);

        if (toggleEcraInteiro != null)
            PlayerPrefs.SetInt("EcraInteiro", toggleEcraInteiro.isOn ? 1 : 0);

        PlayerPrefs.Save();

        AplicarOpcoes();
    }

    public void CarregarOpcoes()
    {
        if (dropdownDificuldade != null)
            dropdownDificuldade.value = PlayerPrefs.GetInt("Dificuldade", 1);

        if (sliderSensibilidade != null)
            sliderSensibilidade.value = PlayerPrefs.GetFloat("Sensibilidade", 1f);

        if (sliderVolumeGeral != null)
            sliderVolumeGeral.value = PlayerPrefs.GetFloat("VolumeGeral", 1f);

        if (sliderMusica != null)
            sliderMusica.value = PlayerPrefs.GetFloat("VolumeMusica", 1f);

        if (sliderEfeitos != null)
            sliderEfeitos.value = PlayerPrefs.GetFloat("VolumeEfeitos", 1f);

        if (dropdownQualidade != null)
            dropdownQualidade.value = PlayerPrefs.GetInt("Qualidade", QualitySettings.GetQualityLevel());

        if (dropdownResolucao != null)
            dropdownResolucao.value = PlayerPrefs.GetInt("Resolucao", dropdownResolucao.value);

        if (toggleEcraInteiro != null)
            toggleEcraInteiro.isOn = PlayerPrefs.GetInt("EcraInteiro", Screen.fullScreen ? 1 : 0) == 1;

        AplicarOpcoes();
    }

    public void AplicarOpcoes()
    {
        if (sliderVolumeGeral != null)
        {
            AudioListener.volume = sliderVolumeGeral.value;
        }

        if (dropdownQualidade != null)
        {
            QualitySettings.SetQualityLevel(dropdownQualidade.value);
        }

        if (dropdownResolucao != null && toggleEcraInteiro != null && resolucoes != null && resolucoes.Length > 0)
        {
            int index = Mathf.Clamp(dropdownResolucao.value, 0, resolucoes.Length - 1);

            Resolution resolucao = resolucoes[index];

            Screen.SetResolution(
                resolucao.width,
                resolucao.height,
                toggleEcraInteiro.isOn
            );
        }
    }

    public void ReporOpcoes()
    {
        if (dropdownDificuldade != null)
            dropdownDificuldade.value = 1;

        if (sliderSensibilidade != null)
            sliderSensibilidade.value = 1f;

        if (sliderVolumeGeral != null)
            sliderVolumeGeral.value = 1f;

        if (sliderMusica != null)
            sliderMusica.value = 1f;

        if (sliderEfeitos != null)
            sliderEfeitos.value = 1f;

        if (dropdownQualidade != null)
            dropdownQualidade.value = QualitySettings.names.Length - 1;

        if (toggleEcraInteiro != null)
            toggleEcraInteiro.isOn = true;

        GuardarOpcoes();
    }
}