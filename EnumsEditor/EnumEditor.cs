using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnumsEditor
{
	[Serializable]
	public partial class EnumEditor<TEnum> where TEnum : Enum
	{
#if UNITY_EDITOR
		[InlineProperty, HideLabel]
		[SerializeField] private EnumEditorIO<TEnum> _inputOutput = new();

		[BoxGroup("Add")]
		[HorizontalGroup("Add/Horizontal")]
		[SerializeField, LabelText("Name"), LabelWidth(50)] private string _addEntryName;

		[HorizontalGroup("Add/Horizontal")]
		[Button("Add"), DisableIf(nameof(CheckIfAddButtonNeedToBeDisabled))] private void AddEnumEntry()
		{
			AddEnumEntry(_addEntryName);
			_addEntryName = string.Empty;
		}

		private bool CheckIfAddButtonNeedToBeDisabled()
		{
			bool addValueExists = !string.IsNullOrEmpty(_addEntryName);
			return addValueExists == false || _addEntryName.IsValidCSharpOperatorName() == false;
		}

		[BoxGroup("Remove")]
		[HorizontalGroup("Remove/Horizontal")]
		[SerializeField, LabelText("ID"), LabelWidth(50)] private TEnum _removeEntryType;

		[HorizontalGroup("Remove/Horizontal")]
		[Button("Remove")] private void RemoveEnumEntry()
		{
			RemoveEnumEntry(_removeEntryType);
			_removeEntryType = default;
		}

		public void AddEnumEntry(string newEnumName)
		{
			if (_inputOutput.LoadEnumsFromFile(out List<string> parsedEnumRaw))
			{
				ParsedEnum parsedEnum = new(parsedEnumRaw);
				parsedEnum.Validate();
				parsedEnum.AddEntry(newEnumName);
				parsedEnum.Validate();
				_inputOutput.WriteEnumsToFile(parsedEnum.ToRaw());
			}
		}

		public void AddEnumEntry(string newEnumName, int id, bool forceID = false)
		{
			if (_inputOutput.LoadEnumsFromFile(out List<string> parsedEnumRaw))
			{
				ParsedEnum parsedEnum = new(parsedEnumRaw);
				parsedEnum.Validate();
				parsedEnum.AddEntry(newEnumName, id, forceID);
				parsedEnum.Validate();
				_inputOutput.WriteEnumsToFile(parsedEnum.ToRaw());
			}
		}

		public void RemoveEnumEntry(TEnum entry)
		{
			if (_inputOutput.LoadEnumsFromFile(out List<string> parsedEnumRaw))
			{
				ParsedEnum parsedEnum = new(parsedEnumRaw);
				parsedEnum.Validate();
				parsedEnum.RemoveEntry(entry.ToString());
				parsedEnum.Validate();
				_inputOutput.WriteEnumsToFile(parsedEnum.ToRaw());
			}
		}
#endif
	}
}
