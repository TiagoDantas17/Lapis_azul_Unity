using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Item")]
    public string itemID = "Key_Cela_01";
    public string itemName = "Key";

    [Header("UI")]
    public GameObject pickupText;

    private bool playerInside = false;
    private Inventory inventory;

    private void Start()
    {
        if (pickupText != null)
        {
            pickupText.SetActive(false);
        }

        if (SaveGameManager.Instance != null && SaveGameManager.Instance.ObjetoJaApanhado(itemID))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory != null)
            {
                inventory.AddItem(itemName);
            }

            if (SaveGameManager.Instance != null)
            {
                SaveGameManager.Instance.RegistarObjetoApanhado(itemID);
            }

            if (pickupText != null)
            {
                pickupText.SetActive(false);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            inventory = other.GetComponent<Inventory>();

            if (pickupText != null)
            {
                pickupText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (pickupText != null)
            {
                pickupText.SetActive(false);
            }
        }
    }
}