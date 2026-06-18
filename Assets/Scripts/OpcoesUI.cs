using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpcoesUI : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject painelOpcoes;
    public GameObject painelAnterior;

    [Header("Áudio")]
    public Slider volumeGeralSlider;

    [Header("Gameplay")]
    public Slider sensibilidadeSlider;

    [Header("Gráficos")]
    public TMP_Dropdown qualidadeDropdown;
    public TMP_Dropdown resolucaoDropdown;
    public Toggle ecraInteiroToggle;

    private Resolution[] resolucoes;
    private bool inicializado = false;

    private void Start()
    {
        Inicializar();
        CarregarOpcoes();
    }

    private void Inicializar()
    {
        if (inicializado) return;

        inicializado = true;

        PrepararQualidade();
        PrepararResolucoes();
    }

    private void PrepararQualidade()
    {
        if (qualidadeDropdown == null) return;

        qualidadeDropdown.ClearOptions();

        List<string> opcoes = new List<string>();

        foreach (string nome in QualitySettings.names)
        {
            opcoes.Add(nome);
        }

        qualidadeDropdown.AddOptions(opcoes);
    }

    private void PrepararResolucoes()
    {
        if (resolucaoDropdown == null) return;

        resolucoes = Screen.resolutions;

        resolucaoDropdown.ClearOptions();

        List<string> opcoes = new List<string>();

        int resolucaoAtualIndex = 0;

        for (int i = 0; i < resolucoes.Length; i++)
        {
            string opcao = resolucoes[i].width + " x " + resolucoes[i].height + " @ " + resolucoes[i].refreshRateRatio.value.ToString("0") + "Hz";
            opcoes.Add(opcao);

            if (resolucoes[i].width == Screen.currentResolution.width &&
                resolucoes[i].height == Screen.currentResolution.height)
            {
                resolucaoAtualIndex = i;
            }
        }

        resolucaoDropdown.AddOptions(opcoes);
        resolucaoDropdown.value = resolucaoAtualIndex;
        resolucaoDropdown.RefreshShownValue();
    }

    public void AbrirOpcoes()
    {
        Inicializar();
        CarregarOpcoes();

        if (painelAnterior != null)
        {
            painelAnterior.SetActive(false);
        }

        if (painelOpcoes != null)
        {
            painelOpcoes.SetActive(true);
        }
    }

    public void FecharOpcoes()
    {
        if (painelOpcoes != null)
        {
            painelOpcoes.SetActive(false);
        }

        if (painelAnterior != null)
        {
            painelAnterior.SetActive(true);
        }
    }

    public void GuardarOpcoes()
    {
        if (volumeGeralSlider != null)
        {
            PlayerPrefs.SetFloat("VolumeGeral", volumeGeralSlider.value);
            AudioListener.volume = volumeGeralSlider.value;
        }

        if (sensibilidadeSlider != null)
        {
            PlayerPrefs.SetFloat("SensibilidadeCamera", sensibilidadeSlider.value);
        }

        if (qualidadeDropdown != null)
        {
            PlayerPrefs.SetInt("Qualidade", qualidadeDropdown.value);
            QualitySettings.SetQualityLevel(qualidadeDropdown.value);
        }

        if (resolucaoDropdown != null && resolucoes != null && resolucoes.Length > 0)
        {
            PlayerPrefs.SetInt("Resolucao", resolucaoDropdown.value);

            Resolution resolucao = resolucoes[resolucaoDropdown.value];
            bool ecraInteiro = ecraInteiroToggle != null && ecraInteiroToggle.isOn;

            Screen.SetResolution(resolucao.width, resolucao.height, ecraInteiro);
        }

        if (ecraInteiroToggle != null)
        {
            PlayerPrefs.SetInt("EcraInteiro", ecraInteiroToggle.isOn ? 1 : 0);
            Screen.fullScreen = ecraInteiroToggle.isOn;
        }

        PlayerPrefs.Save();

        Debug.Log("Opções guardadas.");
    }

    public void CarregarOpcoes()
    {
        Inicializar();

        float volume = PlayerPrefs.GetFloat("VolumeGeral", 1f);
        float sensibilidade = PlayerPrefs.GetFloat("SensibilidadeCamera", 2f);
        int qualidade = PlayerPrefs.GetInt("Qualidade", QualitySettings.GetQualityLevel());
        int resolucao = PlayerPrefs.GetInt("Resolucao", resolucaoDropdown != null ? resolucaoDropdown.value : 0);
        bool ecraInteiro = PlayerPrefs.GetInt("EcraInteiro", Screen.fullScreen ? 1 : 0) == 1;

        if (volumeGeralSlider != null)
        {
            volumeGeralSlider.value = volume;
        }

        if (sensibilidadeSlider != null)
        {
            sensibilidadeSlider.value = sensibilidade;
        }

        if (qualidadeDropdown != null)
        {
            qualidadeDropdown.value = qualidade;
            qualidadeDropdown.RefreshShownValue();
        }

        if (resolucaoDropdown != null && resolucoes != null && resolucoes.Length > 0)
        {
            resolucao = Mathf.Clamp(resolucao, 0, resolucoes.Length - 1);
            resolucaoDropdown.value = resolucao;
            resolucaoDropdown.RefreshShownValue();
        }

        if (ecraInteiroToggle != null)
        {
            ecraInteiroToggle.isOn = ecraInteiro;
        }

        AplicarOpcoesSemGuardar();
    }

    public void ReporOpcoes()
    {
        if (volumeGeralSlider != null)
        {
            volumeGeralSlider.value = 1f;
        }

        if (sensibilidadeSlider != null)
        {
            sensibilidadeSlider.value = 2f;
        }

        if (qualidadeDropdown != null)
        {
            qualidadeDropdown.value = QualitySettings.names.Length - 1;
            qualidadeDropdown.RefreshShownValue();
        }

        if (resolucaoDropdown != null && resolucoes != null && resolucoes.Length > 0)
        {
            resolucaoDropdown.value = resolucoes.Length - 1;
            resolucaoDropdown.RefreshShownValue();
        }

        if (ecraInteiroToggle != null)
        {
            ecraInteiroToggle.isOn = true;
        }

        GuardarOpcoes();
    }

    private void AplicarOpcoesSemGuardar()
    {
        if (volumeGeralSlider != null)
        {
            AudioListener.volume = volumeGeralSlider.value;
        }

        if (qualidadeDropdown != null)
        {
            QualitySettings.SetQualityLevel(qualidadeDropdown.value);
        }

        if (ecraInteiroToggle != null)
        {
            Screen.fullScreen = ecraInteiroToggle.isOn;
        }
    }
}