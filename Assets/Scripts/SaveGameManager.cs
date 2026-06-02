using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public Inventory inventory;

    [Header("Debug")]
    public bool permitirGuardarComF5 = true;
    public bool permitirCarregarComF9 = true;

    private void Start()
    {
        int deveCarregar = PlayerPrefs.GetInt("LoadGame", 0);

        if (deveCarregar == 1)
        {
            CarregarJogo();
        }
    }

    private void Update()
    {
        if (permitirGuardarComF5 && Input.GetKeyDown(KeyCode.F5))
        {
            GuardarJogo();
        }

        if (permitirCarregarComF9 && Input.GetKeyDown(KeyCode.F9))
        {
            CarregarJogo();
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
    }

    public void ApagarSave()
    {
        SaveSystem.DeleteSave();
    }
}