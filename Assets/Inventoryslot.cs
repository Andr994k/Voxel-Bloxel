using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventoryslot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColer;

    private void Awake()
    {
        Deselect();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        { 
        GameObject dropped = eventData.pointerDrag;
        InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();
        draggableItem.parentAfterDrag = transform;
        }
    }

    public void Select()
    {
        image.color = selectedColor;
    }

    public void Deselect()
    {
            image.color = notSelectedColer;
    }

}    
