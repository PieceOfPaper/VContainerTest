using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public partial class PlayerDataManager
{
    public PlayerDataManager()
    {
        Debug.Log("new PlayerDataManager");
    }


    private ReactiveProperty<long> m_UID = new ReactiveProperty<long>();
    public IReadOnlyReactiveProperty<long> UID => m_UID;
    
    private ReactiveProperty<string> m_Nickname = new ReactiveProperty<string>();
    public IReadOnlyReactiveProperty<string> Nickname => m_Nickname;
}
