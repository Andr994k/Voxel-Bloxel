using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMat", menuName = "Game/Mat")]
public class Mat : ScriptableObject
{
    public string Name;
    public float Health;
    public Sprite Image;
    public int amount;
}
