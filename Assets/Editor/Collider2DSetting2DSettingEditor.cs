using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Collider2DSetting2DSettingEditor : Editor
{
    [MenuItem("Tools/Collider2DSetting")]
    public static void Run()
    {
        var files = Directory.GetFiles("Assets/Resources/Colliders","*.prefab");
        List<string> list = new List<string>();
        foreach(var file in files)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            var gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(file);
            int.TryParse(name, out var id);
            list.Add( gameObject.GetComponent<Collider2DSetting>().CreateConfig(id));
        }
        var content = "id\tisEnableColliderDetection\tisCollisionDestory\tcolliderLayer\tcolliderShape\toffset\tradius\tlayer\tsize\n";
        content += string.Join("\n",list);
        File.WriteAllText("Assets/Resources/config/CollisionConfig.csv", content);
        AssetDatabase.Refresh();
    }
}