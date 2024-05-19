using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class UITestPresenter : UIBasePresenterTemplate<UITest>
{
    public override string PrefabName => "UITest";
    public override UIDepthType UIDepthType => UIDepthType.None;
    
    public PlayerDataManager playerDataManager { get; private set; }

    [Inject]
    public void DependencyInject(PlayerDataManager playerDataManager)
    {
        Debug.Log("UITestPresenter.DependencyInject");
        this.playerDataManager = playerDataManager;
    }
}

public class UITest : UIBaseBehaviourTemplate<UITestPresenter>
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

    private void OnEnable()
    {
        Debug.Log("UITest.OnEnable");
    }

    private void OnDisable()
    {
        Debug.Log("UITest.OnDisable");
    }
}
