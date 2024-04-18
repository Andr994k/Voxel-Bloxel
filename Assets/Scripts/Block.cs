using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] Mat Material;
    public Sprite MenuIcon;
    public int Amount;
    // Start is called before the first frame update
    void Start()
    {
        MenuIcon = Material.Image;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
