using System;
using System.Collections.Generic;

namespace EnumsEditor
{
	[Serializable]
	public partial class EnumEditor<TEnum> where TEnum : Enum
	{
		private EnumEditorIO<TEnum> _inputOutput = new EnumEditorIO<TEnum>();

		public void AddEnumEntry(string newEnumName)
		{
			if (_inputOutput.LoadEnumsFromFile(out List<string> parsedEnumRaw))
			{
				ParsedEnum parsedEnum = new ParsedEnum(parsedEnumRaw);
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
				ParsedEnum parsedEnum = new ParsedEnum(parsedEnumRaw);
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
				ParsedEnum parsedEnum = new ParsedEnum(parsedEnumRaw);
				parsedEnum.Validate();
				parsedEnum.RemoveEntry(entry.ToString());
				parsedEnum.Validate();
				_inputOutput.WriteEnumsToFile(parsedEnum.ToRaw());
			}
		}
	}
}
