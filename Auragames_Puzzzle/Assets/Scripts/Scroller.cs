using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Scroller : MonoBehaviour
{
    [Inject]
    public Sounds player;

    public Thumb levelThumb;
    public GameObject scroller;

    internal static Scroller INST;
    private Dictionary<int, Thumb> Thumbs = new Dictionary<int, Thumb>();
    private bool slide;
    private float afterSlide;
    private float startMouseY;
    private Vector3 startPosition;
    public bool wasSlided;
    private float lastMouseY;
    private float slideSpeed;

    void Start()
    {
        INST = this;
        levelThumb.gameObject.SetActive(false);
    }
    public void StartSlide()
    {
        wasSlided = false;
        slide = true;
        startMouseY = Input.mousePosition.y;
        startPosition = scroller.transform.localPosition;
    }
    public void StopSlide()
    {
        slide = false;
        afterSlide = slideSpeed * 10;
    }
    private void Update()
    {
        
        if (slide)
        {
            float slideDelta = Input.mousePosition.y - startMouseY;
            slideSpeed = Input.mousePosition.y - lastMouseY;
            if (slideDelta != 0)
                wasSlided = true;
            scroller.transform.localPosition = startPosition + Vector3.up * slideDelta;
            lastMouseY = Input.mousePosition.y;
        }
        else if (afterSlide != 0)
        {
            scroller.transform.localPosition += Vector3.up * (afterSlide > 0 ? 1 : -1);
            afterSlide /= 5;
        }
        
    }

    internal void AddLevel(Texture2D texture,  string pieces, int levelNumber)
    {

        Thumb g = GameObject.Instantiate(levelThumb, scroller.transform);
        g.transform.SetAsLastSibling();
        g.levelNumber = levelNumber;
        g.piecesNumber = pieces;
        g.gameObject.SetActive(true);

        string levelPrefs = (levelNumber > 1 ? "locked" : "available");


        if (PlayerPrefs.HasKey("Level" + levelNumber))
        {
            levelPrefs = PlayerPrefs.GetString("Level" + levelNumber);
        }
        PlayerPrefs.SetString("Level" + levelNumber, levelPrefs);

        g.SetThumb(texture,levelPrefs);
        g.levelNumber = levelNumber;
        
        Thumb t = g.GetComponent<Thumb>(); 
        Thumbs.Add(levelNumber, t);
        LayoutRebuilder.ForceRebuildLayoutImmediate(scroller.GetComponent<RectTransform>());

    }

    public void GoOff()
    {
        Tweener t = scroller.GetComponent<Tweener>();
        t.targetPosition = t.gameObject.transform.localPosition + new Vector3(500, 0, 0);
        foreach (KeyValuePair<int,Thumb> th in Thumbs) th.Value.DisposeTexture();
        t.GoOn(null);
        Thumbs = new Dictionary<int, Thumb>();
    }
    public void GoOn()
    {
        player.Play("slide");
        Tweener t = scroller.GetComponent<Tweener>();
        t.movingOff = true;
        t.GoOn(null);
    }

    internal void UnlockLevel(int levelNumber)
    {
        PlayerPrefs.SetString("Level" + levelNumber, "available");
        
    }
}
