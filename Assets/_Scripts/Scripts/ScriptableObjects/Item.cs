using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item")]
public class Item : ScriptableObject
{
    public BlockType type;
    
    public bool stackable = true;

    public Sprite image;


}

