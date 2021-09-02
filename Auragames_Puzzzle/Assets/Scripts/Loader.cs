using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class Loader : MonoBehaviour
{
    const string HOST = "http://ramazoid.ru/";
    private Scroller scroller;
    private TweenManager TWenMan;
    public GameObject LoadingIcon;
    
    private Dictionary<int, string> levelPieces = new Dictionary<int, string>();

   
    void Start()
    {
        LoadingIcon.SetActive(false);
        scroller = GetComponent<Scroller>();
        TWenMan = GetComponent<TweenManager>();

    }

    internal void LoadTexture(string url, Action<Texture2D> callback)
    {
           LoadingIcon.SetActive(true);
        StartCoroutine(LoadImageTexture(url, callback));
    }
    IEnumerator LoadImageTexture(string url, Action<Texture2D> callback)
    {
        
        WWW www = new WWW(HOST+url);
        yield return www;
        if (callback != null)
            callback(www.texture);
        LoadingIcon.SetActive(false);
        www.Dispose();www = null;

    }
    IEnumerator GetRequest(string uri, Action<string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    if (callback != null)
                    {
                        callback(webRequest.downloadHandler.text);
                        callback = null;
                    }
                    break;
            }
        }
    }

    internal void LoadXML(string url, Action<string> callback)
    {

        StartCoroutine(GetRequest(HOST + url, callback));
    }

    internal void LoadLevels()
    {
        LoadingIcon.SetActive(true);
        StartCoroutine(GetRequest(HOST+"AuraPuzzles/levels.xml", GetLevels));
    }

    private void GetLevels(string text)
    {
        using (XmlReader reader = XmlReader.Create(new StringReader(text)))
        {
            reader.MoveToContent();
            int levelcounter = 0;
            levelPieces = new Dictionary<int, string>();
            while (reader.Read())
            {
                if (!String.IsNullOrEmpty(reader.Name) && reader.Name == "level")
                {

                    levelPieces.Add(++levelcounter, reader.GetAttribute("pieces"));
                    //print(reader.GetAttribute("name"));
                    LoadThumbnail(levelcounter);
                }

                LoadingIcon.SetActive(false);
               
            }
            TWenMan.Show("LevelScroller", null);
        }
    }
    private void LoadThumbnail(int levelnumber)
    {
        string url= HOST+"AuraPuzzles/Level"+levelnumber+"/thumb.jpg";
       StartCoroutine(LoadImage(url, levelnumber));
    } 
    IEnumerator LoadImage(string url,int levelnumber)
    {
        WWW www = new WWW(url);
        yield return www;
        //print("loaded " + url);
        scroller.AddLevel(www.texture,levelPieces[levelnumber],  levelnumber);

    }
}
