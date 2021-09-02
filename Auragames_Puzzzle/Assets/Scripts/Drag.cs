using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Drag : MonoBehaviour, IPointerDownHandler
{
   

    private Vector3 startmouse;
    public Vector3 startpos;
    public bool drag;
    public  Vector3 NormalPos;
    internal Action<Drag> goal;
    internal Action<Drag> dragStarted;
    internal int origWidth;
    internal int origHeight;
    internal float scaleX;
    internal float scaleY;

    //[Inject]
    //public Sounds player= new Sounds();

    void Update()
    {
        if(drag)
        {
            Vector3 delta = (Input.mousePosition - startmouse);
            transform.localPosition = startpos + delta;
            

        }
        if (Input.GetMouseButtonUp(0))
        {
            
            //print("Distance=" + dist);
            

            if (drag)
            {
                RectTransform rt = GetComponent<RectTransform>();

                Vector3 checkVector = new Vector3(rt.anchoredPosition.x / scaleX, rt.anchoredPosition.y / scaleY, 0);
                    
            float dist = Vector3.Distance(NormalPos, checkVector);

                print("Normalpos="+NormalPos);
                print("anchored  <piece x=\"" + rt.anchoredPosition.x/scaleX+ "\" y=\""+ rt.anchoredPosition.y / scaleY+"\" />");
                print($"distance={dist}");

                if (dist <= 50f)
                {
                    rt.anchoredPosition=new Vector3(NormalPos.x * scaleX, NormalPos.y * scaleY, 0);
                    Sounds.sPlay("pop");
                    goal(this);
                    
                }
                drag = false;
            }
            }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragStarted(this);
        startmouse = Input.mousePosition;
        startpos = transform.localPosition;
        drag = true;
        

    }

}
