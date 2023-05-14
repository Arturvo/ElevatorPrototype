using ZadanieRekrutacyjne.Core.AudioSystem;
using ZadanieRekrutacyjne.Core.InputSystem;
using ZadanieRekrutacyjne.Core.SceneLoader;
using Zenject;

namespace ZadanieRekrutacyjne.Core.DependencyInjection
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ISceneLoader>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IAudioManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}