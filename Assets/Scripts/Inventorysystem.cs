using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;


public class Inventorysystem : MonoBehaviour

{
    public Dictionary<GameObject, GameObject> Inventory= new Dictionary<GameObject, GameObject>();
    List<GameObject> counters= new List<GameObject>();
    List<GameObject> Inventoryspots = new List<GameObject>();
    GameObject Parent;
    GameObject InventorySpot;
    GameObject ItemHolding;
    GameObject EmptySpot;
    GameObject Counter;
    public GameObject LastObjectClicked;
    bool HoldingItem;
    int Xposition;
    int Yposition;
    // Start is called before the first frame update
    void Start()
    {
        EmptySpot = GameObject.Find("Empty");
        Parent = GameObject.Find("Inventory");
        InventorySpot = Resources.Load<GameObject>("Prefab - Inventory/Spot");
        Counter = Resources.Load<GameObject>("Prefab - Inventory/AmountCounter");
        LastObjectClicked = EmptySpot;
        Debug.Log(InventorySpot);
        Xposition = 0;
        Yposition = 0;
        for (int i = 1; i <= 7; i++)
        {
            Xposition += 90;
            Yposition = 325;
            for (int j = 1; j <= 3; j++)
            {
                Yposition -= 60; 
                var Name = i.ToString() + j.ToString();
                var clone = Instantiate(InventorySpot, new Vector3(Xposition,Yposition,0), Quaternion.identity, Parent.transform);
                clone.name = Name;
                //counter.GetComponent<TextMeshPro>() = EmptySpot.GetComponent<int>.amount();
                counters.Add(clone);
                Inventoryspots.Add(clone);
                Inventory.Add(clone, EmptySpot);
                Debug.Log(Name);
                Debug.Log(Inventory);
            }
            

        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print(LastObjectClicked);
            print("mouseclicked");
            if (Inventory.ContainsKey(LastObjectClicked))
            {
                print("here");
                if (Inventory[LastObjectClicked] == EmptySpot && HoldingItem == true)
                {
                    Inventory[LastObjectClicked] = ItemHolding;
                    ItemHolding = EmptySpot;
                    HoldingItem = false;
                    LastObjectClicked.GetComponent<Toggle>().isOn = true;
                    LastObjectClicked.gameObject.transform.GetComponent<Image>().sprite = Inventory[LastObjectClicked].GetComponent<Block>().MenuIcon;

                }
                if (Inventory[LastObjectClicked] != EmptySpot && HoldingItem == true)
                {
                    var previousitem = Inventory[LastObjectClicked];
                    var previouslyholding = ItemHolding;
                    ItemHolding = previousitem;
                    Inventory[LastObjectClicked] = previouslyholding;
                    LastObjectClicked.gameObject.transform.GetComponent<Image>().sprite = Inventory[LastObjectClicked].GetComponent<Block>().MenuIcon;
                }
                if (Inventory[LastObjectClicked] != EmptySpot && HoldingItem == false)
                {
                    ItemHolding = Inventory[LastObjectClicked];
                    Inventory[LastObjectClicked] = EmptySpot;
                    HoldingItem = !HoldingItem;
                    LastObjectClicked.GetComponent<Toggle>().isOn = false;
                    LastObjectClicked.gameObject.transform.GetComponent<Image>().sprite = Inventory[LastObjectClicked].GetComponent<Block>().MenuIcon;
                }
            }
        }


        foreach (GameObject x in Inventoryspots) 
        {
            var indhold = x.transform.Find("AmountCounter").GetComponent<TMP_Text>();
            
            if (indhold.text == "0") 
            {
                indhold.gameObject.SetActive(false);
            }
            else
            {
                indhold.gameObject.SetActive(true);
            }
        }
    }
}
