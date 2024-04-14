using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(PluginData))]
public class PluginDataEditorWindow : Editor
{
	Vector2 horizontalScroll = Vector2.zero;
	ReorderableList gameplayFeaturesList = null;
	SerializedObject data;
	//public static void Open(PluginData pluginData)
	//{
	//	PluginDataEditorWindow window = GetWindow<PluginDataEditorWindow>("Plugin Data");
	//	window.serializedObject = new SerializedObject(pluginData);
	//}

	//private void OnGUI()
	//{
	//	currentProperty = serializedObject.FindProperty("PlayerTypesCount");
	//	DrawProperties(currentProperty, true);
	//}

	private void OnEnable()
	{
		PluginData _data = (PluginData)target;
		data = new SerializedObject(_data);
		gameplayFeaturesList = new ReorderableList(data, data.FindProperty("gameplayFeatures"), true, true, true, true);

		gameplayFeaturesList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Gameplay Features");
		gameplayFeaturesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			rect.y += 2.0f;
			rect.height = EditorGUIUtility.singleLineHeight;
			GUIContent objLabel = new GUIContent($"Feature {index}");
			EditorGUI.PropertyField(rect, gameplayFeaturesList.serializedProperty.GetArrayElementAtIndex(index), objLabel);
		};
	}

	public override void OnInspectorGUI()
	{
		GUILayoutOption[] options = { GUILayout.MinWidth(28.0f), GUILayout.MaxWidth(200.0f) };

		if (Application.isPlaying && !data.FindProperty("canEditDuringPlayMode").boolValue)
		{
			GUI.enabled = false;
		}

		EditorGUILayout.Space();

		GUITitle();

		EditorGUILayout.Space();

		GUITable(data, options);

		EditorGUILayout.Space();
		EditorGUILayout.Separator();
		EditorGUILayout.Space();

		GUITableSize();

		EditorGUILayout.Space();

		GUICellValues();

		EditorGUILayout.Space();

		GUICanEditDuringPlayMode();

		EditorGUILayout.Space();
		EditorGUILayout.Separator();
		EditorGUILayout.Space();

		gameplayFeaturesList.DoLayoutList();

		EditorGUILayout.Space();

		if (GUI.changed)
		{
			//Undo.RecordObject(data, "Table");
			//EditorUtility.SetDirty(data);
			data.ApplyModifiedProperties();
		}
	}

	private static void GUITitle()
	{
		var titleLabelStyle = new GUIStyle();
		titleLabelStyle.fontStyle = FontStyle.BoldAndItalic;
		titleLabelStyle.fontSize = 14;
		EditorGUILayout.LabelField("Table of Gameplay Features", titleLabelStyle);
	}

	private void GUITableSize()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUIModularTextField(ObjectNames.NicifyVariableName(data.FindProperty("playerTypesCount").name));
		GUI.enabled = false;
		EditorGUILayout.IntField(data.FindProperty("playerTypesCount").intValue, GUILayout.Width(24.0f));
		GUI.enabled = true;
		if (Application.isPlaying && !data.FindProperty("canEditDuringPlayMode").boolValue)
		{
			GUI.enabled = false;
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUIModularTextField(ObjectNames.NicifyVariableName(data.FindProperty("gameplayFeaturesCount").name));
		GUI.enabled = false;
		EditorGUILayout.IntField(data.FindProperty("gameplayFeaturesCount").intValue, GUILayout.Width(24.0f));
		GUI.enabled = true;
		if (Application.isPlaying && !data.FindProperty("canEditDuringPlayMode").boolValue)
		{
			GUI.enabled = false;
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
	}

	private void GUICellValues()
	{
		EditorGUILayout.BeginVertical();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUIModularTextField(ObjectNames.NicifyVariableName(data.FindProperty("minCellValue").name));
		data.FindProperty("minCellValue").floatValue = EditorGUILayout.FloatField(data.FindProperty("minCellValue").floatValue, GUILayout.Width(24.0f));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUIModularTextField(ObjectNames.NicifyVariableName(data.FindProperty("maxCellValue").name));
		data.FindProperty("maxCellValue").floatValue = EditorGUILayout.FloatField(data.FindProperty("maxCellValue").floatValue, GUILayout.Width(24.0f));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		GUIModularTextField("Warning: Min Cell Value and Max Cell Value are the minimum and maximum any cell in the table can be.");
		GUIModularTextField("VERY DANGEROUS.");
		EditorGUILayout.EndVertical();
	}

	private void GUICanEditDuringPlayMode()
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUIModularTextField(ObjectNames.NicifyVariableName(data.FindProperty("canEditDuringPlayMode").name + ":"));
		data.FindProperty("canEditDuringPlayMode").boolValue = EditorGUILayout.Toggle(data.FindProperty("canEditDuringPlayMode").boolValue);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
	}

	private static void GUIModularTextField(string title, GUIStyle style = null)
	{
		var label = new GUIContent(title);
		if (style != null)
			EditorGUILayout.LabelField(title, style, GUILayout.Width(GUI.skin.label.CalcSize(label).x));
		else
			EditorGUILayout.LabelField(title, GUILayout.Width(GUI.skin.label.CalcSize(label).x));
	}

	private void GUITable(SerializedObject data, GUILayoutOption[] options)
	{
		var tableStyle = new GUIStyle("box");
		tableStyle.padding = new RectOffset(10, 10, 10, 10);

		var columnLabelStyle = new GUIStyle();
		columnLabelStyle.alignment = TextAnchor.MiddleCenter;
		columnLabelStyle.fontStyle = FontStyle.Bold;

		var rowLabelStyle = new GUIStyle();
		rowLabelStyle.alignment = TextAnchor.MiddleRight;
		rowLabelStyle.fontStyle = FontStyle.Bold;

		EditorGUILayout.BeginVertical(tableStyle);
		horizontalScroll = GUILayout.BeginScrollView(horizontalScroll);
		EditorGUILayout.BeginHorizontal();
		for (int x = -1; x < data.FindProperty("gameplayFeaturesCount").intValue; x++)
		{
			EditorGUILayout.BeginVertical();

			if (x >= 0)
			{
				if (data.FindProperty("gameplayFeatures").arraySize == data.FindProperty("gameplayFeaturesCount").intValue)
				{
					var feature = (GameplayFeature)data.FindProperty("gameplayFeatures").GetArrayElementAtIndex(x).objectReferenceValue;
					feature.featureEnabled = EditorGUILayout.Toggle(feature.featureEnabled);
					data.FindProperty("gameplayFeatures").GetArrayElementAtIndex(x).objectReferenceValue = feature;
				}
			}
			else
			{
				EditorGUILayout.BeginVertical();
				GUIModularTextField("");
				EditorGUILayout.EndHorizontal();
			}

			for (int y = -1; y < data.FindProperty("playerTypesCount").intValue; y++)
			{
				if (x == -1 && y == -1)
				{
					EditorGUILayout.BeginVertical();
					GUIModularTextField("");
					EditorGUILayout.EndHorizontal();
				}
				else if (x == -1)
				{
					EditorGUILayout.BeginVertical();
					var title = ((PlayerTypes)y).ToString().ToLower();
					var newTitle = "";
					var split = title.Split('_');
					foreach (var i in split)
					{
						newTitle += i.FirstCharacterToUpper() + " ";
					}
					GUIModularTextField(newTitle, rowLabelStyle);
					EditorGUILayout.EndHorizontal();

				}
				else if (y == -1)
				{
					EditorGUILayout.BeginVertical();
					var feature = (GameplayFeature)data.FindProperty("gameplayFeatures").GetArrayElementAtIndex(x).objectReferenceValue;
					GUIModularTextField(feature.featureName, columnLabelStyle);
					EditorGUILayout.EndHorizontal();
				}

				if (x >= 0 && y >= 0)
				{
					EditorGUILayout.BeginHorizontal();
					data.FindProperty("tableOfFeatures").GetArrayElementAtIndex(x + y * data.FindProperty("gameplayFeaturesCount").intValue).intValue = (int)Mathf.Clamp(EditorGUILayout.IntField(data.FindProperty("tableOfFeatures").GetArrayElementAtIndex(x + y * 15).intValue, options), data.FindProperty("minCellValue").floatValue, data.FindProperty("maxCellValue").floatValue);
					EditorGUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	//private static string tablePath = "Assets/SO/Table.asset";
	//Vector2 scrollPos;
	//string t = "This is a string inside a Scroll view!";
	////public string[] Strings = { "Larry", "Curly", "Moe" };

	//[MenuItem("Tools/Plugin Data Editor Window")]
	//static void Init()
	//{
	//	GetWindow<PluginDataEditorWindow>();
	//}

	//void OnGUI()
	//{
	//	//ScriptableObject target = this;
	//	//SerializedObject so = new SerializedObject(target);
	//	//SerializedProperty stringsProperty = so.FindProperty("Strings");
	//	//EditorGUILayout.PropertyField(stringsProperty, true);
	//	//so.ApplyModifiedProperties();


	//	PluginData readOnlyData = (PluginData)AssetDatabase.LoadAssetAtPath(tablePath, typeof(PluginData));






	//	//int[][] TheNewList = new int[6][];
	//	//TheNewList[0] = new int[] { 1, 2, 3, 4 };
	//	//TheNewList[1] = new int[] { 1, 2, 3, 4 };
	//	//TheNewList[2] = new int[] { 1, 2, 3, 4 };
	//	//TheNewList[3] = new int[] { 1, 2, 3, 4 };
	//	//TheNewList[4] = new int[] { 1, 2, 3, 4 };
	//	//TheNewList[5] = new int[] { 1, 2, 3, 4 };
	//	//List<int> TheNewList = new List<int>();
	//	//var serializedData = new SerializedObject(readOnlyData);
	//	//SerializedProperty table = serializedData.FindProperty("tableOfFeatures");

	//	EditorGUI.BeginChangeCheck();
	//	//serializedData.Update();
	//	for (int i = 0; i < 6; i++)
	//	{
	//		//SerializedProperty array = table.GetArrayElementAtIndex(i);
	//		EditorGUILayout.BeginHorizontal();
	//		{
	//			var s1 = new GUIStyle { padding = { top = 20, left = 2 } };

	//			if (i == 0)
	//			{
	//				EditorGUILayout.LabelField("eix x " + (i + 1), s1, GUILayout.MaxWidth(40));
	//			}
	//			else
	//			{
	//				EditorGUILayout.LabelField("eix x " + (i + 1), GUILayout.MaxWidth(40));
	//			}

	//			for (var j = 0; j < 15; j++)
	//			{
	//				//var number = array.GetArrayElementAtIndex(j);
	//				var number = readOnlyData.tableOfFeatures[i][j];
	//				EditorGUILayout.BeginVertical();
	//				{
	//					if (i == 0)
	//					{
	//						EditorGUILayout.LabelField(("eix y " + j + 1).ToString(), GUILayout.MaxWidth(10));
	//					}
	//					//EditorGUILayout.PropertyField(number, GUIContent.none, GUILayout.MaxWidth(20));
	//					EditorGUILayout.IntField(number, GUILayout.MaxWidth(20));
	//				}
	//				EditorGUILayout.EndVertical();
	//			}
	//		}
	//		EditorGUILayout.EndHorizontal();
	//		EditorGUILayout.Space();
	//	}
	//	//serializedData.ApplyModifiedProperties();




















	//	EditorGUILayout.BeginHorizontal();
	//	scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(100), GUILayout.Height(100));
	//	GUILayout.Label(t);
	//	EditorGUILayout.EndScrollView();
	//	if (GUILayout.Button("Add More Text", GUILayout.Width(100), GUILayout.Height(100)))
	//		t += " \nAnd this is more text!";
	//	EditorGUILayout.EndHorizontal();
	//	if (GUILayout.Button("Clear"))
	//		t = "";
	//	EditorGUI.EndChangeCheck();










	//	// TO SAVE DO THIS
	//	//EditorUtility.SetDirty(data);
	//	//AssetDatabase.SaveAssets();
	//	//AssetDatabase.Refresh();
	//}
}
