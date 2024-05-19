using VContainer;
using VContainer.Unity;
using UnityEngine;

public class UIManagerLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        UnityEngine.Debug.Log("UIManagerLifetimeScope.Configure");

        builder.Register<UIManagerFactory>(Lifetime.Singleton);
        builder.RegisterFactory<System.Type, UIBasePresenter>(container =>
        {
            return type => container.Resolve<UIManagerFactory>().CreatePresenter(container, type);
        }, Lifetime.Singleton);
        builder.RegisterFactory<UIBasePresenter, GameObject>(container =>
        {
            return presenter => container.Resolve<UIManagerFactory>().Instantiate(container, presenter);
        }, Lifetime.Singleton);
    }
}
