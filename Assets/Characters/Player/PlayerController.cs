using UnityEngine;
using Prime31; //CharacterController2D
using Prime31.ZestKit; //ZestKit

[RequireComponent(typeof (CharacterController2D))]
public class PlayerController : BouncyCharacter 
{
    public float moveSpeed = 8.0f;
    public float gravity = 15.0f;
    public float targetJumpHeight = 2.0f;

    public bool canDoubleJump = false;
    protected bool doubleJumpUsed;

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

            if (controller.isGrounded)
            {
                velocity.y = 0.0f;
                doubleJumpUsed = false;
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
                velocity.x = -moveSpeed;
                spriteRenderer.flipX = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                velocity.x = moveSpeed;
                spriteRenderer.flipX = false;
            }
            else
            {
                velocity.x = 0;
            }

            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                if (controller.isGrounded)
                {
                    velocity.y = Mathf.Sqrt(2f * targetJumpHeight * gravity);
                    _spriteTransform.localScale = new Vector3(startingScale.x * 0.75f, startingScale.y * 1.25f, startingScale.z);
                    _spriteTransform.ZKlocalScaleTo(startingScale, 0.3f).start();
                }
                else if (canDoubleJump && !doubleJumpUsed)
                {
                    doubleJumpUsed = true;
                    velocity.y = Mathf.Sqrt(1.5f * targetJumpHeight * gravity);
                    _spriteTransform.localScale = new Vector3(startingScale.x * 0.75f, startingScale.y * 1.25f, startingScale.z);
                    _spriteTransform.ZKlocalScaleTo(startingScale, 0.3f).start();
                }
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

            controller.move(velocity * Time.deltaTime);
            //set new local velocity
            velocity = controller.velocity;
        }
    }

    void ResetAnchor(ITween<Vector3> garbage)
    {
    }
}