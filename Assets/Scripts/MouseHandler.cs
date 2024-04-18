using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Hentet fra https://discussions.unity.com/t/detect-mouseover-click-for-ui-canvas-object/152611/3

public class Example : MonoBehaviour
    , IPointerClickHandler // 2
    , IDragHandler
    , IPointerEnterHandler
    , IPointerExitHandler
// ... And many more available!
{

    public GameObject Lastclicked;
    void Awake()
    {
    }

    void Update()
    {
    
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        print("I was clicked");
        GameObject.Find("Inventory").GetComponent<Inventorysystem>().LastObjectClicked = gameObject;
        print(GameObject.Find("Inventory").GetComponent<Inventorysystem>().LastObjectClicked);
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("I'm being dragged!");
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
