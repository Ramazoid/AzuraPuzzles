using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    public Vector3 targetPosition;
    public bool fromBottom;
    public Vector3 endPosition;
    public Action callback;
    public bool movingOn;
    public bool movingOff;
    internal float frac=0.1f;

    void Start()
    {
        targetPosition = transform.localPosition;
    }

    
    void Update()
    {
        if (movingOn)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, frac);
            if(Vector3.Distance(transform.localPosition,targetPosition)<0.01f)
            {
                movingOn = false;
                if (callback != null)
                {
                    callback();
                    callback = null;
                }
            }
        }
        else
            if (movingOff)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, endPosition, 0.1f);
            if (Vector3.Distance(endPosition, transform.localPosition) < 0.01f)
            {
                movingOff = false;
                if (callback != null)
                {
                    callback();
                    callback = null;
                }
            }
        }
    }
    internal void GoOn(Action cb)
    {
        callback = cb;
        movingOn = true;
    }
    internal void GoOff(Action cb)
    {
        endPosition = new Vector3(0, Screen.height, 0);
        callback = cb;
        movingOff = true;
    }
}
