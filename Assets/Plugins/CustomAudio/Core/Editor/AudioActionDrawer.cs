using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace AudioSystem.Edit
{
    [CustomPropertyDrawer(typeof(AudioAction))]
    public class AudioActionDrawer : PropertyDrawer
    {
        private const int spacing = 20;
        private const string clipLibraryPath = "Assets/Resources/Audio/AudioAsset.asset";
        private const string emptyString = "----------";
        private ClipAssetLibrary clipAssetLibrary;

        private SerializedProperty type;
        private SerializedProperty bgmActionType;
        private SerializedProperty soundActionType;

        private SerializedProperty loop;
        private SerializedProperty clipReferenceType;
        private SerializedProperty groupName;
        private SerializedProperty clipName;
        private SerializedProperty clip;
        private SerializedProperty soundMode;

        private SerializedProperty bgmResumeFadeIn;
        private SerializedProperty fadeDuration;

        private SerializedProperty rndPitch;
        private SerializedProperty rndRange;

        private bool initlized = false;

        private Rect backgroundRect;
        private Rect currentPosition;
        private Color rectColor = new Color(0.25f, 0.25f, 0.25f, 1);

        private void Initlize(SerializedProperty property)
        {
            if (initlized)
                return;

            LoadClipAssetLibrary();

            type = property.FindPropertyRelative("type");
            bgmActionType = property.FindPropertyRelative("bgmActionType");
            soundActionType = property.FindPropertyRelative("soundActionType");

            loop = property.FindPropertyRelative("loop");
            clipReferenceType = property.FindPropertyRelative("clipReferenceType");
            groupName = property.FindPropertyRelative("groupName");
            clipName = property.FindPropertyRelative("clipName");
            clip = property.FindPropertyRelative("clip");
            soundMode = property.FindPropertyRelative("soundMode");

            bgmResumeFadeIn = property.FindPropertyRelative("bgmResumeFadeIn");
            fadeDuration = property.FindPropertyRelative("fadeDuration");

            rndPitch = property.FindPropertyRelative("rndPitch");
            rndRange = property.FindPropertyRelative("rndRange");

            initlized = true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initlize(property);

            position.y += spacing * 0.5f;

            backgroundRect = new Rect(
                position.position + new Vector2(-4, 0),
                position.size + new Vector2(8, -10)
            );
            EditorGUI.DrawRect(backgroundRect, rectColor);

            currentPosition = position;
            currentPosition.height = spacing;
            currentPosition.y += 4;
            DrawProperty(currentPosition, type, "Type");

            currentPosition.y += spacing;
            switch ((AudioSourceType)type.enumValueIndex)
            {
                case AudioSourceType.BGM:
                    DrawProperty(currentPosition, bgmActionType, "Action");

                    switch ((BgmActionType)bgmActionType.enumValueIndex)
                    {
                        case BgmActionType.Play:
                            currentPosition.y += spacing * 1.5f;
                            DrawProperty(currentPosition, clipReferenceType, "Reference Mode");
                            DrawClipPropertyField();
                            currentPosition.y += spacing * 1.5f;
                            DrawProperty(currentPosition, loop, "Loop");
                            break;
                        case BgmActionType.Pause:
                            break;
                        case BgmActionType.Resume:
                            currentPosition.y += spacing * 1.5f;
                            DrawProperty(currentPosition, bgmResumeFadeIn, "Fade In");
                            if (bgmResumeFadeIn.boolValue)
                            {
                                currentPosition.y += spacing;
                                DrawProperty(currentPosition, fadeDuration, "Fade Duration");
                            }
                            else
                                fadeDuration.floatValue = 1;
                            break;
                        case BgmActionType.Stop:
                            break;
                        case BgmActionType.FadeIn:
                            currentPosition.y += spacing * 1.5f;
                            DrawProperty(currentPosition, fadeDuration, "Fade Duration");
                            break;
                        case BgmActionType.FadeOut:
                            currentPosition.y += spacing * 1.5f;
                            DrawProperty(currentPosition, fadeDuration, "Fade Duration");
                            break;
                    }
                    break;
                case AudioSourceType.Sound:
                    DrawProperty(currentPosition, soundActionType, "Action");

                    switch ((SoundActionType)soundActionType.enumValueIndex)
                    {
                        case SoundActionType.Play:
                            currentPosition.y += spacing * 1.5f;
                            DrawProperty(currentPosition, clipReferenceType, "Reference Mode");
                            DrawClipPropertyField();
                            currentPosition.y += spacing * 1.5f;
                            DrawProperty(currentPosition, loop, "Loop");
                            currentPosition.y += spacing;
                            DrawProperty(currentPosition, soundMode, "Play Mode");
                            currentPosition.y += spacing;
                            DrawProperty(currentPosition, rndPitch, "Random Pitch");
                            currentPosition.y += spacing;
                            DrawProperty(currentPosition, rndRange, "Random Range");
                            break;
                        case SoundActionType.Stop:
                            currentPosition.y += spacing * 1.5f;
                            DrawProperty(currentPosition, clipName, "Clip Name");
                            break;
                        case SoundActionType.AttchSoundOnBGM:
                            currentPosition.y += spacing * 1.5f;
                            DrawProperty(currentPosition, clipReferenceType, "Reference Mode");
                            DrawClipPropertyField();
                            currentPosition.y += spacing * 1.5f;
                            DrawProperty(currentPosition, loop, "Loop");
                            currentPosition.y += spacing;
                            DrawProperty(currentPosition, soundMode, "Play Mode");
                            break;
                        case SoundActionType.StopAll:
                            break;
                    }
                    break;
            }
        }

        private int propertyHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Initlize(property);

            propertyHeight = 16 + spacing * 2;

            switch ((AudioSourceType)type.enumValueIndex)
            {
                case AudioSourceType.BGM:
                    switch ((BgmActionType)bgmActionType.enumValueIndex)
                    {
                        case BgmActionType.Play:
                            propertyHeight += spacing * 3;
                            switch ((ClipReferenceType)clipReferenceType.enumValueIndex)
                            {
                                case ClipReferenceType.File:
                                    propertyHeight += spacing;
                                    break;
                                case ClipReferenceType.Library:
                                    propertyHeight += spacing * 2;
                                    break;
                            }
                            break;
                        case BgmActionType.Pause:
                            break;
                        case BgmActionType.Resume:
                            propertyHeight += (int)(spacing * 1.5f);
                            if (bgmResumeFadeIn.boolValue)
                                propertyHeight += spacing;
                            break;
                        case BgmActionType.Stop:
                            break;
                        case BgmActionType.FadeIn:
                            propertyHeight += (int)(spacing * 1.5f);
                            break;
                        case BgmActionType.FadeOut:
                            propertyHeight += (int)(spacing * 1.5f);
                            break;
                    }
                    break;
                case AudioSourceType.Sound:
                    switch ((SoundActionType)soundActionType.enumValueIndex)
                    {
                        case SoundActionType.Play:
                            propertyHeight += spacing * 6;
                            switch ((ClipReferenceType)clipReferenceType.enumValueIndex)
                            {
                                case ClipReferenceType.File:
                                    propertyHeight += spacing;
                                    break;
                                case ClipReferenceType.Library:
                                    propertyHeight += spacing * 2;
                                    break;
                            }
                            break;
                        case SoundActionType.Stop:
                            propertyHeight += (int)(spacing * 1.5f);
                            break;
                        case SoundActionType.AttchSoundOnBGM:
                            propertyHeight += spacing * 6;
                            switch ((ClipReferenceType)clipReferenceType.enumValueIndex)
                            {
                                case ClipReferenceType.File:
                                    propertyHeight += spacing;
                                    break;
                                case ClipReferenceType.Library:
                                    propertyHeight += spacing * 2;
                                    break;
                            }
                            break;
                        case SoundActionType.StopAll:
                            break;
                    }
                    break;
            }
            return propertyHeight;
        }

        private void DrawProperty(Rect rect, SerializedProperty property, string label)
        {
            Rect contentRect = EditorGUI.PrefixLabel(rect, new GUIContent(label));
            EditorGUI.PropertyField(contentRect, property, GUIContent.none);
        }

        private void DrawClipPropertyField()
        {
            switch ((ClipReferenceType)clipReferenceType.enumValueIndex)
            {
                case ClipReferenceType.File:
                    currentPosition.y += spacing;
                    DrawProperty(currentPosition, clip, "Clip");
                    break;
                case ClipReferenceType.Library:
                    DrawClipSelector();
                    break;
            }
        }

        private void DrawClipSelector()
        {
            //Group
            int groupPupupIndex = 0;
            int groupIndex = clipAssetLibrary.FindIndex(groupName.stringValue);
            if (groupIndex != -1)
                groupPupupIndex = groupIndex + 1;

            currentPosition.y += spacing;
            int newGroupPupupIndex = EditorGUI.Popup(
                currentPosition,
                "Group Name",
                groupPupupIndex,
                GenerateGroupPopupOptions().ToArray()
            );
            if (newGroupPupupIndex > 0)
                groupName.stringValue = clipAssetLibrary[newGroupPupupIndex - 1].GroupName;
            else
                groupName.stringValue = "";

            //Clip
            ClipAssetGroup group = clipAssetLibrary[groupName.stringValue];
            if (group == null)
            {
                currentPosition.y += spacing;
                EditorGUI.Popup(currentPosition, "Clip Name", 0, new string[] { emptyString });
                clipName.stringValue = "";
            }
            else
            {
                int clipPupupIndex = 0;
                int clipIndex = clipAssetLibrary[groupName.stringValue].FindIndex(
                    clipName.stringValue
                );
                if (clipIndex != -1)
                    clipPupupIndex = clipIndex + 1;

                currentPosition.y += spacing;
                int newClipPupupIndex = EditorGUI.Popup(
                    currentPosition,
                    "Clip Name",
                    clipPupupIndex,
                    GenerateClipPopupOptions().ToArray()
                );
                if (newClipPupupIndex > 0)
                    clipName.stringValue = clipAssetLibrary[groupName.stringValue][
                        newClipPupupIndex - 1
                    ].clipName;
                else
                    clipName.stringValue = "";
            }
        }

        private void LoadClipAssetLibrary()
        {
            clipAssetLibrary = AssetDatabase.LoadAssetAtPath<ClipAssetLibrary>(clipLibraryPath);
        }

        private IEnumerable<string> GenerateGroupPopupOptions()
        {
            yield return emptyString;
            for (int i = 0; i < clipAssetLibrary.GroupCount; i++)
                yield return clipAssetLibrary[i].GroupName;
        }

        private IEnumerable<string> GenerateClipPopupOptions()
        {
            ClipAssetGroup group = clipAssetLibrary[groupName.stringValue];
            yield return emptyString;
            for (int i = 0; i < group.Count; i++)
                yield return group[i].clipName;
        }
    }
}
