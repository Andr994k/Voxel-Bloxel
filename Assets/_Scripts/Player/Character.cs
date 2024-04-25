using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using Image = UnityEngine.UI.Image;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private PlayerMovement playerMovement;

    //Destroy and place length
    public float interactionRayLength = 5;

    //Place length for editor mode
    public float editorInteractionRayLength = 20;

    //The interactable ground
    public LayerMask groundMask;

    //Currently holding block
    public BlockType activeBlock = BlockType.Dirt;

    //The blocktype that the player has just destroyed
    public BlockType currentDestroyedBlock = BlockType.Stone;

    //The prefab representing a block after being destroyed
    public GameObject dropBlockPrefab;

    //the images representing the different stages of a block being destroyed
    public GameObject t1;
    public GameObject t2;
    public GameObject t3;

    bool madeT1 = false;
    bool madeT2 = false;
    bool madeT3 = false;

    //the most inefficient implementation possible, but im lazy and dont have time
    //Different gameobjects for all the sides of a block
    GameObject xt1destroysprite;
    GameObject yt1destroysprite;
    GameObject zt1destroysprite;
    GameObject minusxt1destroysprite;
    GameObject minusyt1destroysprite;
    GameObject minuszt1destroysprite;

    GameObject xt2destroysprite;
    GameObject yt2destroysprite;
    GameObject zt2destroysprite;
    GameObject minusxt2destroysprite;
    GameObject minusyt2destroysprite;
    GameObject minuszt2destroysprite;

    GameObject xt3destroysprite;
    GameObject yt3destroysprite;
    GameObject zt3destroysprite;
    GameObject minusxt3destroysprite;
    GameObject minusyt3destroysprite;
    GameObject minuszt3destroysprite;

    //Bool for determining when player is using edit mode
    public bool isInEditorMode = true;
    public Vector3Int pos1;
    public Vector3Int pos2;
    public bool firstPointPlaced = false;
    public bool secondPointPlaced = false;
    public bool playerHasConfirmed = false;

    //List of all cardinal directions in unity, needed for editor mode checks
    public List<Vector3Int> directionList = new List<Vector3Int> {Vector3Int.up, Vector3Int.down, Vector3Int.right, Vector3Int.left, Vector3Int.forward, Vector3Int.back};

    //List of all chunks that have been modified by the player, so that they can be saved
    [HideInInspector]public List<ChunkData> modifiedChunks;


    //Needed for foreach loop making a list of chunks that have been modified
    ChunkData chunkData;

    public Animator animator;

    bool isWaiting = false;

    public World world;

    public Image Healthbar;

    private float currenthealth;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        world = FindObjectOfType<World>();
        modifiedChunks = new List<ChunkData>();
    }
    private void Start()
    {
        currenthealth = 100;
        playerInput.OnLeftMouseClick += HandleLeftMouseClick;
        playerInput.OnRightMouseClick += HandleRightMouseClick;
    }


    (bool, RaycastHit) lookForChunk(Vector3Int coords, Vector3Int direction)
    {
        Ray chunkRay = new Ray(coords, direction);
        RaycastHit chunkhit;
        return (Physics.Raycast(chunkRay, out chunkhit, Mathf.Infinity, groundMask), chunkhit);
    }

    void Update()
    {
        if (isInEditorMode && Input.GetMouseButtonDown(0))
        {
            EditorPlace();
        }
        if (isInEditorMode && firstPointPlaced && secondPointPlaced && Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Starting...");
            List<Vector3Int> blockCoordList = FindCoordinates(pos1, pos2);

            foreach (Vector3Int coordinate in blockCoordList)
            {
                Debug.Log("(Foreach)Trying coordinate:" + coordinate);
                Debug.Log(directionList.Count);

                for (int i = 0; i < directionList.Count; i++)
                {
                    Debug.Log("(For)Trying direction:" + directionList[i]);

                    (bool hitSomething, RaycastHit hit) = lookForChunk(coordinate, directionList[i]);

                    if (hitSomething)
                    {
                        Debug.Log("Made block at" +  coordinate);
                        world.SetBlockEditor(hit, coordinate, activeBlock);
                        break;
                    }
                }
                
            }
            firstPointPlaced = false;
            secondPointPlaced = false;
        }
        
        //Healthbar.fillAmount = currenthealth / 100f;

        //animator.SetBool("isGrounded", playerMovement.IsGrounded);
        if (playerMovement.IsGrounded && playerInput.IsJumping && isWaiting == false)
        {
            //animator.SetTrigger("jump");
            isWaiting = true;
            //StopAllCoroutines();
            StartCoroutine(ResetWaiting());
        }
        //animator.SetFloat("speed", playerInput.MovementInput.magnitude);
        playerMovement.HandleGravity(playerInput.IsJumping);
        playerMovement.Walk(playerInput.MovementInput, playerInput.RunningPressed);


        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            activeBlock = BlockType.Nothing;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeBlock = BlockType.Stone;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeBlock = BlockType.Sand;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeBlock = BlockType.Dirt;            
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            activeBlock = BlockType.Grass_Dirt;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            activeBlock = BlockType.TreeLeavesSolid;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            activeBlock = BlockType.TreeTrunk;
        }


        if (Input.GetKeyDown(KeyCode.O))
        {
            makeModifiedChunksList();
            SaveSystem.SaveWorld(modifiedChunks);
        }


        if (activeBlock == BlockType.Nothing)
        {
            animator.SetBool("HoldingBlock", false);
        }
        else
        {
            animator.SetBool("HoldingBlock", true);
        }


    }

    List<Vector3Int> FindCoordinates(Vector3Int start, Vector3Int end)
    {
        List<Vector3Int> coordinates = new List<Vector3Int>();

        //Need to find the larger coordinate of the two, so that they can be used in the for-loop structure, as it assumes the first variable is the smallest
        int minX = Mathf.Min(start.x, end.x);
        int maxX = Mathf.Max(start.x, end.x);
        int minY = Mathf.Min(start.y, end.y);
        int maxY = Mathf.Max(start.y, end.y);
        int minZ = Mathf.Min(start.z, end.z);
        int maxZ = Mathf.Max(start.z, end.z);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                for (int z = minZ; z <= maxZ; z++)
                {
                    Vector3Int coordinate = new Vector3Int(x, y, z);
                    coordinates.Add(coordinate);
                }
            }
        }

        return coordinates;
    }
    void makeModifiedChunksList()
    {
        foreach (var pair in world.worldData.chunkDataDictionary)
        {
            chunkData = pair.Value;

            if (chunkData.modifiedByThePlayer)
            {
                modifiedChunks.Add(chunkData);
            }
        }
    }

    IEnumerator ResetWaiting()
    {
        yield return new WaitForSeconds(0.1f);
        //animator.ResetTrigger("jump");
        isWaiting = false;
    }


    private void HandleLeftMouseClick()
    {
        if (isInEditorMode)
        {
            //Dont do anything here if player is in editormode, functionality is being done elsewhere
        }
        else
        {
            Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(playerRay, out hit, interactionRayLength, groundMask))
            {
                animator.SetBool("Mining", true);
                StartCoroutine(DestroyBlock(hit));
            }
        }
    }
    private void HandleRightMouseClick()
    {
        if (isInEditorMode)
        {
            //Dont do anything here if player is in editormode, functionality is being done elsewhere
        }
        else
        {
            Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(playerRay, out hit, interactionRayLength, groundMask))
            {
                ModifyTerrain(hit, activeBlock);
            }
        }
    }

    private void ModifyTerrain(RaycastHit hit, BlockType blockType)
    {
        world.SetBlock(hit, blockType);
    }
    void EditorPlace()
    {
        Ray editorRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit editorHit;
        if (Physics.Raycast(editorRay, out editorHit, editorInteractionRayLength, groundMask) && firstPointPlaced == false)
        {
            pos1 = new Vector3Int((int)editorHit.point.x, (int)editorHit.point.y, (int)editorHit.point.z);
            firstPointPlaced = true;
            Debug.Log(pos1);
        }

        else if (Physics.Raycast(editorRay, out editorHit, editorInteractionRayLength, groundMask) && secondPointPlaced == false)
        {
            pos2 = new Vector3Int((int)editorHit.point.x, (int)editorHit.point.y, (int)editorHit.point.z);
            secondPointPlaced = true;
            Debug.Log(pos2);
        }
        if (firstPointPlaced && secondPointPlaced)
        {
            //Show some ui depicting a line between the two points idk
            
        }
        
    }
    IEnumerator DestroyBlock(RaycastHit hit)
    {
        bool notDestroyed = true;
        int counter = 0;
        while (notDestroyed)
        {
            //check if the player is still pointing at the same block
            Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hitCheck;
            Physics.Raycast(playerRay, out hitCheck, interactionRayLength, groundMask);
            hitCheck.point = world.GetBlockPos(hitCheck);

            hit.point = world.GetBlockPos(hit);



            Vector3Int blockPos = world.GetBlockPos(hit);
            

            Vector3 xnewpos = new Vector3(blockPos.x+ 0.51f, blockPos.y, blockPos.z);
            Vector3 ynewpos = new Vector3(blockPos.x, blockPos.y+ 0.51f, blockPos.z);
            Vector3 znewpos = new Vector3(blockPos.x, blockPos.y, blockPos.z+0.51f);
            Vector3 minusxnewpos = new Vector3(blockPos.x - 0.51f, blockPos.y, blockPos.z);
            Vector3 minusynewpos = new Vector3(blockPos.x, blockPos.y - 0.51f, blockPos.z);
            Vector3 minusznewpos = new Vector3(blockPos.x, blockPos.y, blockPos.z - 0.51f);

            Quaternion xrotation = new Quaternion(Quaternion.identity.x, Quaternion.identity.y + 90, Quaternion.identity.z, Quaternion.identity.w+ 90);
            Quaternion yrotation = new Quaternion(Quaternion.identity.x+ 90, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w + 90);



            if (Input.GetMouseButton(0) && counter < 3 && hit.point == hitCheck.point)
            {
                yield return new WaitForSeconds(0.33f);
                counter++;

                if (counter == 1)
                {
                    if (!madeT1)
                    {
                        xt1destroysprite = Instantiate(t1, xnewpos, xrotation);
                        yt1destroysprite = Instantiate(t1, ynewpos, yrotation);
                        zt1destroysprite = Instantiate(t1, znewpos, Quaternion.identity);
                        minusxt1destroysprite = Instantiate(t1, minusxnewpos, xrotation);
                        minusyt1destroysprite = Instantiate(t1, minusynewpos, yrotation);
                        minuszt1destroysprite = Instantiate(t1, minusznewpos, Quaternion.identity);

                        madeT1 = true;
                    }

                }
                if (counter == 2)
                {
                    if (!madeT2)
                    {
                        xt2destroysprite = Instantiate(t2, xnewpos, xrotation);
                        yt2destroysprite = Instantiate(t2, ynewpos, yrotation);
                        zt2destroysprite = Instantiate(t2, znewpos, Quaternion.identity);
                        minusxt2destroysprite = Instantiate(t2, minusxnewpos, xrotation);
                        minusyt2destroysprite = Instantiate(t2, minusynewpos, yrotation);
                        minuszt2destroysprite = Instantiate(t2, minusznewpos, Quaternion.identity);

                        madeT2 = true;
                    }

                }
                if (counter == 3)
                {

                    if (!madeT3)
                    {
                        xt3destroysprite = Instantiate(t3, xnewpos, xrotation);
                        yt3destroysprite = Instantiate(t3, ynewpos, yrotation);
                        zt3destroysprite = Instantiate(t3, znewpos, Quaternion.identity);
                        minusxt3destroysprite = Instantiate(t3, minusxnewpos, xrotation);
                        minusyt3destroysprite = Instantiate(t3, minusynewpos, yrotation);
                        minuszt3destroysprite = Instantiate(t3, minusznewpos, Quaternion.identity);

                        madeT3 = true;
                    }

                    //This is for finding which type of block the player is mining, in order to drop the correct block when destroyed
                    //Reference to the chunkrenderer associated with the chunk where the raycast hit
                    //This is needed for the chunkData associated with the renderer, which stores various data about the blocks present in the chunk
                    ChunkRenderer chunkRenderer = hit.collider.GetComponent<ChunkRenderer>();

                    //Getting the index of the block from its raycast
                    //For this we need to convert raycast coordinates into a Vector3Int, which we can use to get the local coordinates of the block, which is needed to find the index of the block, and therefore the blocktype corresponding with it
                    Vector3Int intPosition = new Vector3Int ((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
                    Vector3Int localPosition = Chunk.GetBlockInChunkCoordinates(chunkRenderer.chunkData, intPosition);
                    int index = Chunk.GetIndexFromPosition(chunkRenderer.chunkData, localPosition.x, localPosition.y, localPosition.z);


                    //Using said index to get the blocktype from the list of blocks
                    currentDestroyedBlock = chunkRenderer.chunkData.blocks[index];


                    ModifyTerrain(hit, BlockType.Air);
                    notDestroyed = false;
                    StopAllCoroutines();
                    animator.SetBool("Mining", false);
                    Instantiate(dropBlockPrefab, hit.point, Quaternion.identity);


                    Destroy(xt1destroysprite);
                    Destroy(yt1destroysprite);
                    Destroy(zt1destroysprite);
                    Destroy(minusxt1destroysprite);
                    Destroy(minusyt1destroysprite);
                    Destroy(minuszt1destroysprite);

                    Destroy(xt2destroysprite);
                    Destroy(yt2destroysprite);
                    Destroy(zt2destroysprite);
                    Destroy(minusxt2destroysprite);
                    Destroy(minusyt2destroysprite);
                    Destroy(minuszt2destroysprite);

                    Destroy(xt3destroysprite);
                    Destroy(yt3destroysprite);
                    Destroy(zt3destroysprite);
                    Destroy(minusxt3destroysprite);
                    Destroy(minusyt3destroysprite);
                    Destroy(minuszt3destroysprite);

                    madeT1 = false;
                    madeT2 = false;
                    madeT3 = false;
                }
            }
            else
            {
                Destroy(xt1destroysprite);
                Destroy(yt1destroysprite);
                Destroy(zt1destroysprite);
                Destroy(minusxt1destroysprite);
                Destroy(minusyt1destroysprite);
                Destroy(minuszt1destroysprite);

                Destroy(xt2destroysprite);
                Destroy(yt2destroysprite);
                Destroy(zt2destroysprite);
                Destroy(minusxt2destroysprite);
                Destroy(minusyt2destroysprite);
                Destroy(minuszt2destroysprite);

                Destroy(xt3destroysprite);
                Destroy(yt3destroysprite);
                Destroy(zt3destroysprite);
                Destroy(minusxt3destroysprite);
                Destroy(minusyt3destroysprite);
                Destroy(minuszt3destroysprite);


                animator.SetBool("Mining", false);
                notDestroyed = false;

                madeT1 = false;
                madeT2 = false;
                madeT3 = false;
            }

        }
        
        
    }
}
