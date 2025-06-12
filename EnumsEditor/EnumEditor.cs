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
		private const string ODIN_ADD_GROUP_ID = "Add";
		private const string ODIN_ADD_SUBGROUP_ID = ODIN_ADD_GROUP_ID + "/Horizontal";
		private const string ODIN_REMOVE_GROUP_ID = "Remove";
		private const string ODIN_REMOVE_SUBGROUP_ID = ODIN_REMOVE_GROUP_ID + "/Horizontal";

		[InlineProperty, HideLabel]
		[SerializeField]
		private EnumEditorIO<TEnum> _inputOutput = new();

		[BoxGroup(ODIN_ADD_GROUP_ID)]
		[HorizontalGroup(ODIN_ADD_SUBGROUP_ID)]
		[SerializeField, LabelText("Name"), LabelWidth(50)]
		private string _addEntryName;

		[HorizontalGroup(ODIN_ADD_SUBGROUP_ID)]
		[Button(ODIN_ADD_GROUP_ID), DisableIf(nameof(CheckIfAddButtonNeedToBeDisabled))]
		private void AddEntry()
		{
			AddEntry(_addEntryName);
			_addEntryName = string.Empty;
		}

		private bool CheckIfAddButtonNeedToBeDisabled()
		{
			bool addValueExists = !string.IsNullOrEmpty(_addEntryName);
			return addValueExists == false || _addEntryName.IsValidCSharpOperatorName() == false;
		}

		[BoxGroup(ODIN_REMOVE_GROUP_ID)]
		[HorizontalGroup(ODIN_REMOVE_SUBGROUP_ID)]
		[SerializeField, LabelText("ID"), LabelWidth(50)]
		private TEnum _removeEntryType;

		[HorizontalGroup(ODIN_REMOVE_SUBGROUP_ID)]
		[Button(ODIN_REMOVE_GROUP_ID)]
		private void RemoveEntry()
		{
			RemoveEntry(_removeEntryType);
			_removeEntryType = default;
		}

		public void AddEntry(string newEnumName)
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

		public void AddEntry(string newEnumName, int id, bool forceID = false)
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

		public void RemoveEntry(TEnum entry)
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
