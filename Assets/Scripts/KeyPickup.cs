using UnityEngine;
using TMPro;

public class KeyPickup : MonoBehaviour
{
    public GameObject pickupText;

    private bool playerInside = false;

    private Inventory inventory;

    private void Start()
    {
        pickupText.SetActive(false);
    }


    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            inventory.AddItem("Key");

            Destroy(gameObject);

            pickupText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            inventory = other.GetComponent<Inventory>();

            pickupText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            pickupText.SetActive(false);
        }
    }
}