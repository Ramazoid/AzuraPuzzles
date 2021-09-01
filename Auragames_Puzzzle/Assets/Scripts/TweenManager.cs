using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TweenManager : MonoBehaviour
{
    [Inject]
    public Sounds player;

    private Tweener[] Tweeners;
    private Dictionary<string, Tweener> TweenerDic = new Dictionary<string, Tweener>();

    void Start()
    {
        Tweeners = GameObject.FindObjectsOfType<Tweener>();
        
        foreach(Tweener t in Tweeners)
        {
            TweenerDic.Add(t.name, t);

            if(t.fromBottom)
            {
                t.transform.localPosition = new Vector3(0, -1000, 0);
            }
            else
                t.transform.localPosition = new Vector3(0, 1000, 0);
        }

    }

    internal void Hide(string tname, Action cb)
    {
        Tweener t = GetTweenerByName(tname);
        t.GoOff(cb);
    }

    public void Show(string tname, Action cb)
    {
        player.Play("slide");
        Tweener t = GetTweenerByName(tname);
        t.GoOn(cb);
     
    }

    private Tweener GetTweenerByName(string tname)
    {
        Tweener t;
        if (TweenerDic.TryGetValue(tname,out t))
        {
            return t;
        }
        else
        {
            throw new Exception($"Tweener {tname} not found!");

        }
    }

  
}
