using UnityEngine;
using Prime31; //CharacterController2D
using Prime31.ZestKit; //ZestKit

[RequireComponent(typeof (CharacterController2D))]
public class PlayerController : BouncyCharacter 
{
    //movement
    public float moveSpeed = 8.0f;
    public float airMoveSpeed = 8.0f;
    private float deltaX = 0.0f; // holds the change in motion so that we handle no keys pushed && both keys pushed well
    public float gravity = 15.0f;
    public float targetJumpHeight = 2.0f;
    //jumping
    public bool canDoubleJump = false;
    protected bool doubleJumpUsed;
    protected bool jumpReleased;
    protected bool jumpHeldLastFrame;
    protected bool holdJump;
    public bool canHoldJump;
    //refs
    protected CharacterController2D controller;
    protected SpriteRenderer spriteRenderer;
    protected Vector2 velocity;
    protected Transform _spriteTransform;

    public override void Awake()
    {
        base.Awake();
        controller = GetComponent<CharacterController2D>();
        doubleJumpUsed = false;
        //get sprite/animator data from child
        _animator = GetComponentInChildren<Animator>();
        _spriteTransform = _animator.gameObject.GetComponent<Transform>();
        spriteRenderer = _animator.gameObject.GetComponent<SpriteRenderer>();
        startingScale = _spriteTransform.localScale;
    }

    void Update()
    {
        if (!paused)
        {
            holdJump = false;
            deltaX = 0f;
            if (controller.isGrounded)
            { 
                velocity.y = 0.0f;
                doubleJumpUsed = false;
                jumpHeldLastFrame = false;
                if(controller.becameGroundedThisFrame)
                {
                    // Debug.Log("Was not grounded last frame");
                    //bounce
                    _spriteTransform.localScale = new Vector3(startingScale.x * 1.15f, startingScale.y * .85f, startingScale.z);
                    _spriteTransform.ZKlocalScaleTo(startingScale, 0.2f).setCompletionHandler(ResetAnchor).start();

                }
            }
            // else
            // {
            //     Debug.Log("Not Grounded");
            // }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if(controller.isGrounded) deltaX -= moveSpeed;
                else deltaX -= airMoveSpeed;
                spriteRenderer.flipX = true;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if(controller.isGrounded) deltaX += moveSpeed;
                else deltaX += airMoveSpeed;
                spriteRenderer.flipX = false;
            }
            velocity.x = deltaX;


            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                if (controller.isGrounded)
                {
                    velocity.y = Mathf.Sqrt(2f * targetJumpHeight * gravity);
                    _spriteTransform.localScale = new Vector3(startingScale.x * 0.75f, startingScale.y * 1.25f, startingScale.z);
                    _spriteTransform.ZKlocalScaleTo(startingScale, 0.3f).start();
                    jumpReleased = false;
                }
                else if (canDoubleJump && !doubleJumpUsed)
                {
                    doubleJumpUsed = true;
                    velocity.y = Mathf.Sqrt(1.5f * targetJumpHeight * gravity);
                    _spriteTransform.localScale = new Vector3(startingScale.x * 0.75f, startingScale.y * 1.25f, startingScale.z);
                    _spriteTransform.ZKlocalScaleTo(startingScale, 0.3f).start();
                }
            }
            else if(canHoldJump && !controller.isGrounded && !jumpReleased && !jumpHeldLastFrame && Input.GetKey(KeyCode.Space) && (velocity.y >= -0.05 && velocity.y <= 0.05))
            {
                Debug.Log("Holding jump!");
                holdJump = true;
                jumpHeldLastFrame = true;  
            }
            if(Input.GetKeyUp(KeyCode.Space) && !jumpReleased && velocity.y > 0)
            {
                Debug.Log("Short hop!");
                jumpReleased = true;
                //cut vertical speed
                velocity.y *= 0.5f;
            }

            if (Mathf.Abs(velocity.x) > 0 && controller.isGrounded)
            {
                _animator.SetBool("walking", true);
            }
            else
            {
                _animator.SetBool("walking", false);
            }

            velocity.y -= gravity * Time.deltaTime;
            if(holdJump) velocity.y = 0;

            controller.move(velocity * Time.deltaTime);
            //set new local velocity
            velocity = controller.velocity;
        }
    }

    void ResetAnchor(ITween<Vector3> garbage)
    {
    }
}