using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public string nomeItem;

    public void Interagir()
    {
        InventoryManager.instance.AdicionarItem(nomeItem);

        Destroy(gameObject);
    }

    public string GetNomeInteracao()
    {
        return "Apanhar " + nomeItem;
    }
}
