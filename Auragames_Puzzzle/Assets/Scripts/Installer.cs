using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
    public GameObject AudioPlayer;
    public override void InstallBindings()
    {
        Container.Bind<Sounds>().FromComponentsInChildren().AsTransient();

    }
}