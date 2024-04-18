using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon")]
public class ItemScriptableObject : ScriptableObject
{
    public string Name;
    public string Description;
}
