using System;
using System.Linq;
using System.Reflection;

namespace EnumsEditor.Editor
{
	public static class EditorUtilities
	{
		public static void SetFieldValue(this object obj, string name, object value)
		{
			const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			FieldInfo field = obj.GetType().GetField(name, bindingFlags);
			field?.SetValue(obj, value);
		}

		public static object GetFieldValue(this object obj, string name)
		{
			const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			Type type = obj.GetType();
			FieldInfo field = GetFieldIncludingBaseTypes(type, name, bindingFlags);
			return field?.GetValue(obj);
		}

		public static object InvokeMethod(this object obj, string methodName, params object[] args)
		{
			Type type = obj.GetType();
			MethodInfo method;
			if (args == null || args.Length == 0)
				method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			else
				method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, args.Select(a => a.GetType()).ToArray(), null);
			return method == null ? null : method.Invoke(obj, args);
		}

		public static FieldInfo GetFieldIncludingBaseTypes(Type type, string fieldName, BindingFlags flags)
		{
			while (type != null)
			{
				FieldInfo field = type.GetField(fieldName, flags);
				if (field != null)
					return field;

				type = type.BaseType;
			}
			return null;
		}
	}
}
