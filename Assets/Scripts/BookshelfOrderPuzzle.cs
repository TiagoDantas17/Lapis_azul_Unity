using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookshelfOrderPuzzle : MonoBehaviour
{
    [Header("ID do puzzle")]
    public string puzzleID = "Puzzle_Estante_Livro_01";

    [Header("Ordem dos livros")]
    public string[] ordemInicial =
    {
        "Medo",
        "Censura",
        "Poeta",
        "Silêncio",
        "Liberdade"
    };

    public string[] ordemCorreta =
    {
        "Poeta",
        "Censura",
        "Silêncio",
        "Medo",
        "Liberdade"
    };

    [Header("Botões dos livros")]
    public Button[] botoesLivros;
    public TMP_Text[] textosLivros;

    [Header("Textos")]
    public TMP_Text textoEstado;
    public GameObject painelResolvido;

    [Header("Recompensa")]
    public bool adicionarRecompensaAoInventario = true;
    public string itemRecompensa = "Pista_Estante";

    [Header("Objetos opcionais")]
    public GameObject objetoParaAtivar;
    public GameObject objetoParaDesativar;

    private string[] ordemAtual;
    private int primeiroSelecionado = -1;
    private bool resolvido = false;

    private Inventory inventory;
    private BookShelfPuzzleInteractable interacao;

    private void Start()
    {
        if (painelResolvido != null)
            painelResolvido.SetActive(false);

        PrepararPuzzle();
        AtualizarUI();
    }

    public void IniciarPuzzle(Inventory novoInventory, BookShelfPuzzleInteractable novaInteracao)
    {
        inventory = novoInventory;
        interacao = novaInteracao;

        PrepararPuzzle();

        if (PlayerPrefs.GetInt(puzzleID, 0) == 1)
        {
            resolvido = true;
            MostrarResolvido();
        }
        else
        {
            resolvido = false;

            if (painelResolvido != null)
                painelResolvido.SetActive(false);

            if (textoEstado != null)
                textoEstado.text = "Organiza os livros na ordem certa.";
        }

        AtualizarUI();
    }

    private void PrepararPuzzle()
    {
        if (ordemAtual != null && ordemAtual.Length == ordemInicial.Length)
            return;

        ordemAtual = new string[ordemInicial.Length];

        for (int i = 0; i < ordemInicial.Length; i++)
        {
            ordemAtual[i] = ordemInicial[i];
        }
    }

    public void ClicarLivro(int index)
    {
        if (resolvido) return;
        if (index < 0 || index >= ordemAtual.Length) return;

        if (primeiroSelecionado == -1)
        {
            primeiroSelecionado = index;

            if (textoEstado != null)
                textoEstado.text = "Escolhe outro livro para trocar.";
        }
        else
        {
            if (primeiroSelecionado == index)
            {
                primeiroSelecionado = -1;

                if (textoEstado != null)
                    textoEstado.text = "Seleção cancelada.";

                return;
            }

            TrocarLivros(primeiroSelecionado, index);
            primeiroSelecionado = -1;

            AtualizarUI();
            VerificarResposta();
        }
    }

    private void TrocarLivros(int a, int b)
    {
        string temp = ordemAtual[a];
        ordemAtual[a] = ordemAtual[b];
        ordemAtual[b] = temp;
    }

    private void VerificarResposta()
    {
        if (ordemAtual.Length != ordemCorreta.Length)
        {
            Debug.LogWarning("A ordem inicial e a ordem correta têm tamanhos diferentes.");
            return;
        }

        for (int i = 0; i < ordemCorreta.Length; i++)
        {
            if (ordemAtual[i] != ordemCorreta[i])
            {
                if (textoEstado != null)
                    textoEstado.text = "Ainda não está certo...";

                return;
            }
        }

        ResolverPuzzle();
    }

    private void ResolverPuzzle()
    {
        resolvido = true;

        PlayerPrefs.SetInt(puzzleID, 1);
        PlayerPrefs.Save();

        if (adicionarRecompensaAoInventario && inventory != null)
        {
            if (!inventory.HasItem(itemRecompensa))
            {
                inventory.AddItem(itemRecompensa);
            }
        }

        if (SaveGameManager.Instance != null)
        {
            SaveGameManager.Instance.GuardarJogo();
        }

        if (objetoParaAtivar != null)
            objetoParaAtivar.SetActive(true);

        if (objetoParaDesativar != null)
            objetoParaDesativar.SetActive(false);

        MostrarResolvido();

        StartCoroutine(FecharDepois());
    }

    private void MostrarResolvido()
    {
        if (textoEstado != null)
            textoEstado.text = "A estante faz um clique... encontraste uma pista.";

        if (painelResolvido != null)
            painelResolvido.SetActive(true);

        if (botoesLivros != null)
        {
            foreach (Button botao in botoesLivros)
            {
                if (botao != null)
                    botao.interactable = false;
            }
        }
    }

    private IEnumerator FecharDepois()
    {
        yield return new WaitForSeconds(1.5f);

        if (interacao != null)
            interacao.FecharPuzzle();
    }

    private void AtualizarUI()
    {
        if (textosLivros == null || ordemAtual == null) return;

        for (int i = 0; i < textosLivros.Length && i < ordemAtual.Length; i++)
        {
            if (textosLivros[i] != null)
                textosLivros[i].text = ordemAtual[i];
        }

        if (botoesLivros != null)
        {
            foreach (Button botao in botoesLivros)
            {
                if (botao != null)
                    botao.interactable = !resolvido;
            }
        }
    }
}