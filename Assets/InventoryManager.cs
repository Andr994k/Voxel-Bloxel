using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventoryslot[] inventoryslots ;
    public GameObject Inventoryitemprefab;
    public GameObject Inv;
    public GameObject InvBackground;

    int selectedslot = -1;
    public bool inventoryactive = false;



    void Changeselectedslot(int newvalue)
    {
        if (selectedslot >= 0) 
        {
            inventoryslots[selectedslot].Deselect(); 
        }

        inventoryslots[newvalue].Select();
        selectedslot = newvalue;
    }

    public void Start()
    {
        Changeselectedslot(0);
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 7)
            {
                Changeselectedslot (number-1);
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Inv.activeSelf == true)
            {
                Inv.SetActive(false);
                InvBackground.SetActive(false);

            }
            else
            {
                Inv.SetActive(true);
                InvBackground.SetActive(true);
            }

        }

    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventoryslots.Length; i++)
        {
            Inventoryslot slot = inventoryslots[i];
            InventoryItem iteminslot = slot.GetComponentInChildren<InventoryItem>();
            if (iteminslot != null && iteminslot.item == item && iteminslot.CurrentCount < iteminslot.maxcount && iteminslot.item.stackable == true)
            {
                iteminslot.CurrentCount++;
                iteminslot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventoryslots.Length ; i++)
        {
            Inventoryslot slot = inventoryslots[i];
            InventoryItem iteminslot = slot.GetComponentInChildren<InventoryItem>();
            if (iteminslot == null)
            {
                SpawnItem(item, slot);
                slot.GetComponentInChildren<InventoryItem>().CurrentCount = 1;
                return true;
            }
        }

        
        return false;
    }
    public void RemoveItem(Item item) 
    { 
    }

    void SpawnItem(Item item, Inventoryslot slot)
    {
        GameObject newItemGo = Instantiate(Inventoryitemprefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
    public Item GetSelectedItem(bool use)
    {
        Inventoryslot slot = inventoryslots[selectedslot];
        InventoryItem iteminslot = slot.GetComponent<InventoryItem>();
        if (iteminslot != null)
        {
            Item item = iteminslot.item;
            if (use==true)
            {
                iteminslot.CurrentCount--;
                if (iteminslot.CurrentCount <=0)
                {
                    Destroy(iteminslot.gameObject);
                }
                else
                {
                    iteminslot.RefreshCount();
                }
            }
            return item;
        }
        else
        {
            return null;
        }
    }
}
