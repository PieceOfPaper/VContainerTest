using VContainer;
using VContainer.Unity;

public class ManagerLifetimeScope : LifetimeScope
{
    public UIManager m_UIManager;
    
    protected override void Configure(IContainerBuilder builder)
    {
        UnityEngine.Debug.Log("ManagerLifetimeScope.Configure");
        builder.Register<PlayerDataManager>(Lifetime.Singleton);
        builder.RegisterComponent(m_UIManager);
    }
}
