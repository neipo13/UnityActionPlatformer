using UnityEngine;
using Prime31.ZestKit;

public class SkewOnCollision : PausableMonoBehavior
{
    protected Material _material;               //cached material component for zestkit tweening horizontal skew

    public float maxSkew = 1.0f;                //horizontal skew amount, experiment in the inspector on the material to see what an approprite skew is for your usage
    public float skewTime = 0.5f;               //time in seconds to skew to max and then to skew back

    protected ITween<float> skewTween;          //holder for skew outward
    protected ITween<float> returnTween;        //holder for skew back to 0

    public override void Awake()
    {
        base.Awake();
        _material = GetComponent<SpriteRenderer>().material;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        //use the parent object's scale to set the skew direction (by direction they are facing)
        float skew = maxSkew;
        if (col.GetComponent<Transform>().parent.localScale.x < 0)
        {
            skew *= -1;
        }

        //create tweens and set easing type to punch for fast inital movement
        skewTween = _material.ZKfloatTo(skew, skewTime, "_HorizontalSkew");
        skewTween.setEaseType(EaseType.Punch);
        returnTween = _material.ZKfloatTo(0.0f, skewTime, "_HorizontalSkew");
        returnTween.setEaseType(EaseType.Punch);
        //set returnTween to play immediately after skewTween
        skewTween.setNextTween(returnTween);
        //start the tween
        skewTween.start();
        
    }
}