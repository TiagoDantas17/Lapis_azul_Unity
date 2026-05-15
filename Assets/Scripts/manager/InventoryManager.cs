using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public List<string> itens = new List<string>();

    void Awake()
    {
        instance = this;
    }

    public void AdicionarItem(string item)
    {
        itens.Add(item);

        Debug.Log("Item adicionado: " + item);
    }

    public bool TemItem(string item)
    {
        return itens.Contains(item);
    }

    public void RemoverItem(string item)
    {
        itens.Remove(item);
    }
}