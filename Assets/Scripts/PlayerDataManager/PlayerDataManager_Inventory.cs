using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ItemInfo
{
    public long uid;
    public int id;
}

public class InventoryInfo
{
    private ReactiveDictionary<long, ItemInfo> m_ItemInfos = new ReactiveDictionary<long, ItemInfo>();
    public IReadOnlyReactiveDictionary<long, ItemInfo> ItemInfos => m_ItemInfos;
}

public partial class PlayerDataManager
{
    private InventoryInfo m_InventoryInfo;
    public InventoryInfo InventoryInfo => m_InventoryInfo;
}
