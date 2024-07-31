using UnityEngine;
using UnityEngine;
namespace InspectorEx
{
	public enum CompareType
	{
		Equal,
		NonEqual,
		Less,
		LessEqual,
		More,
		MoreEqual,
		Contains,
	}

	/// <summary>
	/// 特性：条件判断Inspector窗口属性的显隐
	/// </summary>
	public class PropertyActiveAttribute : PropertyAttribute
	{
		public string field;
		public CompareType compareType;
		public object compareValue;

		public PropertyActiveAttribute(string fieldName, CompareType type, object value) {
			field = fieldName;
			compareType = type;
			compareValue = value;
		}
	}
}
