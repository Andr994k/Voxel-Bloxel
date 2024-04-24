using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Recipe")]
public class Recipe : ScriptableObject
{
    public GameObject Product;
    public Dictionary<GameObject, int> objectandcountneeded = new Dictionary<GameObject, int>();
    public GameObject object1;
    public int RequiredAmount1;
    public GameObject object2;
    public int RequiredAmount2;
    public GameObject object3;
    public int RequiredAmount3;
    public List<GameObject> RequiredItems = new List<GameObject>();

    private void Start()
    {
        objectandcountneeded.Add(object1, RequiredAmount1);
        objectandcountneeded.Add(object2, RequiredAmount2);
        objectandcountneeded.Add(object3, RequiredAmount3);
        RequiredItems.Add(object1);
        RequiredItems.Add(object2);
        RequiredItems.Add(object3);
    }

}
