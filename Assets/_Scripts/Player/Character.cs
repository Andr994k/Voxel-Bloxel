using System;
using System.Collections;
using System.Collections.Generic;
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

    //
    public Vector3Int blockCoords;


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
    }
    private void Start()
    {
        currenthealth = 100;
        playerInput.OnLeftMouseClick += HandleLeftMouseClick;
        playerInput.OnRightMouseClick += HandleRightMouseClick;
    }


    void Update()
    {
        if (isInEditorMode && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(EditorPlace());
        }
        if (isInEditorMode && firstPointPlaced && secondPointPlaced && Input.GetKeyDown(KeyCode.C))
        {
            Vector3Int blockDifference = pos1 - pos2;
            Debug.Log(blockDifference);
            Math.Abs(blockDifference.y);
            Math.Abs(blockDifference.z);
            firstPointPlaced = false;
            secondPointPlaced = false;
            
            for (int a = 0; a >= Math.Abs(blockDifference.x); a++)
            {
                Debug.Log("First Layer");
                if (blockDifference.x < 0)
                {
                    blockCoords.x = blockDifference.x - a;
                }
                if (blockDifference.x >= 0)
                {
                    blockCoords.x = blockDifference.x + a;
                }
                for (int b = 0; b >= Math.Abs(blockDifference.y); b++)
                {
                    Debug.Log("Second Layer");
                    if (blockDifference.y < 0)
                    {
                        blockCoords.y = blockDifference.y - b;
                    }
                    if (blockDifference.y >= 0)
                    {
                        blockCoords.y = blockDifference.y + b;
                    }
                    for (int c = 0; c >= Math.Abs(blockDifference.z); c++)
                    {
                        Debug.Log("Third Layer");
                        if (blockDifference.z < 0)
                        {
                            blockCoords.z = blockDifference.z - c;
                        }
                        if (blockDifference.z >= 0)
                        {
                            blockCoords.z = blockDifference.z + c;
                        }
                        Ray chunkRay = new Ray(blockCoords, Vector3Int.up);
                        RaycastHit chunkhit;
                        Physics.Raycast(chunkRay, out chunkhit, Mathf.Infinity, groundMask);
                        ModifyTerrain(chunkhit, activeBlock);
                    }
                }
            }


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
            activeBlock = BlockType.TreeTrunk;
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
            activeBlock = BlockType.Dirt;
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
    IEnumerator EditorPlace()
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
        yield return null;
        
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

                if (Input.GetMouseButton(0) == false)
                {
                    animator.SetBool("Mining", false);
                }
                notDestroyed = false;

                madeT1 = false;
                madeT2 = false;
                madeT3 = false;
            }

        }
        
        
    }
}
