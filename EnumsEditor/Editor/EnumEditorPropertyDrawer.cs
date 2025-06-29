using System;
using UnityEditor;
using UnityEngine;

namespace EnumsEditor.Editor
{
	[CustomPropertyDrawer(typeof(EnumEditor<>), true)]
	public class EnumEditorPropertyDrawer : PropertyDrawer
	{
		private const string AUTO_FIND_ENUM_FILE_FIELD_NAME = "_autoFindEnumFile";
		private const string MANUAL_ENUM_FILE_PATH_FIELD_NAME = "_manualEnumFilePath";
		private const string INPUT_OUTPUT_FIELD_NAME = "_inputOutput";

		private object _enumEditorObject;

		private string _entryName;
		private Enum _selectedRemoveEnum;
        private bool _mainTogglePersistentName;

		private bool AutoFindEnumFileFiledValue
		{
			get
			{
				object inputOutputValue = _enumEditorObject.GetFieldValue(INPUT_OUTPUT_FIELD_NAME);
				return (bool)inputOutputValue.GetFieldValue(AUTO_FIND_ENUM_FILE_FIELD_NAME);
			}
			set
			{
				object inputOutputValue = _enumEditorObject.GetFieldValue(INPUT_OUTPUT_FIELD_NAME);
				inputOutputValue.SetFieldValue(AUTO_FIND_ENUM_FILE_FIELD_NAME, value);
			}
		}

		private string ManualEnumFilePathValue
		{
			get
			{
				object inputOutputValue = _enumEditorObject.GetFieldValue(INPUT_OUTPUT_FIELD_NAME);
				return (string)inputOutputValue.GetFieldValue(MANUAL_ENUM_FILE_PATH_FIELD_NAME);
			}
			set
			{
				object inputOutputValue = _enumEditorObject.GetFieldValue(INPUT_OUTPUT_FIELD_NAME);
				inputOutputValue.SetFieldValue(MANUAL_ENUM_FILE_PATH_FIELD_NAME, value);
			}
		}

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var mainFoldToggle = EditorGUILayout.BeginFoldoutHeaderGroup(_mainTogglePersistentName, label);
            _mainTogglePersistentName = mainFoldToggle;
			EditorGUILayout.EndFoldoutHeaderGroup();
			var labelWidthCache = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 100f;
			if (mainFoldToggle)
			{
			    CacheEnumEditorObject(property);
			    CacheEnumType(_enumEditorObject);
                
				DrawPathGroup();
				DrawAddGroup();
				DrawRemoveGroup();
			}
			EditorGUIUtility.labelWidth = labelWidthCache;
		}

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0f;
        }

        private void DrawPathGroup()
		{
			EditorGUILayout.BeginVertical(EditorStyles.BoxContainer);
			EditorGUILayout.LabelField("File path", EditorStyles.BoxHeader);

			bool autoFindEnumFileFiledValue = EditorGUILayout.Toggle("Auto find", AutoFindEnumFileFiledValue);
			AutoFindEnumFileFiledValue = autoFindEnumFileFiledValue;
			if (!autoFindEnumFileFiledValue)
			{
				EditorGUILayout.HelpBox("Click on cs file with RMB -> Copy Path", MessageType.Info);
				ManualEnumFilePathValue = EditorGUILayout.TextField("Manual Path", ManualEnumFilePathValue);
			}
			EditorGUILayout.EndVertical();
		}

		private void DrawAddGroup()
		{
			EditorGUILayout.BeginVertical(EditorStyles.BoxContainer);
			EditorGUILayout.LabelField("Add", EditorStyles.BoxHeader);

			EditorGUILayout.BeginHorizontal();

			bool checkIfAddButtonNeedToBeDisabled = CheckIfAddButtonNeedToBeDisabled();

			Color guiColorCache = GUI.color;
			GUI.color = checkIfAddButtonNeedToBeDisabled ? Color.red : Color.white;
			_entryName = EditorGUILayout.TextField("Name", _entryName);
			GUI.color = guiColorCache;

			GUI.enabled = !checkIfAddButtonNeedToBeDisabled;
			if (GUILayout.Button("Add"))
			{
				var addEntryMethodName = nameof(EnumEditor<Enum>.AddEnumEntry);
				_enumEditorObject.InvokeMethod(addEntryMethodName, _entryName);
			}
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		private void DrawRemoveGroup()
		{
			EditorGUILayout.BeginVertical(EditorStyles.BoxContainer);
			EditorGUILayout.LabelField("Remove", EditorStyles.BoxHeader);

			EditorGUILayout.BeginHorizontal();

			_selectedRemoveEnum = EditorGUILayout.EnumPopup("ID", _selectedRemoveEnum);

			if (GUILayout.Button("Remove"))
			{
				var removeEntryMethodName = nameof(EnumEditor<Enum>.RemoveEnumEntry);
				_enumEditorObject.InvokeMethod(removeEntryMethodName, _selectedRemoveEnum);
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		private void CacheEnumEditorObject(SerializedProperty property)
		{
			_enumEditorObject ??= GetEnumEditorObject(property);
		}

		private void CacheEnumType(object enumEditorObject)
		{
			if (_selectedRemoveEnum == null)
			{
				Type enumType = GetEnumType(enumEditorObject);
				_selectedRemoveEnum = (Enum)Enum.GetValues(enumType).GetValue(0);
			}
		}

		private object GetEnumEditorObject(SerializedProperty property)
		{
			var objectWithEnumEditorField = property.serializedObject.targetObject;
			var enumEditorField = objectWithEnumEditorField.GetFieldValue(property.name);
			return enumEditorField;
		}

		private Type GetEnumType(object enumEditorObject)
		{
			if (enumEditorObject == null)
				return null;

			var genericType = enumEditorObject.GetType();
			if (genericType.IsGenericType && genericType.GetGenericTypeDefinition() == typeof(EnumEditor<>))
			{
				Type enumType = genericType.GetGenericArguments()[0];
				return enumType;
			}

			var genericTypeBase = enumEditorObject.GetType().BaseType;
			if (genericTypeBase != null && genericTypeBase.IsGenericType && genericTypeBase.GetGenericTypeDefinition() == typeof(EnumEditor<>))
			{
				Type enumType = genericTypeBase.GetGenericArguments()[0];
				return enumType;
			}

			return null;
		}

		private bool CheckIfAddButtonNeedToBeDisabled()
		{
			bool addValueExists = !string.IsNullOrEmpty(_entryName);
			return addValueExists == false || _entryName.IsValidCSharpOperatorName() == false;
		}
	}
}
