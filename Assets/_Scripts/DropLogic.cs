using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DropLogic : MonoBehaviour
{
    public GameObject dirtChild;
    public GameObject grassChild;
    public GameObject sandChild;
    public GameObject stoneChild;
    public InventoryManager inventoryManager;

    public Collider activeCollider;

    public GameObject playerObject;

    public Collider playerCollider;

    public BlockType activeBlock = BlockType.Nothing;

    public Item itemtype;


    private void Awake()
    {
        inventoryManager = GameObject.Find("ÏnventoryManager").GetComponent<InventoryManager>();

        playerObject = GameObject.FindGameObjectWithTag("Player");

        playerCollider = playerObject.GetComponent<Collider>();

        Character character = playerObject.GetComponent<Character>();

        BlockType block = character.currentDestroyedBlock;

        if (block == BlockType.Stone)
        {
            stoneChild.SetActive(true);
            activeCollider = stoneChild.GetComponent<Collider>();
            activeBlock = BlockType.Stone;
            itemtype = Resources.Load<Item>("Items/BingChilling");
        }
        if (block == BlockType.Sand)
        {
            sandChild.SetActive(true);
            activeCollider = sandChild.GetComponent<Collider>();
            activeBlock = BlockType.Stone;
            itemtype = Resources.Load<Item>("Items/BingChilling");
        }
        if (block == BlockType.Dirt)
        {
            dirtChild.SetActive(true);
            activeCollider = dirtChild.GetComponent<Collider>();
            activeBlock = BlockType.Dirt;
            itemtype = Resources.Load<Item>("Items/BingChilling");
        }
        if (block == BlockType.Grass_Dirt)
        {
            grassChild.SetActive(true);
            activeCollider = grassChild.GetComponent<Collider>();
            activeBlock = BlockType.Grass_Dirt;
            itemtype = Resources.Load<Item>("Items/BingChilling");
        }
        if (block == BlockType.Dirt)
        {
            dirtChild.SetActive(true);
            activeCollider = dirtChild.GetComponent<Collider>();
        }
        if (block == BlockType.Sand)
        {
            sandChild.SetActive(true);
            activeCollider = sandChild.GetComponent<Collider>();
        }
        if (block == BlockType.Grass_Dirt)
        {
            grassChild.SetActive(true);
            activeCollider = grassChild.GetComponent<Collider>();
        }
    }
    private void Update()
    {
        if (playerCollider.bounds.Intersects(activeCollider.bounds))
        {

            inventoryManager.AddItem(itemtype);

            Destroy(gameObject);
        }
    }
}
