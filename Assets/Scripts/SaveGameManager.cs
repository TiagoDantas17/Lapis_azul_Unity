using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance;

    [Header("Refer�ncias")]
    public Transform player;
    public Inventory inventory;

    [Header("Autosave")]
    public bool autosaveAtivo = true;
    public float tempoEntreAutosaves = 300f; // 300 segundos = 5 minutos

    private float timerAutosave;

    private List<string> collectedItemIDs = new List<string>();

    private void Awake()
    {
        Instance = this;

        int deveCarregar = PlayerPrefs.GetInt("LoadGame", 0);

        if (deveCarregar == 1)
        {
            CarregarJogo();

            PlayerPrefs.SetInt("LoadGame", 0);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        if (!autosaveAtivo) return;

        timerAutosave += Time.deltaTime;

        if (timerAutosave >= tempoEntreAutosaves)
        {
            GuardarJogo();
            timerAutosave = 0f;
        }
    }

    public void GuardarJogo()
    {
        if (player == null)
        {
            Debug.LogWarning("Falta ligar o Player no SaveGameManager.");
            return;
        }

        if (inventory == null)
        {
            Debug.LogWarning("Falta ligar o Inventory no SaveGameManager.");
            return;
        }

        SaveData data = new SaveData();

        data.playerX = player.position.x;
        data.playerY = player.position.y;
        data.playerZ = player.position.z;

        data.inventoryItems = new List<string>(inventory.items);
        data.collectedItemIDs = new List<string>(collectedItemIDs);

        SaveSystem.SaveGame(data);
    }

    public void CarregarJogo()
    {
        SaveData data = SaveSystem.LoadGame();

        if (data == null)
        {
            return;
        }

        if (player != null)
        {
            Vector3 novaPosicao = new Vector3(data.playerX, data.playerY, data.playerZ);

            Rigidbody rb = player.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.position = novaPosicao;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                player.position = novaPosicao;
            }
        }

        if (inventory != null)
        {
            inventory.items.Clear();
            inventory.items.AddRange(data.inventoryItems);
        }

        collectedItemIDs = new List<string>(data.collectedItemIDs);
    }

    public void RegistarObjetoApanhado(string itemID)
    {
        if (!collectedItemIDs.Contains(itemID))
        {
            collectedItemIDs.Add(itemID);
        }

        GuardarJogo();
    }

    public bool ObjetoJaApanhado(string itemID)
    {
        return collectedItemIDs.Contains(itemID);
    }

    public void ApagarSave()
    {
        SaveSystem.DeleteSave();

        collectedItemIDs.Clear();
    }
}