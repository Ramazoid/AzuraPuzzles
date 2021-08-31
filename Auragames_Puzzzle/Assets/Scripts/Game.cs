using UnityEngine;

public class Game : MonoBehaviour
{
    private TweenManager TWenMan;
    private Level Level;
    private Scroller scroller;
    public int currentLevel;

    void Start()
    {
      
        TWenMan = GetComponent<TweenManager>();
        Level = GetComponent<Level>();
        scroller = GetComponent<Scroller>();
        TWenMan.Show("WelcomeText",()=>
        {
            TWenMan.Show("Logo",()=> {
                TWenMan.Show("PlayButton", null);
            });
        });
    }

    public void HideWelcomeAndStart()
    {
        Sounds.Play("pop");
        TWenMan.Hide("WelcomeText", () =>
        {
            TWenMan.Hide("Logo", () => {
                TWenMan.Hide("PlayButton", ()=> {
                    GetComponent<Loader>().LoadLevels();
                });
            });
        });
    }
    public void InitLevel(Thumb t)
    {
        if (t.locked) return;
        scroller.GoOff();
        Level.LoadLevel(t);
        Sounds.Play("pop");

    }
    public void BackButton()
    {
        Sounds.Play("pop");
        Level.SaveAndHideLevel();
        scroller.GoOn();
    }
    public void RepeatButton()
    {
        Sounds.Play("pop");
        print("repeat");
        Level.RepeatLevel();
    }
}
