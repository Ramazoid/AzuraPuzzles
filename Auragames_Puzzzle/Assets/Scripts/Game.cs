using UnityEngine;
using Zenject;

public class Game : MonoBehaviour
{
    [Inject]
    public Sounds player;

    private TweenManager TWeenMan;
    private Level Level;
    private Scroller scroller;
    public int currentLevel;

    void Start()
    {

        TWeenMan = GetComponent<TweenManager>();
        Level = GetComponent<Level>();
        scroller = GetComponent<Scroller>();
        TWeenMan.Show("WelcomeText",()=>
        {
            TWeenMan.Show("Logo",()=> {
                TWeenMan.Show("PlayButton", null);
            });
        });
    }

    public void HideWelcomeAndStart()
    {
        player.Play("pop");
        TWeenMan.Hide("WelcomeText", () =>
        {
            TWeenMan.Hide("Logo", () => {
                TWeenMan.Hide("PlayButton", ()=> {
                    GetComponent<Loader>().LoadLevels();
                });
            });
        });
    }
    public void InitLevel(Thumb t)
    {
        scroller.scroller.gameObject.SetActive(false);
        if (scroller.wasSlided) return;
        //scroller.GoOff();
        scroller.ClearThumbs();
        Level.LoadLevel(t);
        player.Play("pop");
        

    }
    public void BackButton()
    {
       
        player.Play("pop");
        Level.SaveAndHideLevel();
        scroller.GoOn();
        scroller.scroller.gameObject.SetActive(true);
    }
    public void RepeatButton()
    {
        player.Play("pop");
        print("repeat");
        Level.RepeatLevel();
    }
}
