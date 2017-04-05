using UnityEngine;
using Prime31.ZestKit;

public class BouncyCharacter : PausableMonoBehavior 
{
    //MUST BE SET IN AWAKE
    protected Vector3 startingScale;
    protected ITween<Vector3> bounceTween;

    //Both do the same thing but are separated to make it easier when calling
    public void Squash(Transform trans, float xStretch, float ySquash, float time)
    {
        trans.localScale = new Vector3(startingScale.x * xStretch, startingScale.y * ySquash, startingScale.z);
        bounceTween = trans.ZKlocalScaleTo(startingScale, time);
        bounceTween.start();
    }

    public void Stretch(Transform trans, float xSquash, float yStretch, float time)
    {
        trans.localScale = new Vector3(startingScale.x * xSquash, startingScale.y * yStretch, startingScale.z);
        bounceTween = trans.ZKlocalScaleTo(startingScale, time);
        bounceTween.start();
    }

    //override pause and unpause to stop/start the tween respectively
    public override void Pause()
    {
        base.Pause();
        if(bounceTween != null && bounceTween.isRunning())
            bounceTween.stop();
    }

    public override void UnPause()
    {
        base.UnPause();
        if (bounceTween != null && bounceTween.isRunning())
            bounceTween.start();
    }
}