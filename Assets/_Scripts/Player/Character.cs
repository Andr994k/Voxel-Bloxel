using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
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
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
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




            if (Input.GetMouseButton(0) && counter < 3 && hit.point == hitCheck.point)
            {
                yield return new WaitForSeconds(0.33f);
                counter++;
                if (counter == 1)
                {
                    Instantiate(t1, hit.point, Quaternion.identity);
                }
                if (counter == 2)
                {
                    Instantiate(t2, hit.point, Quaternion.identity);
                }
                if (counter == 3)
                {
                    ModifyTerrain(hit, BlockType.Air);
                    notDestroyed = false;
                    StopAllCoroutines();
                    animator.SetBool("Mining", false);
                    Instantiate(dropBlockPrefab, hit.point, Quaternion.identity);
                }
            }
            else
            {
                if (Input.GetMouseButton(0) == false)
                {
                    animator.SetBool("Mining", false);
                }
                notDestroyed = false;
            }

        }
        
        
    }
}
