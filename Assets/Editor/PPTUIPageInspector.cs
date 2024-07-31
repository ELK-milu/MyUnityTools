using DG.Tweening;
using System;
using UnityEditor;
[CustomEditor(typeof(PPTUIPage))]
public class PPTUIPageInspector : Editor
{
	private SerializedObject obj;
	private PPTUIPage pptuiPage;
	private SerializedProperty type;
    
	void OnEnable()
	{
		obj = new SerializedObject(target);
	}

	public override void OnInspectorGUI()
	{
		pptuiPage = (PPTUIPage)target;
		EditorGUILayout.HelpBox("入场动画",MessageType.None);
		pptuiPage.OnOpenAnimation = (bool)EditorGUILayout.Toggle("OnOpenAnimation", pptuiPage.OnOpenAnimation);
		ChoiceAnimParamField(pptuiPage.OnOpenAnimation, "OpenAnimComponentManager");
		
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("出场动画",MessageType.None);
		pptuiPage.OnCloseAnimation = (bool)EditorGUILayout.Toggle("OnCloseAnimation", pptuiPage.OnCloseAnimation);
		ChoiceAnimParamField(pptuiPage.OnCloseAnimation, "CloseAnimComponentManager");
		
		obj.ApplyModifiedProperties();
	}

	private void ChoiceAnimParamField(bool thisAnimation, string thisAnim)
	{
		SerializedProperty thisAnimParam = obj.FindProperty(thisAnim);
		if(thisAnimation)
		{
			EditorGUILayout.PropertyField(thisAnimParam,true);
		}
	}
	
}
