using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.AI.Navigation;
using UnityEngine;

public class LETHIMCOOK : MonoBehaviour
{
    public NavMeshSurface surface;
    public void CookingTime()
    {
        surface.BuildNavMesh();
    }

}
