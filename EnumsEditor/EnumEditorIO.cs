using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EnumsEditor
{
	public partial class EnumEditor<TEnum> where TEnum : Enum
	{
		[Serializable]
		private class EnumEditorIO<TEnum1>
		{
			[SerializeField] private bool _autoFindEnumFile = true;
			[SerializeField] private string _manualEnumFilePath;

			public void WriteEnumsToFile(List<string> parsedEnumRaw)
			{
				if (TryGetFilePath(out string path) == false)
					return;

				string enumName = typeof(TEnum1).Name;
				using (StreamWriter streamWriter = new(path))
				{
					streamWriter.WriteLine("public enum " + enumName);
					streamWriter.WriteLine("{");
					for (int i = 0; i < parsedEnumRaw.Count; i++)
						streamWriter.WriteLine("\t" + parsedEnumRaw[i] + ",");
					streamWriter.WriteLine("}");
				}
				AssetDatabase.Refresh();
			}

			public bool LoadEnumsFromFile(out List<string> parsedEnumRaw)
			{
				if (TryGetFilePath(out string path) == false)
				{
					parsedEnumRaw = null;
					return false;
				}

				using (StreamReader streamReader = new(path))
				{
					string loadedString = streamReader.ReadToEnd();
					int openingBraceIndex = loadedString.IndexOf('{');
					int closingBraceIndex = loadedString.IndexOf('}');
					loadedString = loadedString.Substring(openingBraceIndex + 1, closingBraceIndex - openingBraceIndex - 1).Trim();
					IEnumerable<string> entries = loadedString.Split(',').Select(entry => entry.Trim()).Where(entry => !string.IsNullOrEmpty(entry));
					List<string> entriesList = entries.ToList();
					parsedEnumRaw = entriesList;
					return true;
				}
			}

			private bool TryGetFilePath(out string path)
			{
				if (_autoFindEnumFile)
					return TryGetEnumFilePathByGenericType(out path);

				if (ValidateManualPath())
				{
					path = _manualEnumFilePath;
					return true;
				}

				path = _manualEnumFilePath;
				return false;
			}

			private bool TryGetEnumFilePathByGenericType(out string path)
			{
				string enumName = GetEnumFileName();
				string searchFilter = $"glob:\"Assets/**/*{enumName}\"";
				string[] guids = AssetDatabase.FindAssets(searchFilter);

				if (guids.Length == 0)
				{
					EditorUtility.DisplayDialog("Enum Editor Message", "Enum file not found. " +
					                                                   $"\n\nSearch filter: {searchFilter}", "Close");
					path = null;
					return false;
				}

				if (guids.Length > 1)
				{
					string[] paths = guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();
					string pathWithSameName = paths.FirstOrDefault(path => Path.GetFileName(path) == enumName);

					if (pathWithSameName != null)
					{
						path = AssetDatabase.GUIDToAssetPath(guids[0]);
						return true;
					}

					EditorUtility.DisplayDialog("Enum Editor Message", $"Found more than one enum file:" +
					                                                   $"\n\nSearch filter: {searchFilter}" +
					                                                   $"\n\n{paths.ListToString()}", "Close");
					path = null;
					return false;
				}

				path = AssetDatabase.GUIDToAssetPath(guids[0]);
				return true;
			}

			private bool ValidateAutoFindPath() => TryGetEnumFilePathByGenericType(out _);

			private bool ValidateManualPath() => !string.IsNullOrEmpty(_manualEnumFilePath);

			private string GetEnumFileName() => typeof(TEnum1).Name + ".cs";
		}
	}
}
