using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public Image GameField;
    public GameObject piecesShop;
    private Loader Loader;
    private Scroller scroller;
    private int levelNumber;
    private int piecesNumber;
    private Sprite levelImage;
    private float scaleX;
    private float scaleY;
    private int piececounter;
    private Dictionary<int, Vector3> pieces;
    private List<Drag> Drags = new List<Drag>();
    public Sprite winTexture;
    public GameObject BackButton;
    public GameObject RepeatButton;
    private List<Drag> goaledPieces = new List<Drag>();

    void Start()
    {
        GameField.gameObject.SetActive(false);
        GameField.transform.localPosition = Vector3.zero;
        Loader = GetComponent<Loader>();
        scroller = GetComponent<Scroller>();
    }

   
    internal void LoadLevel(Thumb t)
    {

        if(levelNumber!= t.levelNumber)
        {
            levelNumber = t.levelNumber;
            goaledPieces = new List<Drag>();
        }
        piecesNumber = int.Parse(t.piecesNumber);
        GameField.gameObject.SetActive(true);
        piecesShop.SetActive(true);
        RepeatButton.SetActive(false);


        Loader.LoadTexture("http://ramazoid.ru/AuraPuzzles/Level" + levelNumber + "/image.jpg", (Texture2D t) =>
         {
             GameField.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
             levelImage = GameField.sprite;
             scaleX = (float)250 / (float)t.width;
             scaleY = (float)250 / (float)t.height;
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
    }

    void GoalCount(Drag d)
    {
        if (!goaledPieces.Contains(d))
            goaledPieces.Add(d);
        d.transform.SetParent(GameField.transform);
        print("Goaled=" + goaledPieces.Count);
        if (goaledPieces.Count == piecesNumber)
        {
            Sounds.Play("win");
            foreach (Drag dd in Drags) dd.gameObject.SetActive(false);
            GameField.sprite = winTexture;
            scroller.UnlockLevel(levelNumber + 1);
            piecesShop.SetActive(false);
            RepeatButton.SetActive(true);
        }

    }
    private void LoadPieces()
    {
        Loader.LoadXML("http://ramazoid.ru/AuraPuzzles/level" + levelNumber + "/pieces.xml", (string xmlstring) =>
        {
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
        d.transform.SetParent(piecesShop.transform);
    }

    private void AddPieceToShop(int pieñeNumber, Vector3 pos)
    {

        Loader.LoadTexture("http://ramazoid.ru/AuraPuzzles/level" + levelNumber + "/piece" + pieñeNumber + ".png", (Texture2D t) =>
        {
            GameObject g = new GameObject(); g.name = "Piece_" + pieñeNumber;


            Image p = g.AddComponent<Image>();
            Drag d = g.AddComponent<Drag>();
            Drags.Add(d);
            d.goal = GoalCount;
            d.dragStarted = DragStarted;
            d.NormalPos = pos;
            p.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
            float newWidth = t.width * scaleX;

            g.transform.SetParent(piecesShop.transform);
            g.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, t.width * scaleX);
            g.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, t.height * scaleY);
            d.startpos = g.transform.localPosition;
        });


    }
}
