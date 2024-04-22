using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Assertions.Must;
using Unity.PlasticSCM.Editor.WebApi;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public Image image;
    public TMP_Text text;
    public int maxcount = 64;
    
    [HideInInspector]public int CurrentCount = 1;


    [HideInInspector]public Item item;
    [HideInInspector] public Transform parentAfterDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        //transform.GetChild(0).GetComponent < TMP_Text >().raycastTarget = false;
    }

    public void InitializeItem(Item newitem)
    {
        item = newitem;
        image.sprite = newitem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        text.text = CurrentCount.ToString();
        bool textActive = CurrentCount > 1;
        text.gameObject.SetActive(textActive);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        //transform.GetChild(0).GetComponent<TMP_Text>().raycastTarget = true;
    }


   
    
}
    

