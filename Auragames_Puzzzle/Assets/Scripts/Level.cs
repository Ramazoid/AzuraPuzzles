using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Level : MonoBehaviour
{
    [Inject]
    public Sounds player;

    public Image GameField;
    public GameObject piecesShop;
    private Loader Loader;
    private Scroller scroller;
    private float Iwidth;

    public float Iheight;

    private int levelNumber;
    private int piecesNumber;
    private Sprite levelImage;
    private int origGFWidth;
    private int origGFHeight;
    private float scaleX;
    private float scaleY;
    private int piececounter;
    private Dictionary<int, Vector3> pieces;
    private List<Drag> Drags = new List<Drag>();
    public Sprite winTexture;
    public GameObject backButton;
    public GameObject RepeatButton;
    private List<Drag> goaledPieces = new List<Drag>();
    public GameObject backButtonArrow;

    void Start()
    {
        GameField.gameObject.SetActive(false);
        GameField.transform.localPosition = Vector3.zero;
        Loader = GetComponent<Loader>();
        scroller = GetComponent<Scroller>();
        Iwidth = GameField.rectTransform.rect.width;
        Iheight = GameField.rectTransform.rect.height;
        print($"width={Iwidth} height={Iheight}");
    }

    private void Update()
    {
        if (Iwidth != GameField.rectTransform.rect.width || Iheight != GameField.rectTransform.rect.height)
        {
            print("Resized!!!!!!!!!!!!!");
            RescalePieces();
   
            Iwidth = GameField.rectTransform.rect.width;
        }
    }

    private void RescalePieces()
    {
        Iwidth = GameField.rectTransform.rect.width;
        Iheight = GameField.rectTransform.rect.height;

        scaleX = (float)Iwidth / (float)origGFWidth;
        scaleY = (float)Iheight / (float)origGFHeight;

        foreach (Drag d in Drags)
        {
            d.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, d.origWidth * scaleX);
            d.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, d.origHeight * scaleY);
        }

    }

    internal void LoadLevel(Thumb t)
    {
        backButtonArrow.SetActive(true);
        backButton.SetActive(false);
        GameField.gameObject.SetActive(true);

        if(levelNumber!= t.levelNumber)
        {
            levelNumber = t.levelNumber;
           // goaledPieces = new List<Drag>();
        }
        piecesNumber = int.Parse(t.piecesNumber);
        GameField.gameObject.SetActive(true);
        piecesShop.SetActive(true);
        RepeatButton.SetActive(false);


        Loader.LoadTexture("AuraPuzzles/Level" + levelNumber + "/image.jpg", (Texture2D tt) =>
         {
             GameField.sprite = Sprite.Create(tt, new Rect(0, 0, tt.width, tt.height), Vector2.zero);
             levelImage = GameField.sprite;
             origGFWidth = tt.width;
             origGFHeight = tt.height;
             scaleX = (float)Iwidth / (float)tt.width;
             scaleY = (float)Iheight / (float)tt.height;
             print($"scalex={scaleX} scaaley={scaleY}");
         });

        if (piecesShop.transform.childCount > 0)
        {
            RepeatButton.SetActive(false);
            piecesShop.SetActive(true);
        }
        else
            LoadPieces();
    }

    public void RepeatLevel()
    {
        RepeatButton.SetActive(false);
        piecesShop.SetActive(true);
        GameField.sprite = levelImage;
        goaledPieces = new List<Drag>();
        foreach (Drag dd in Drags)
        {

            dd.transform.localPosition = Vector3.zero;
            dd.transform.SetParent(piecesShop.transform);
            dd.gameObject.SetActive(true);

        }
    }
    internal void SaveAndHideLevel()
    {
        GameField.gameObject.SetActive(false);
        piecesShop.gameObject.SetActive(false);
        //foreach (Drag dd in Drags) Destroy(dd.gameObject);
        Drags = new List<Drag>();
        GetComponent<Loader>().LoadLevels();
    }

    void GoalCount(Drag d)
    {
        if (!goaledPieces.Contains(d))
            goaledPieces.Add(d);
        d.transform.SetParent(GameField.transform);
        print("Goaled=" + goaledPieces.Count);
        if (goaledPieces.Count == piecesNumber)
        {
            player.Play("win");
            foreach (Drag dd in Drags) dd.gameObject.SetActive(false);
            GameField.sprite = winTexture;
            scroller.UnlockLevel(levelNumber + 1);
            piecesShop.SetActive(false);
            backButtonArrow.SetActive(false);
            backButton.SetActive(true);
            RepeatButton.SetActive(true);
            goaledPieces = new List<Drag>();
        }

    }
    private void LoadPieces()
    {
        Loader.LoadXML("AuraPuzzles/level" + levelNumber + "/pieces.xml", (string xmlstring) =>
        {
            foreach (Drag dd in Drags) Destroy(dd.gameObject);
            goaledPieces = new List<Drag>();
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlstring)))
            {
                reader.MoveToContent();
                piececounter = 0;
                pieces = new Dictionary<int, Vector3>();
                while (reader.Read())
                {
                    if (!String.IsNullOrEmpty(reader.Name) && reader.Name == "piece")
                    {
                        float x = float.Parse(reader.GetAttribute("x"));
                        float y = float.Parse(reader.GetAttribute("y"));
                        //print($"x={x} y={y}");
                        Vector3 pos = new Vector3(x, y, 0);
                        pieces.Add(++piececounter, pos);
                        AddPieceToShop(piececounter, pos);
                    }

                }

            }
        });
    }
    private void DragStarted(Drag d)
    {
        d.scaleX = scaleX;
        d.scaleY = scaleY;
        d.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, d.origWidth * scaleX);
        d.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, d.origHeight * scaleY);
        d.transform.SetParent(GameField.transform);
    }

    private void AddPieceToShop(int pieñeNumber, Vector3 pos)
    {

        Loader.LoadTexture("AuraPuzzles/level" + levelNumber + "/piece" + pieñeNumber + ".png", (Texture2D t) =>
        {
            GameObject g = new GameObject(); g.name = "Piece_" + pieñeNumber;


            Image p = g.AddComponent<Image>();
            Drag d = g.AddComponent<Drag>();
            Drags.Add(d);
            d.goal = GoalCount;
            d.dragStarted = DragStarted;
            d.NormalPos = pos;
            p.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
            d.origWidth = t.width;
            d.origHeight = t.height;
            

            g.transform.SetParent(piecesShop.transform);
            g.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, t.width * scaleX);
            g.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, t.height * scaleY);
            d.startpos = g.transform.localPosition;
        });


    }
}
