using Zenject;

public class BoardInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<BoardController>().AsSingle();
    }
}