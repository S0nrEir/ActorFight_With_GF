using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Aquila.AbilityEditor;
using System;

namespace Editor.AbilityEditor.Inspector
{
    /// <summary>
    /// SerializedTrackData 的自定义 PropertyDrawer
    /// 解决 [SerializeReference] 字段手动添加时类型信息丢失的问题
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializedTrackData))]
    public class SerializedTrackDataDrawer : PropertyDrawer
    {
        private ReorderableList _clipsList;
        private SerializedProperty _currentProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // 绘制折叠标题
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded,
                label,
                true
            );

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                float yOffset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                // 绘制 TrackName
                var trackNameProp = property.FindPropertyRelative("TrackName");
                if (trackNameProp != null)
                {
                    EditorGUI.PropertyField(
                        new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight),
                        trackNameProp
                    );
                    yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // 绘制 TrackColor
                var trackColorProp = property.FindPropertyRelative("TrackColor");
                if (trackColorProp != null)
                {
                    EditorGUI.PropertyField(
                        new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight),
                        trackColorProp
                    );
                    yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // 绘制 IsEnabled
                var isEnabledProp = property.FindPropertyRelative("IsEnabled");
                if (isEnabledProp != null)
                {
                    EditorGUI.PropertyField(
                        new Rect(position.x, position.y + yOffset, position.width, EditorGUIUtility.singleLineHeight),
                        isEnabledProp
                    );
                    yOffset += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }

                // 绘制 Clips 列表（使用自定义绘制）
                var clipsProp = property.FindPropertyRelative("Clips");
                if (clipsProp != null)
                {
                    DrawClipsList(new Rect(position.x, position.y + yOffset, position.width, 0), clipsProp);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        private void DrawClipsList(Rect position, SerializedProperty clipsProp)
        {
            // 初始化 ReorderableList（如果需要）
            if (_clipsList == null || _currentProperty != clipsProp)
            {
                _currentProperty = clipsProp;
                _clipsList = new ReorderableList(clipsProp.serializedObject, clipsProp, true, true, true, true);

                _clipsList.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Clips");
                };

                _clipsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = clipsProp.GetArrayElementAtIndex(index);
                    if (element != null)
                    {
                        EditorGUI.PropertyField(rect, element, GUIContent.none, true);
                    }
                };

                _clipsList.elementHeightCallback = (int index) =>
                {
                    var element = clipsProp.GetArrayElementAtIndex(index);
                    return element != null ? EditorGUI.GetPropertyHeight(element, true) + 4 : EditorGUIUtility.singleLineHeight;
                };

                _clipsList.onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Effect Clip"), false, () => AddClip(clipsProp, typeof(EffectClipData)));
                    menu.AddItem(new GUIContent("Audio Clip"), false, () => AddClip(clipsProp, typeof(AudioClipData)));
                    menu.AddItem(new GUIContent("VFX Clip"), false, () => AddClip(clipsProp, typeof(VFXClipData)));
                    menu.ShowAsContext();
                };
            }

            _clipsList.DoList(position);
        }

        private void AddClip(SerializedProperty clipsProp, Type clipType)
        {
            clipsProp.serializedObject.Update();

            int newIndex = clipsProp.arraySize;
            clipsProp.InsertArrayElementAtIndex(newIndex);

            var newElement = clipsProp.GetArrayElementAtIndex(newIndex);
            newElement.managedReferenceValue = Activator.CreateInstance(clipType);

            clipsProp.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // TrackName
            var trackNameProp = property.FindPropertyRelative("TrackName");
            if (trackNameProp != null)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // TrackColor
            var trackColorProp = property.FindPropertyRelative("TrackColor");
            if (trackColorProp != null)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // IsEnabled
            var isEnabledProp = property.FindPropertyRelative("IsEnabled");
            if (isEnabledProp != null)
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Clips
            var clipsProp = property.FindPropertyRelative("Clips");
            if (clipsProp != null && _clipsList != null)
            {
                height += _clipsList.GetHeight();
            }

            return height;
        }
    }
}
