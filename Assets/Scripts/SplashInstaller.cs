using Zenject;

public class SplashInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SplashController>().AsSingle();

    }
}