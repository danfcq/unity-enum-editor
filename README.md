# EnumEditor for Unity

[![Unity](https://img.shields.io/badge/Unity-2021+-black.svg)](https://unity3d.com/pt/get-unity/download/archive)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

![EnumEditor Screenshot](README_RESOURCES/EnumEditor_screenshot.png)

## 🔍️ Overview
- A simple and user-friendly Enum Editor tool designed for Unity.
- Parses and directly modifies `.cs` files containing enums.
- Built with non-programmers in mind, so editing code is no longer required.

## 🛠️ Main Features
- **Easily Add or Remove Enum Entries**: Manage entries with awareness of constant values to keep enums consistent. 
- **Flexible File Path Handling**: Auto-detect enum files or specify the path manually.

## ⚙️ Installation 

Copy or move the `EnumsEditor` folder into your Unity project's `Assets` folder:
   ```
   Your_Unity_Project/Assets/EnumsEditor
   ```

That's it! You can now start using the Unity Enum Editor to manage your enums. 🎉

## 📝 Usage 
You can easily use the Unity Enum Editor by adding it as a serialized field in your classes wherever you need it.

```csharp MonoBehaviour
public class Demo : MonoBehaviour 
{
    // In MonoBehaviour
    [SerializeField] private EnumEditor<eSound> _soundsEnumEditor;
}
```

```csharp ScriptableObject
[CreateAssetMenu(fileName = "SoundsConfig", menuName = "Configs/Sounds")]
public class SoundsConfig : ScriptableObject
{
    // In ScriptableObject
    [SerializeField] private EnumEditor<eSound> _soundsEnumEditor;
}
```

## Limitations

- Currently, `EnumEditor` does not support constant value types apart from the default `int`.

---

## 📜 License 
This project is licensed under the terms of the [LICENSE](LICENSE) file.

---

Feel free to reach out with any feedback or suggestions! 💬