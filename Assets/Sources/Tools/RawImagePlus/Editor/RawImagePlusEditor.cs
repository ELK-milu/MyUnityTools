using UnityEngine;
using UnityEditor.UI;
using UnityEditor;
/// <summary>
/// --(*^__^*) --
/// ____AUTHOR:    Luojie
/// ____DESC:      文件描述
/// ____UNITYVERSION:  2018.4.6f1
/// --(＝^ω^＝) --
/// </summary>
[CustomEditor(typeof(RawImagePlus))]
public class RawImagePlusEditor : RawImageEditor
{
    private RawImagePlus rawImagePlus;
    protected override void OnEnable()
    {
        base.OnEnable();
        if (rawImagePlus == null)
            rawImagePlus = target as RawImagePlus;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        rawImagePlus.ScaleMode = (ScaleMode)EditorGUILayout.EnumPopup("ScaleMode", rawImagePlus.scaleMode);
    }
    [MenuItem("GameObject/UI/Extension/RawImagePlus")]
    static void AddButtonPlus()
    {
        GameObject go = new GameObject("ButtonPlus");
        go.AddComponent<RawImagePlus>();
        go.transform.SetParent(Selection.activeTransform);
        go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Selection.activeGameObject = go;
    }
}
