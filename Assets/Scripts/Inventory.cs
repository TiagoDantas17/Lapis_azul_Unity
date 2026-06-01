using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> items = new List<string>();

    public void AddItem(string itemName)
    {
        items.Add(itemName);

        Debug.Log(itemName + " adicionada ao inventário");
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }
}
