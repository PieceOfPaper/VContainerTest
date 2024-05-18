using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

public class UILifetimeScope : LifetimeScope
{
    private readonly static System.Type[] UI_PRESENTER_TYPES =
    {
        typeof(IuiTestPresenter),
    };

    protected override void Configure(IContainerBuilder builder)
    {
        UnityEngine.Debug.Log("UILifetimeScope.Configure");
        for (var i = 0; i < UI_PRESENTER_TYPES.Length; i ++)
        {
            EntryPointsBuilder.EnsureDispatcherRegistered(builder);
            builder.Register(UI_PRESENTER_TYPES[i], Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
