using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;

        var uiManager = FindObjectOfType<UIManager>();
        uiManager.OpenUI<UITestPresenter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
