using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public Thumb levelThumb;
    public GameObject scroller;

    internal static Scroller INST;
    private Dictionary<int, Thumb> Thumbs = new Dictionary<int, Thumb>();

    void Start()
    {
        INST = this;
        levelThumb.gameObject.SetActive(false);
    }

    internal void AddLevel(Texture2D texture, string levelName, string pieces, int levelNumber)
    {

        Thumb g = GameObject.Instantiate(levelThumb, scroller.transform);
        g.transform.SetAsLastSibling();
        g.levelNumber = levelNumber;
        g.piecesNumber = pieces;
        g.gameObject.SetActive(true);
        g.SetThumb(texture, levelName,levelNumber>3);
        Thumb t = g.GetComponent<Thumb>(); 
        Thumbs.Add(levelNumber, t);

    }

    public void GoOff()
    {
        Tweener t = scroller.GetComponent<Tweener>();
        t.targetPosition = t.gameObject.transform.localPosition + new Vector3(500, 0, 0);
        t.GoOn(null);  
    }
    public void GoOn()
    {
        Sounds.Play("slide");
        Tweener t = scroller.GetComponent<Tweener>();
        t.movingOff = true;
        t.GoOn(null);
    }

    internal void UnlockLevel(int levelnumber)
    {
        Thumb t;
        Thumbs.TryGetValue(levelnumber,out t);
        print("founded" + t.name);
        t.Unlock();
    }
}
