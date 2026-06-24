using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookshelfOrderPuzzle : MonoBehaviour
{
    [Header("ID")]
    public string puzzleID = "Puzzle_Estante_Click_Final_02";

    [Header("Inventário")]
    public string livroCorreto = "Livro_Censurado";
    public string itemRecompensa = "Pista_Estante";

    [Header("UI")]
    public TMP_Text textoEstado;
    public GameObject painelResolvido;

    [Header("Botões")]
    public Button botaoEspacoVazio;
    public Button botaoVoltar;

    [Header("Mundo")]
    public GameObject livroVisualNaEstante;

    private Inventory inventory;
    private BookShelfPuzzleInteractable interacao;
    private bool resolvido;

    private void Awake()
    {
        LigarBotoes();
    }

    private void OnEnable()
    {
        LigarBotoes();
    }

    private void Start()
    {
        if (painelResolvido != null)
            painelResolvido.SetActive(false);

        if (PlayerPrefs.GetInt(puzzleID, 0) == 1)
        {
            resolvido = true;

            if (livroVisualNaEstante != null)
                livroVisualNaEstante.SetActive(true);
        }
        else
        {
            resolvido = false;

            if (livroVisualNaEstante != null)
                livroVisualNaEstante.SetActive(false);
        }
    }

    private void LigarBotoes()
    {
        if (botaoEspacoVazio != null)
        {
            botaoEspacoVazio.onClick.RemoveAllListeners();
            botaoEspacoVazio.onClick.AddListener(ResolverPuzzleDireto);
        }

        if (botaoVoltar != null)
        {
            botaoVoltar.onClick.RemoveAllListeners();
            botaoVoltar.onClick.AddListener(FecharPuzzle);
        }
    }

    public void IniciarPuzzle(Inventory novoInventory, BookShelfPuzzleInteractable novaInteracao)
    {
        inventory = novoInventory;
        interacao = novaInteracao;

        LigarBotoes();

        if (PlayerPrefs.GetInt(puzzleID, 0) == 1)
        {
            resolvido = true;
            MostrarResolvido();
            return;
        }

        resolvido = false;

        if (painelResolvido != null)
            painelResolvido.SetActive(false);

        if (textoEstado != null)
            textoEstado.text = "Escolhe onde colocar o Livro Censurado.";
    }

    public void ResolverPuzzleDireto()
    {
        if (resolvido) return;

        resolvido = true;

        Debug.Log("PUZZLE DA ESTANTE RESOLVIDO.");

        PlayerPrefs.SetInt(puzzleID, 1);
        PlayerPrefs.Save();

        if (livroVisualNaEstante != null)
            livroVisualNaEstante.SetActive(true);
        else
            Debug.LogWarning("Falta ligar o Livro Visual Na Estante.");

        if (inventory == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                inventory = player.GetComponent<Inventory>();
        }

        if (inventory != null)
        {
            inventory.RemoveItem(livroCorreto);

            if (!inventory.HasItem(itemRecompensa))
                inventory.AddItem(itemRecompensa);
        }
        else
        {
            Debug.LogWarning("Não encontrei o Inventory.");
        }

        MostrarResolvido();

        if (SaveGameManager.Instance != null)
            SaveGameManager.Instance.GuardarJogo();

        StartCoroutine(FecharDepois());
    }

    private void MostrarResolvido()
    {
        if (textoEstado != null)
            textoEstado.text = "O livro encaixa na estante. Encontraste uma pista.";

        if (painelResolvido != null)
            painelResolvido.SetActive(true);

        if (livroVisualNaEstante != null)
            livroVisualNaEstante.SetActive(true);
    }

    public void FecharPuzzle()
    {
        if (interacao != null)
            interacao.FecharPuzzle();
        else
            gameObject.SetActive(false);
    }

    private IEnumerator FecharDepois()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        FecharPuzzle();
    }
}