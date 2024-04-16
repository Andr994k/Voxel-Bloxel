using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private PlayerMovement playerMovement;

    public float interactionRayLength = 5;

    public LayerMask groundMask;

    private BlockType activeBlock = BlockType.Dirt;


    public bool fly = false;

    //public Animator animator;

    bool isWaiting = false;

    public World world;

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
            activeBlock = BlockType.Dirt;
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
            if (Input.GetMouseButton(0) && counter < 10)
            {
                Debug.Log("buttonpressed" + counter + "times");
                yield return new WaitForSeconds(0.1f);
                counter++;
                if (counter == 10)
                {
                    ModifyTerrain(hit, BlockType.Air);
                    notDestroyed = false;
                    StopAllCoroutines();
                }
            }
            else
            {
                notDestroyed = false;
            }

        }
        
        
    }
}
