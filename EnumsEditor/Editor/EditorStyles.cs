using UnityEngine;
using UnityEditor;

namespace EnumsEditor.Editor
{
	public static class EditorStyles
	{
		private static GUIStyle _boxContainer;
		private static GUIStyle _boxHeader;

		private static Texture2D _boxHeaderTexture2D;

		public static readonly Color HeaderBoxBackgroundColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.06f) : new Color(1f, 1f, 1f, 0.26f);

		public static Texture2D BoxHeaderTexture2D
		{
			get
			{
				if (!_boxHeaderTexture2D)
					_boxHeaderTexture2D = CreateTexture2D(2, 2, HeaderBoxBackgroundColor);
				return _boxHeaderTexture2D;
			}
		}

		public static GUIStyle BoxContainer
		{
			get
			{
				if (_boxContainer == null)
				{
					_boxContainer = new GUIStyle(UnityEditor.EditorStyles.helpBox);
					_boxContainer.margin = new RectOffset(0, 0, 0, 2);
				}
				return _boxContainer;
			}
		}

		public static GUIStyle BoxHeader
		{
			get
			{
				if (_boxHeader == null)
				{
					_boxHeader = new GUIStyle(UnityEditor.EditorStyles.label);
					_boxHeader.margin = new RectOffset(0, 0, 0, 2);
					_boxHeader.padding = new RectOffset(0, 5, 3, 3);
					_boxHeader.fontSize = 12;
					_boxHeader.fontStyle = FontStyle.Bold;

					_boxHeader.normal.background = BoxHeaderTexture2D;

					_boxHeader.onNormal.background = _boxHeader.normal.background;
					_boxHeader.hover.background = _boxHeader.normal.background;
					_boxHeader.onHover.background = _boxHeader.normal.background;
					_boxHeader.active.background = _boxHeader.normal.background;
					_boxHeader.onActive.background = _boxHeader.normal.background;
				}
				return _boxHeader;
			}
		}

		public static Texture2D CreateTexture2D(int width, int height, Color fillColor)
		{
			Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Point;

			Color[] pixels = new Color[width * height];
			System.Array.Fill(pixels, fillColor);

			texture.SetPixels(pixels);
			texture.Apply();
			return texture;
		}
	}
}
