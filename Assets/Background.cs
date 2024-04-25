using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Background : MonoBehaviour, IDropHandler
{
    [SerializeField] GameObject dropBlockPrefab;   
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();
        Destroy(dropped);
        for (int i = 0; i == draggableItem.CurrentCount; i ++)
        {
            Instantiate(dropBlockPrefab, gameObject.transform.position, Quaternion.identity);
        }

    }
}
