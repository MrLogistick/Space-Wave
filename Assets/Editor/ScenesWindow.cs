using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public class ScenesWindow : EditorWindow
{
    [MenuItem("Window/Scenes Tab")]
    public static void ShowWindow()
    {
        GetWindow<ScenesWindow>("Scenes");
    }

    Vector2 scrollPos;

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });

        foreach (string guid in sceneGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string name = Path.GetFileNameWithoutExtension(path);

            if (GUILayout.Button(name))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(path);
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }
}