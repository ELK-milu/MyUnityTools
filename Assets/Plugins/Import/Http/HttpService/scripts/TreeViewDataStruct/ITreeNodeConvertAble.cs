using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlusbeQuickPlugin.HttpService
{
    /// <summary>
    /// 可以被转化为树状图数据的接口
    /// </summary>
    public interface ITreeNodeConvertAble
    {
        int ID { get; }
        int ParentID { get; }
        string Name { get; }
        object NodeData { get; }
    }
}

