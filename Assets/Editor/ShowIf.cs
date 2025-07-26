using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ShowIf : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var obj = target;
        var fields = obj.GetType()
                        .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        
        foreach (var field in fields) {
            bool shouldShow = true;
            var showIf = field.GetCustomAttribute<ShowIfAttribute>();

            var hide = field.GetCustomAttribute<HideInInspector>();
            if (hide != null) continue; // skip drawing this field

            if (showIf != null) { // if the attribute showIf is not null, get the value of the bool field and set shouldShow accordingly
                string conditionName = showIf.ConditionFieldName;
                bool negate = false;

                if (conditionName.StartsWith("!")) {
                    negate = true;
                    conditionName = conditionName.Substring(1);
                }

                var conditionField = obj.GetType().GetField(conditionName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (conditionField != null && conditionField.FieldType == typeof(bool)) {
                    bool conditionValue = (bool)conditionField.GetValue(obj);
                    shouldShow = negate ? !conditionValue : conditionValue;
                } else {
                    Debug.LogWarning($"ShowIf: Could not find bool field '{conditionName}' in {obj.name}");
                }
            }

            if (shouldShow) { // finds the field that will be affected and draws it
                SerializedProperty prop = serializedObject.FindProperty(field.Name);
                if (prop != null) {
                    EditorGUILayout.PropertyField(prop, true);
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}