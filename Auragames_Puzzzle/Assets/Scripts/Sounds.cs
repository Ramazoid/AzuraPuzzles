using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public List<AudioClip> clips;
    private static  Sounds INST;
    private AudioSource Player;

    void Start()
    {
        INST = this;
        Player = GetComponent<AudioSource>();
        
    }
    
    
    public void Play(string clipName)
    {
        INST.Player.Stop();
        INST.Player.clip = GetClipByName(clipName);
        INST.Player.Play();

    }

    private static AudioClip GetClipByName(string clipName)
    {
        foreach (AudioClip c in INST.clips)
            if (c.name == clipName)
                return c;
        throw new Exception($"Audioclip [{clipName}] not found!!");
    }

    internal static void sPlay(string clipName)
    {
        INST.Play(clipName);
    }
}
