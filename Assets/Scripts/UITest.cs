using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class IuiTestPresenter : IuiBasePresenterTemplate<IuiTest>
{
    public override string PrefabName => "TODO";
    public override UIDepthType UIDepthType => UIDepthType.None;
    
    public PlayerDataManager playerDataManager { get; private set; }

    [Inject]
    public void DependencyInject(PlayerDataManager playerDataManager)
    {
        Debug.Log("UITestPresenter.DependencyInject");
        this.playerDataManager = playerDataManager;
    }
}

public class IuiTest : IuiBaseBehaviourTemplate<IuiTestPresenter>
{
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Debug.Log("UITest.OnInitialized");
    }

    private void Awake()
    {
        Debug.Log("UITest.Awake");
    }

    private void Start()
    {
        Debug.Log("UITest.Start");
    }
}
