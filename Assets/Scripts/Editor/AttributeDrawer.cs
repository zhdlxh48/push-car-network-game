using UnityEditor;
using UnityEngine;
using PushCar.Attributes;

namespace PushCar.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute), true)]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool disabled = true;
            switch (((ReadOnlyAttribute)attribute).runtimeOnly)
            {
                case ReadOnlyType.FULLY_DISABLED:
                    disabled = true;
                    break;

                case ReadOnlyType.EDITABLE_RUNTIME:
                    disabled = !Application.isPlaying;
                    break;

                case ReadOnlyType.EDITABLE_EDITOR:
                    disabled = Application.isPlaying;
                    break;
            }

            using (var scope = new EditorGUI.DisabledGroupScope(disabled))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }

    [UnityEditor.CustomPropertyDrawer(typeof(BeginReadOnlyAttribute))]
    public class BeginReadOnlyGroupDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return 0;
        }

        public override void OnGUI(Rect position)
        {
            EditorGUI.BeginDisabledGroup(true);
        }
    }

    [UnityEditor.CustomPropertyDrawer(typeof(EndReadOnlyAttribute))]
    public class EndReadOnlyGroupDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return 0;
        }

        public override void OnGUI(Rect position)
        {
            EditorGUI.EndDisabledGroup();
        }
    }
}