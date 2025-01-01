using Edit;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AudioSystem.Edit
{
    public class AudioEditorWindow : EditorWindow
    {
        private const string AudioAssetPath = "Assets/Resources/Audio/AudioAsset.asset";

        private ClipAssetLibrary audioAsset;
        private SerializedObject groupSO;
        private SerializedProperty groupName;
        private SerializedProperty clipAssets;
        private ReorderableList clipList;

        private int groupIndex;

        private EditorGUISplitView splitView;
        private Color backgroundColor;
        private Color buttonFocusColor = new Color(0.6f, 0.9f, 1, 1);

        private bool AssetExist { get { return audioAsset && audioAsset.GroupCount > 0; } }
        private SerializedProperty ClipName(int index) { return clipAssets.GetArrayElementAtIndex(index).FindPropertyRelative("clipName"); }
        private SerializedProperty Clip(int index) { return clipAssets.GetArrayElementAtIndex(index).FindPropertyRelative("clip"); }

        [MenuItem("CustomAudio/Audios")]
        public static void ShowWindow()
        {
            AudioEditorWindow window = GetWindow<AudioEditorWindow>(false, "Audios", true);
            window.Initialize();
        }

        private void Initialize()
        {
            minSize = new Vector2(500, 350);
            splitView = new EditorGUISplitView(EditorGUISplitView.Direction.Horizontal, 200);

            LoadAudioAssets();
            if (AssetExist) SelectGroup(0);
            else groupIndex = -1;
        }

        private void OnGUI()
        {
            if (splitView == null) Initialize();

            splitView.BeginSplitView();
            {
                if (AssetExist) DrawAudioGroupButtons();
            }
            splitView.Split();
            {
                if (groupIndex != -1) DrawGroupEditPanel();
            }
            splitView.EndSplitView();
        }

        private void SelectGroup(int index)
        {
            groupIndex = index;
            groupSO = new SerializedObject(audioAsset[index]);
            groupName = groupSO.FindProperty("groupName");
            clipAssets = groupSO.FindProperty("assets");
            GenerateClipList();
        }

        private GUIStyle groupButtonStyle;
        private void DrawAudioGroupButtons()
        {
            groupButtonStyle = new GUIStyle(EditorStyles.miniButton) { fontSize = 14, fixedHeight = 30 };

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(15);
                GUILayout.Label("Groups", new GUIStyle(EditorStyles.label) { fontSize = 20, fontStyle = FontStyle.Bold });
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(15);
                GUILayout.BeginVertical();
                {
                    backgroundColor = GUI.backgroundColor;
                    for (int i = 0; i < audioAsset.GroupCount; i++)
                    {
                        if (groupIndex == i) GUI.backgroundColor = buttonFocusColor;
                        if (GUILayout.Button(audioAsset[i].GroupName, groupButtonStyle)) SelectGroup(i);
                        GUI.backgroundColor = backgroundColor;
                        GUILayout.Space(5);
                    }
                }
                GUILayout.EndVertical();
                GUILayout.Space(15);
            }
            GUILayout.EndHorizontal();
        }

        private void DrawGroupEditPanel()
        {
            groupSO.Update();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Group Name", new GUIStyle(EditorStyles.label) { fontSize = 16 }, GUILayout.MaxWidth(ClipNameSize + 23));
                    EditorGUILayout.PropertyField(groupName, GUIContent.none);
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.LabelField("Clips", new GUIStyle(EditorStyles.label) { fontSize = 16 }, GUILayout.MaxWidth(60));

                GUILayout.Space(5);

                clipList.DoLayoutList();
            }
            GUILayout.EndVertical();
            GUILayout.Space(15);
            groupSO.ApplyModifiedProperties();
        }

        private void GenerateClipList()
        {
            clipList = new ReorderableList(groupSO, clipAssets, true, false, true, true);
            clipList.elementHeight = 20;
            clipList.drawElementCallback = DrawClipAsset;
            clipList.onAddCallback = AddClipAsset;
        }

        private const int ClipNameSize = 100;
        private void DrawClipAsset(Rect rect, int index, bool isActive, bool isFocused)
        {
            float originWidth = rect.width;

            rect.x += 2;
            rect.y += 2;
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.width = ClipNameSize;
            EditorGUI.PropertyField(rect, ClipName(index), GUIContent.none);

            rect.x += ClipNameSize + 5;
            rect.width = originWidth - ClipNameSize - 7;
            EditorGUI.PropertyField(rect, Clip(index), GUIContent.none);
        }
        private void AddClipAsset(ReorderableList list = null)
        {
            clipAssets.arraySize += 1;
            groupSO.ApplyModifiedProperties();

            ClipName(clipAssets.arraySize - 1).stringValue = "NewClip";
            Clip(clipAssets.arraySize - 1).objectReferenceValue = null;
            groupSO.ApplyModifiedProperties();
        }

        private void LoadAudioAssets()
        {
            audioAsset = AssetDatabase.LoadAssetAtPath<ClipAssetLibrary>(AudioAssetPath);
        }
    }
}