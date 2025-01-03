using Zenject;

public class FlagQuizInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<FlagQuizController>().AsSingle();

    }
}