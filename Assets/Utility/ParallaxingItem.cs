using UnityEngine;
using System.Collections;

public class ParallaxingItem : MonoBehaviour 
{
    private Transform _transform;
    private Transform _cameraTransform;
    private CameraFollow _cameraFollow;
    private float cameraWidth;
    private float spriteWidth;
    private float cameraLeft;
    private float debug;

    public bool followsCameraY = false;
    public float xSizeOfGroup = 0.0f;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        _cameraTransform = Camera.main.GetComponent<Transform>();
        cameraLeft = Camera.main.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height;
        if (GetComponent<SpriteRenderer>() != null)
            spriteWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        else
            spriteWidth = xSizeOfGroup + cameraLeft;
        cameraWidth = _cameraFollow.camXmax - _cameraFollow.camXmin;
        _transform.position = new Vector3(_cameraTransform.position.x - cameraLeft, _transform.position.y, _transform.position.z);
    }

    void LateUpdate()
    {
        float currentCameraPercentage = (_cameraTransform.position.x  - _cameraFollow.camXmin) / cameraWidth;
        float newSpritePosition = (_cameraTransform.position.x - cameraLeft) - (spriteWidth * currentCameraPercentage) + (2 *cameraLeft * currentCameraPercentage);
        Vector3 newPosition = new Vector3(newSpritePosition, _transform.position.y, _transform.position.z);
        if(followsCameraY)
        {
            newPosition.y = _cameraTransform.position.y;
        }
        _transform.position = newPosition;
    }




}
