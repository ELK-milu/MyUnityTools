using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Profiling;

public class TestFirstWindow : UIWindowBase 
{

    public Text txtDebug;

    //UI打开的初始化请放在这里
    public override void OnOpen()
    {
        AddOnClickListener("btn_1", OnClick1,"我是参数");

       
    }

    

    private void OnClick1(InputUIOnClickEvent e)
    {
        Debug.Log("btn1" + "," + e.m_param);

        UIManager.OpenUIWindow<TestSendWindow>(this);
    }

    //private readonly static string TotalAllocMemroyFormation = "Alloc Memory : {0}M";
    //private readonly static string TotalReservedMemoryFormation = "Reserved Memory : {0}M";
    //private readonly static string TotalUnusedReservedMemoryFormation = "Unused Reserved: {0}M";
    ////private readonly static string RuntimeMemorySizeFormation = "RuntimeMemorySize: {0}M";
    //private readonly static string MonoHeapFormation = "Mono Heap : {0}M";
    //private readonly static string MonoUsedFormation = "Mono Used : {0}M";
    //// 字节到兆
    //private float ByteToM = 0.000001f;

    //GUI.Label(this.allocMemoryRect,
    //    string.Format(TotalAllocMemroyFormation, Profiler.GetTotalAllocatedMemory() * ByteToM));
    //GUI.Label(this.reservedMemoryRect,
    //    string.Format(TotalReservedMemoryFormation, Profiler.GetTotalReservedMemory() * ByteToM));
    //GUI.Label(this.unusedReservedMemoryRect,
    //    string.Format(TotalUnusedReservedMemoryFormation, Profiler.GetTotalUnusedReservedMemory() * ByteToM));

    //GUI.Label(this.monoHeapRect,
    //    string.Format(MonoHeapFormation, Profiler.GetMonoHeapSize() * ByteToM));
    //GUI.Label(this.monoUsedRect,
    //    string.Format(MonoUsedFormation, Profiler.GetMonoUsedSize() * ByteToM));
}