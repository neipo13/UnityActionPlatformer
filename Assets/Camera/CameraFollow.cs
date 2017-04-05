using UnityEngine;
using Prime31;
using Prime31.ZestKit;
/// <summary>
/// A simple and expandable 2D gameObject following camera based on Prime31's charactercontroller2D and Zestkit
/// </summary>
public class CameraFollow : PausableMonoBehavior
{
    #region attributes
    private TransformSpringTween cameraFollowTween; //zestkit overdamped spring to follow player, by overdamping we get a smoother follow effect
    private Camera cam;                             //cached Camera component of this object
    public CharacterController2D playerController;  //the CharacterController2D of the player, used to check if the player is grounded for platform snapping

    public Transform followTarget;              //target to follow

    public float camZPostion = -10f;            //default z position, for 2D cameras this is typically -10f
    public float camYmin = 0.0f;                //min height for camera
    public float camYmax = 10.0f;               //max height for camera
    public float camXmin = 0.0f;
    public float camXmax = 50.0f;

    public bool yLocked = false;                //should the camera be locked in place in the y direction
    public bool xLocked = false;                //should the camera be locked in place in the x direction
    private float lastYPosition;                //holds previous position of the camera in the y location, used to keep camera still until platform snap occurs or camera is unlocked
    private float lastXPosition;                //holds previous position of the camera in the x location, used to keep camera still until camera is unlocked

    public bool platformSnap = false;            //should platform snapping be used (move camera only when outside bounds or when player lands on a platform)
    public float minYMoveDist = 1f;             //minimun distance for platform snapping to occur, keeps camera from moving too much on smaller jumps
    public float maxYDistBeforeMove = 3.0f;     //how far the camera should wait for a platform to snap to before just following the player anyway
    
    [Range(0.1f, 5f)]
    public float damping;                       //more damping = slower to follow, low damping = locked on to player
    #endregion
    #region init
    void Start () 
    {
        //get cached components
        //playerController = followTarget.GetComponent<CharacterController2D>();
        cam = GetComponent<Camera>();
        //find the target camera position to start
        Vector3 targetPos = followTarget.position;
        targetPos.z = camZPostion;
        lastYPosition = cam.transform.position.y;
        //create the tween and set it's new target position
        cameraFollowTween = new TransformSpringTween(_transform, TransformTargetType.Position, targetPos);
        cameraFollowTween.dampingRatio = damping;
        cameraFollowTween.start();
	}
    #endregion
    #region update
    void Update()
    {
        if (!paused)
        {
            //reset a new target position each frame, adjusting for any updated target positioning from last frame
            Vector3 newTarget = followTarget.position;
            newTarget.z = camZPostion;
            //adjust this new vec3 based on target position and the flag attributes
            FollowYDirection(ref newTarget);
            FollowXDirection(ref newTarget);
            //set this adjusted target pos as the spring tween's new target
            cameraFollowTween.setTargetValue(newTarget);
        }
    }

    void FollowYDirection(ref Vector3 newTarget)
    {
        //if y is locked, don't let the camera move in the y direction
        if (yLocked)
        {
            newTarget.y = lastYPosition;
        }
        //next check if the target has moved far enough that we should bother looking into it
        else if (Mathf.Abs(Mathf.Abs(newTarget.y) - Mathf.Abs(lastYPosition)) > minYMoveDist)
        {
            //if we are platform snapping, check if we have moved up and are not grounded
            if (platformSnap && newTarget.y > lastYPosition && !playerController.isGrounded)
            {
                //if so, leave the camera be
                newTarget.y = lastYPosition;
            }
                //otherwise check if we have moved below the minimum possible camera position, if so stay at the minimum position
            else if (newTarget.y <= camYmin)
            {
                newTarget.y = camYmin;
                lastYPosition = newTarget.y;
            }
                //otherwise check if the camera has moved above the maximum possible camera position, if so stay at that max
            else if (newTarget.y >= camYmax)
            {
                newTarget.y = camYmax;
                lastYPosition = newTarget.y;
            }
                //otherwise, the camera just needs to follow the target and recognize where it is now for the next frame
            else
            {
                lastYPosition = newTarget.y;
            }
        }
        //if it hasn't moved enough, leave the camera in place
        else
        {
            newTarget.y = lastYPosition;
        }
    }

    void FollowXDirection(ref Vector3 newTarget)
    {
        //if x is locked, don't move the camera
        if(xLocked)
        {
            newTarget.x = lastXPosition;
        }
            //otherwise if the target is within the camera bounds follow the target and recognize where the camera has moved toward for the next frame
        else
        {
            if (newTarget.x <= camXmin || newTarget.x >= camXmax)
            {
                newTarget.x = lastXPosition;
            }
            else
            {
                lastXPosition = newTarget.x;
            }
        }
    }
    #endregion
    #region public functions
    /// <summary>
    /// Simple way of locking the camera via a function call
    /// </summary>
    /// <param name="xDir"> boolean setting the xLocked value </param>
    /// <param name="yDir"> boolean setting the yLocked value</param>
    public void LockCamera(bool xDir = false, bool yDir = false)
    {
        xLocked = xDir;
        yLocked = yDir;
    }

    public override void Pause()
    {
        base.Pause();
        cameraFollowTween.stop();
    }

    public override void UnPause()
    {
        base.UnPause();
        cameraFollowTween.start();
    }
    #endregion
}
