using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id)
    {
       bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if (result == true)
            Debug.Log("added");
        if (result == false)
            Debug.Log("error");
    }
}
