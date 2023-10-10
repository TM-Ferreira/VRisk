using System;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class MenuController : MonoBehaviour
{
    private enum OpenMode {
        SCALE,
        SLIDE
    }

    private enum TweenType {
        LERP,
        LEAN
    }

    public Vector3 targetPos;
    public float animationTime = 0.5f;
    public float lerpSpeed = 2.5f;
    
    [SerializeField] private OpenMode openMode = OpenMode.SCALE;
    [SerializeField] private OpenMode closeMode = OpenMode.SCALE; 
    [SerializeField] private TweenType tweenTypeOpen = TweenType.LEAN;
    [SerializeField] private TweenType tweenTypeClose = TweenType.LEAN;

    [SerializeField] private Vector3 defaultPos;
    [SerializeField] private Vector3 awayPos; 
    
    [SerializeField] private bool startOpen = false;
    [SerializeField] private bool open = true;

    void Start()
    {
        defaultPos = transform.localPosition;
        
        if (!startOpen)
        {
            switch (openMode)
            {
                case OpenMode.SCALE:
                    transform.localScale = new Vector3(0, 0, 1);
                    break;
                
                case OpenMode.SLIDE:
                    transform.localPosition = awayPos;
                    break;
            }

            targetPos = awayPos;
            open = false;
        }
    }

    public void OpenMenu()
    {
        open = true;

        switch (tweenTypeOpen)
        {
            case TweenType.LERP:
                transform.localScale = new Vector3(1, 1, 1);
                transform.position = awayPos;
                targetPos = defaultPos;
                break;

            case TweenType.LEAN:
                switch (openMode)
                {
                    case OpenMode.SCALE:
                        transform.localPosition = defaultPos;
                        transform.LeanScale(new Vector3(1,1,1), animationTime);
                        break;
            
                    case OpenMode.SLIDE:
                        transform.localScale = new Vector3(1, 1, 1);
                        transform.LeanMoveLocal(defaultPos, animationTime).setEaseInOutBack();
                        break;
                }
                break;
        }
    }

    public void CloseMenu()
    {
        open = false;

        switch (tweenTypeClose)
        {
            case TweenType.LERP:
                transform.localScale = new Vector3(1, 1, 1);
                transform.position = defaultPos;
                targetPos = awayPos;
                break;
            
            case TweenType.LEAN:
                switch (closeMode)
                {
                    case OpenMode.SCALE:
                        transform.LeanScale(new Vector3(0,0,1), animationTime).setEaseInBack();
                        break;
            
                    case OpenMode.SLIDE:
                        transform.LeanMoveLocal(awayPos, animationTime).setEaseInOutBack();
                        break;
                }
                break;
        }
    }

    private void Update()
    {
        if (tweenTypeOpen == TweenType.LERP || tweenTypeClose == TweenType.LERP)
        {
            if (!VectorHelper.ApproximatelyEqual(transform.localPosition, targetPos, 0.01f))
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, lerpSpeed * Time.deltaTime);
            }
        }
    }
    
    public void ToggleOpenClose()
    {
        if (open)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
}
