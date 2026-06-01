using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public TMP_Text inventoryText;

    private void Update()
    {
        inventoryText.text = "Inventário:\n";

        foreach (string item in inventory.items)
        {
            inventoryText.text += "- " + item + "\n";
        }
    }
}