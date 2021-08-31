using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thumb : MonoBehaviour
{
    public Image thumim;
    public Image Locker;
    public bool locked;
    public string levelName;
    internal int levelNumber;
    internal string piecesNumber;

    internal void SetThumb(Texture2D texture, string levelName, bool locked)
    {
        this.locked = locked;
        this.levelName = levelName;
       thumim.sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), Vector2.zero);
        this.locked = locked;
        Locker.gameObject.SetActive(locked);
    }

    internal void Unlock()
    {
        Locker.gameObject.SetActive(false);
        this.locked = false;
    }
}
