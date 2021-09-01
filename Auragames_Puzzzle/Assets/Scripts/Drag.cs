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

   [Inject]
    public Sounds player= new Sounds();

    void Update()
    {
        if(drag)
        {
            Vector3 delta = (Input.mousePosition - startmouse);
            transform.localPosition = startpos + delta;
            

        }
        if (Input.GetMouseButtonUp(0))
        {
            float dist = Vector3.Distance(NormalPos, transform.localPosition);
            //print("Distance=" + dist);
            

            if (drag)
            {
                //print($"<piece x=\"" + transform.localPosition.x + "\" y=\"" + transform.localPosition.y + "\" />");

                if (dist <= 10f)
                {
                    transform.localPosition = NormalPos;
                    player.Play("pop");
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
