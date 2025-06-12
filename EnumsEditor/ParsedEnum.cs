using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumsEditor
{
#if UNITY_EDITOR
	public partial class EnumEditor<TEnum> where TEnum : Enum
	{
		private class ParsedEnum
		{
			private const string NONE_ENTRY_NAME = "None";

			private List<ParsedEnumEntry> _entries;

			public ParsedEnum(List<string> parsedEnumRaw)
			{
				_entries = parsedEnumRaw.Select(entry => new ParsedEnumEntry(entry)).ToList();
			}

			public void AddEntry(string name)
			{
				if (IsNameValid(name))
				{
					_entries.Add(new ParsedEnumEntry(name, GetMaxID() + 1));
				}
			}

			public void AddEntry(string name, int id, bool forceID = false)
			{
				if (IsNameValid(name))
				{
					var existingEntryWithID = _entries.Find(e => e.ID == id);
					if (existingEntryWithID == null || forceID)
						_entries.Add(new ParsedEnumEntry(name, id));
				}
			}

			public void RemoveEntry(string name)
			{
				_entries.RemoveAll(entry => entry.Name == name);
			}

			public void Validate()
			{
				ValidateEnumsDuplicates();
				ValidateNoneEntry();
				ValidateEnumsIDs();
				SortByID();
			}

			public List<string> ToRaw()
			{
				return _entries.Select(entry => entry.ToString()).ToList();
			}

			private void ValidateNoneEntry()
			{
				if (_entries.Any(entry => entry.ID == 0))
					return;

				for (int i = 0; i < _entries.Count; i++)
				{
					if (string.Equals(_entries[i].Name, NONE_ENTRY_NAME, StringComparison.CurrentCultureIgnoreCase))
					{
						if (_entries[i].ID == null)
							_entries[i].SetID(0);
						return;
					}
				}

				_entries.Add(new ParsedEnumEntry(NONE_ENTRY_NAME, 0));
			}

			private void ValidateEnumsIDs()
			{
				int maxID = GetMaxID();
				int currentID = maxID + 1;
				foreach (ParsedEnumEntry parsedEnum in _entries.Where(parsedEnum => parsedEnum.ID == null))
				{
					parsedEnum.SetID(currentID);
					currentID++;
				}
			}

			private void SortByID()
			{
				_entries.Sort((a, b) =>
				{
					if (a.ID == null || b.ID == null)
						return 0;
					return a.ID.Value.CompareTo(b.ID.Value);
				});
			}

			private int GetMaxID()
			{
				int maxID = _entries.Max(entry => entry.ID ?? 0);
				return maxID;
			}

			private void ValidateEnumsDuplicates()
			{
				_entries = _entries.Distinct(new ParsedEnumEntry.OnlyNamesComparer()).ToList();
			}

			private bool IsNameValid(string name)
			{
				return string.IsNullOrEmpty(name) == false && name.IsValidCSharpOperatorName();
			}

			private class ParsedEnumEntry
			{
				private readonly string _name;
				private int? _id;

				public string Name => _name;
				public int? ID => _id;

				public ParsedEnumEntry(string enumAndId)
				{
					List<string> split = enumAndId.Split("=").Select(item => item.Trim()).ToList();
					if (split.Count == 1)
					{
						_name = split[0];
						_id = null;
					}
					else if (split.Count == 2)
					{
						_name = split[0];
						_id = Convert.ToInt32(split[1]);
					}
					else
					{
						UnityEngine.Debug.LogError("Failed to parse enum string. Ensure the input matches the expected format.");
					}
				}

				public ParsedEnumEntry(string name, int id)
				{
					_name = name;
					_id = id;
				}

				public void SetID(int id)
				{
					_id = id;
				}

				public override string ToString()
				{
					return _id == null ? _name : $"{_name} = {_id}";
				}

				internal class OnlyNamesComparer : IEqualityComparer<ParsedEnumEntry>
				{
					public bool Equals(ParsedEnumEntry x, ParsedEnumEntry y)
					{
						if (ReferenceEquals(x, y))
							return true;
						if (ReferenceEquals(x, null))
							return false;
						if (ReferenceEquals(y, null))
							return false;
						if (x.GetType() != y.GetType())
							return false;
						return x._name == y._name;
					}

					public int GetHashCode(ParsedEnumEntry obj)
					{
						return obj._name != null ? obj._name.GetHashCode() : 0;
					}
				}
			}
		}
	}
#endif
}
