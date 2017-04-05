using UnityEngine;

public class PausableMonoBehavior : MonoBehaviour 
{
    public static bool paused = false; 

    protected Rigidbody2D _rigidbody;               //for pausing gravity and velocity
    private float originalGravityValue;
    private Vector2 originalVelocity;

    protected Animator _animator;                   //so animations do not continue playing while paused

    //TODO: Decide if I need this
    protected Transform _transform;                 //simply so everyobject owns a cached version of it's transform

    public virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
    }

    public virtual void Pause()
    {
        paused = true;
        if (_rigidbody != null)
        {
            originalGravityValue = _rigidbody.gravityScale;
            originalVelocity = _rigidbody.velocity;
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = Vector2.zero;
            //Debug.Log("Rigidbody Paused");
        }
        if(_animator != null)
        {
            _animator.enabled = false;
            //Debug.Log("Animator Paused");
        }
    }

    public virtual void UnPause()
    {
        paused = false;
        if (_rigidbody != null)
        {
            _rigidbody.gravityScale = originalGravityValue;
            _rigidbody.velocity = originalVelocity;
            //Debug.Log("Rigidbody UnPaused");
        }
        if (_animator != null)
        {
            _animator.enabled = true;
            //Debug.Log("Animator Unpaused");
        }

    }
}