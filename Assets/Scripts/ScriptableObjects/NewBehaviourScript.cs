using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item")]
public class Item : MonoBehaviour
{
    public ItemType type;
    public ActionType actionType;
    
    public bool stackable = true;

    public Sprite image;


}

public enum ItemType
{
    Block,
    Tool
}

public enum ActionType
{
    Dig,
    Mine
}