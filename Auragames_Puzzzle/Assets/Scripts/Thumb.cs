using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thumb : MonoBehaviour
{
    public Image thumim;
    public Image Locker;
    public string locked;
    internal int levelNumber;
    internal string piecesNumber;

    internal void SetThumb(Texture2D texture,string locked)
    {
        this.locked = locked;
        
       thumim.sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), Vector2.zero);
        this.locked = locked;
        Locker.gameObject.SetActive(locked!="available");
    }

    internal void Unlock()
    {
        Locker.gameObject.SetActive(false);
        locked = "available";
        PlayerPrefs.SetString("Level" + levelNumber, locked);
    }

    internal void DisposeTexture()
    {
        Destroy(gameObject );
        
    }
}
