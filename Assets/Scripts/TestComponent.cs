using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComponent : MonoBehaviour
{
    [VContainer.Inject]
    public void DependencyInjection(UIManager uiManager)
    {
        Debug.Log($"TestComponent.DependencyInjection");
    }


    private void Awake()
    {
        Debug.Log($"TestComponent.Awake");
    }

    private void Start()
    {
        Debug.Log($"TestComponent.Start");
    }
}
