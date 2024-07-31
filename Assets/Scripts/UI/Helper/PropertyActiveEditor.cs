using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace InspectorEx
{
	[CustomPropertyDrawer(typeof(PropertyActiveAttribute), true)]
	public class PropertyActiveEditor : PropertyDrawer
	{
		private bool isShow = false;

		/// <summary>
		/// 修改属性占用高度
		/// </summary>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var attr = attribute as PropertyActiveAttribute;
			var field = attr.field;
			var compareType = attr.compareType;
			var compareValue = attr.compareValue;

			var parent = property.GetActualObjectParent();
			var fieldInfo = parent.GetType().GetField(field);
			var fieldValue = fieldInfo?.GetValue(parent);

			isShow = IsMeetCondition(fieldValue, compareType, compareValue);
			if (!isShow) return 0;
			float height = base.GetPropertyHeight(property, label);
			height = CaculateHeight(property,height);
			return height;
		}

		private float CaculateHeight(SerializedProperty property,float height)
		{
			if(property.isExpanded)
			{
				foreach (SerializedProperty child in property) 
				{
					height += 15;
					CaculateHeight(child,height);
				}
			}
			return height;
		}
		
		/// <summary>
		/// 符合条件
		/// </summary>
		private bool IsMeetCondition(object fieldValue, CompareType compareType, object compareValue) {
			if (compareType == CompareType.Equal) {
				return fieldValue.Equals(compareValue);
			}
			else if (compareType == CompareType.NonEqual) {
				return !fieldValue.Equals(compareValue);
			}
			else if (compareType == CompareType.Contains)
			{
				return ((Int32)fieldValue & (Int32)compareValue)!= 0;
			}
			else if (IsValueType(fieldValue.GetType()) && IsValueType(compareValue.GetType())) {
				switch (compareType) {
					case CompareType.Less:
						return Comparer.DefaultInvariant.Compare(fieldValue, compareValue) < 0;
					case CompareType.LessEqual:
						return Comparer.DefaultInvariant.Compare(fieldValue, compareValue) <= 0;
					case CompareType.More:
						return Comparer.DefaultInvariant.Compare(fieldValue, compareValue) > 0;
					case CompareType.MoreEqual:
						return Comparer.DefaultInvariant.Compare(fieldValue, compareValue) >= 0;
				}
			}
			return false;
		}

		/// <summary>
		/// 是否是值类型
		/// </summary>
		private bool IsValueType(Type type) {
			//IsPrimitive就是系统自带的类，IsValueType就是值类型，再排除char剩下的就是int，float这些了
			return (type.IsPrimitive && type.IsValueType && type != typeof(char));
		}

		/// <summary>
		/// 绘制
		/// </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			if (isShow)
			{
				EditorGUI.PropertyField(position, property, label, true);
			}
		}
	}
}
