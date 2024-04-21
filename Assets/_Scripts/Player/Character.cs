using System.Collections;
using System.Collections.Generic;
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

    //the most inefficient implementation possible, but im lazy
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



    public bool fly = false;

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
        playerInput.OnFly += HandleFlyClick;
    }

    private void HandleFlyClick()
    {
        fly = !fly;
    }

    void Update()
    {
        //Healthbar.fillAmount = currenthealth / 100f;
        if (fly)
        {
            //animator.SetFloat("speed", 0);
            //animator.SetBool("isGrounded", false);
            //animator.ResetTrigger("jump");
            playerMovement.Fly(playerInput.MovementInput, playerInput.IsJumping, playerInput.RunningPressed);

        }
        else
        {
            //animator.SetBool("isGrounded", playerMovement.IsGrounded);
            if (playerMovement.IsGrounded && playerInput.IsJumping && isWaiting == false)
            {
                //animator.SetTrigger("jump");
                isWaiting = true;
                StopAllCoroutines();
                StartCoroutine(ResetWaiting());
            }
            //animator.SetFloat("speed", playerInput.MovementInput.magnitude);
            playerMovement.HandleGravity(playerInput.IsJumping);
            playerMovement.Walk(playerInput.MovementInput, playerInput.RunningPressed);


        }
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
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, interactionRayLength, groundMask))
        {
            animator.SetBool("Mining", true);
            StartCoroutine(DestroyBlock(hit));
        }

    }
    private void HandleRightMouseClick()
    {
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, interactionRayLength, groundMask))
        {
            ModifyTerrain(hit, activeBlock);
        }

    }

    private void ModifyTerrain(RaycastHit hit, BlockType blockType)
    {
        world.SetBlock(hit, blockType);
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
