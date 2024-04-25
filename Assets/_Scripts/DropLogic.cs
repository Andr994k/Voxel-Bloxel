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

    public string destroyedby;

    [HideInInspector] public string droppedby;


    private void Awake()
    {
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

        playerObject = GameObject.FindGameObjectWithTag("Player");

        playerCollider = playerObject.GetComponent<Collider>();

        Character character = playerObject.GetComponent<Character>();
        if (destroyedby == "character")
        {
            BlockType block = character.currentDestroyedBlock;
            if (block == BlockType.Stone)
            {
                stoneChild.SetActive(true);
                activeCollider = stoneChild.GetComponent<Collider>();
                activeBlock = BlockType.Stone;
                itemtype = Resources.Load<Item>("Items/Stone");
            }
            if (block == BlockType.Sand)
            {
                sandChild.SetActive(true);
                activeCollider = sandChild.GetComponent<Collider>();
                activeBlock = BlockType.Stone;
                itemtype = Resources.Load<Item>("Items/Sand");
            }
            if (block == BlockType.Dirt)
            {
                dirtChild.SetActive(true);
                activeCollider = dirtChild.GetComponent<Collider>();
                activeBlock = BlockType.Dirt;
                itemtype = Resources.Load<Item>("Items/Dirt");
            }
            if (block == BlockType.Grass_Dirt)
            {
                grassChild.SetActive(true);
                activeCollider = grassChild.GetComponent<Collider>();
                activeBlock = BlockType.Grass_Dirt;
                itemtype = Resources.Load<Item>("Items/Grass");
            }
        }
        if (destroyedby == "Inv")
        {
            BlockType block = itemtype.type;
            if (block == BlockType.Stone)
            {
                stoneChild.SetActive(true);
                activeCollider = stoneChild.GetComponent<Collider>();
                activeBlock = BlockType.Stone;
                itemtype = Resources.Load<Item>("Items/Stone");
            }
            if (block == BlockType.Sand)
            {
                sandChild.SetActive(true);
                activeCollider = sandChild.GetComponent<Collider>();
                activeBlock = BlockType.Stone;
                itemtype = Resources.Load<Item>("Items/Sand");
            }
            if (block == BlockType.Dirt)
            {
                dirtChild.SetActive(true);
                activeCollider = dirtChild.GetComponent<Collider>();
                activeBlock = BlockType.Dirt;
                itemtype = Resources.Load<Item>("Items/Dirt");
            }
            if (block == BlockType.Grass_Dirt)
            {
                grassChild.SetActive(true);
                activeCollider = grassChild.GetComponent<Collider>();
                activeBlock = BlockType.Grass_Dirt;
                itemtype = Resources.Load<Item>("Items/Grass");
            }
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
