using UnityEditor;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;

namespace oezyowen.Editor
{
    public class ToolsMenu
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            Dir("", "_Third Party", "Audio", "Prefabs", "Scripts", "Sprites", "Scenes");
            Refresh();
        }

        public static void Dir(string root, params string[] dir)
        {
            var fullPath = Combine(dataPath, root);
            foreach(var newDirectory  in dir)
            {
                CreateDirectory(Combine(fullPath, newDirectory));
            }
        }
    }
}
