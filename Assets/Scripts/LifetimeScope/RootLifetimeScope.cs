using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        UnityEngine.Debug.Log("RootLifetimeScope.Configure");
    }
}
