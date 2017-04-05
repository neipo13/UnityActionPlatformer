using UnityEngine;
using Prime31.ZestKit;

public class CameraShake : MonoBehaviour 
{
    //cached attributes
    private Camera cam;
    private CameraShakeTween camShaker;

    //public attributes for default shakes
    [Range(0.0f, 1.0f)]
    public float defaultIntensity = 0.3f;
    [Range(0.0f, 1.0f)]
    public float defaultyDegredation = 0.95f;
    [Range(-1, 1)]
    public int horizontalDirection = 0;
    [Range(-1, 1)]
    public int verticalDirection = 0;

    //USED FOR NON-MOVING CAMERAS
    private Transform _transform;
    private Vector3 originalCamPos;
    private bool centered;


    void Start()
    {
        //cache important components
        cam = GetComponent<Camera>();

        //USED FOR NON-MOVING CAMERAS
        _transform = GetComponent<Transform>();
        originalCamPos = new Vector3(0f, 0f, -10f);
        centered = true;

        //create the tween for future use with default values
        camShaker = new CameraShakeTween(cam, 0.3f, 0.95f, Vector3.zero);
    }

    /// <summary>
    /// Called when the user would like the shake the camera, accepts a horizontal direction allowing the user to shake
    /// the camera in a specific direction rather than all over if desired
    /// </summary>
    /// <param name="horizdir">-1 to shake to left, 0 to shake all over, 1 to shake right, assume general shake unless direction specified</param>
    /// <param name="horizdir">-1 to shake to down, 0 to shake all over, 1 to shake up, assume general shake unless direction specified</param>
    public void Shake(float intensity, float degredation, int horizDir = 0, int verticalDir = 0)
    {
        camShaker.shake(intensity, degredation, new Vector3(horizDir, verticalDir, 0f));
        //USED FOR NON-MOVING CAMERAS
        //originalCamPos = _transform.position;
        centered = false;
    }

    public void Shake()
    {
        camShaker.shake(defaultIntensity, defaultyDegredation, new Vector3(horizontalDirection, verticalDirection, 0f));
    }

        //USED FOR NON-MOVING CAMERAS
    void FixedUpdate()
    {
        if(!centered)
        {
            _transform.position = Vector3.Lerp(_transform.position, originalCamPos, 0.1f);
            if(Mathf.Abs(_transform.position.x) < 0.001f && Mathf.Abs(_transform.position.y) < 0.001f)
            {
                //Debug.Log("RECENTERED");
                _transform.position = originalCamPos;
                centered = true;
            }
        }
    }

    public void Center()
    {
        centered = false;
    }
}