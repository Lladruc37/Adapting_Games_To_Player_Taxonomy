using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(PluginData))]
public class PluginDataEditorWindow : Editor
{
	private Vector2 horizontalScroll = Vector2.zero;
	private ReorderableList gameplayFeaturesList = null;
	private SerializedObject data;

	private void OnEnable()
	{
		PluginData _data = (PluginData)target;
		data = new SerializedObject(_data);
		gameplayFeaturesList = new ReorderableList(data, data.FindProperty("GameplayFeatures"), true, true, true, true);

		gameplayFeaturesList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Gameplay Features");
		gameplayFeaturesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
		{
			rect.y += 2.0f;
			rect.height = EditorGUIUtility.singleLineHeight;
			var objLabel = new GUIContent($"Feature {index}");
			EditorGUI.PropertyField(rect, gameplayFeaturesList.serializedProperty.GetArrayElementAtIndex(index), objLabel);
		};
	}

	public override void OnInspectorGUI()
	{
		GUILayoutOption[] options = { GUILayout.MinWidth(28.0f), GUILayout.MaxWidth(240.0f) };

		if (Application.isPlaying && !data.FindProperty("CanEditDuringPlayMode").boolValue)
			GUI.enabled = false;

		EditorGUILayout.Space();

		GUITitle();

		EditorGUILayout.Space();

		GUITable(options);

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
			data.ApplyModifiedProperties();
	}

	private static void GUITitle()
	{
		var titleLabelStyle = new GUIStyle();
		titleLabelStyle.fontStyle = FontStyle.BoldAndItalic;
		titleLabelStyle.fontSize = 14;
		EditorGUILayout.LabelField("Table of Gameplay Features", titleLabelStyle);
	}

	private void GUITable(GUILayoutOption[] options)
	{
		var boxStyle = new GUIStyle("box");
		boxStyle.padding = new RectOffset(10, 10, 10, 10);

		var columnLabelStyle = new GUIStyle();
		columnLabelStyle.alignment = TextAnchor.MiddleCenter;
		columnLabelStyle.fontStyle = FontStyle.Bold;

		var rowLabelStyle = new GUIStyle();
		rowLabelStyle.alignment = TextAnchor.MiddleRight;
		rowLabelStyle.fontStyle = FontStyle.Bold;

		// variables
		int playerTypesCount = data.FindProperty("PlayerTypesCount").intValue;
		int gameplayFeaturesCount = data.FindProperty("GameplayFeaturesCount").intValue;
		SerializedProperty gameplayFeatures = data.FindProperty("GameplayFeatures");
		SerializedProperty tableOfFeatures = data.FindProperty("TableOfFeatures");

		EditorGUILayout.BeginVertical(boxStyle);
		horizontalScroll = GUILayout.BeginScrollView(horizontalScroll);
		EditorGUILayout.BeginHorizontal();

		for (int x = -1; x < gameplayFeaturesCount; x++)
		{
			EditorGUILayout.BeginVertical();

			if (x >= 0)
			{
				if (gameplayFeatures.arraySize > x)
				{
					// gameplay feature enabled toggle
					var feature = (GameplayFeature)gameplayFeatures.GetArrayElementAtIndex(x).objectReferenceValue;
					if (feature != null)
					{
						feature.FeatureEnabled = EditorGUILayout.Toggle(feature.FeatureEnabled);
						gameplayFeatures.GetArrayElementAtIndex(x).objectReferenceValue = feature;
					}
					else
						GUIModularTextField("Error");
				}
				else
					GUIModularTextField("Error");
			}
			else
			{
				// empty cell
				EditorGUILayout.BeginVertical();
				GUIModularTextField("");
				EditorGUILayout.EndHorizontal();
			}

			for (int y = -1; y < playerTypesCount; y++)
			{
				if (x == -1 && y == -1)
				{
					// empty cell
					EditorGUILayout.BeginVertical();
					GUIModularTextField("");
					EditorGUILayout.EndHorizontal();
				}
				else if (x == -1)
				{
					// player types title cell
					EditorGUILayout.BeginVertical();
					GUIModularTextField(PlayerTypeIndexToTitle(y), rowLabelStyle);
					EditorGUILayout.EndHorizontal();

				}
				else if (y == -1)
				{
					if (gameplayFeatures.arraySize > x)
					{
						// gameplay feature title cell
						EditorGUILayout.BeginVertical();
						var feature = (GameplayFeature)gameplayFeatures.GetArrayElementAtIndex(x).objectReferenceValue;

						if (feature != null)
							GUIModularTextField(feature.FeatureName, columnLabelStyle);
						else
							GUIModularTextField("Error");

						EditorGUILayout.EndHorizontal();
					}
					else
					{
						EditorGUILayout.BeginVertical();
						GUIModularTextField("Error");
						EditorGUILayout.EndHorizontal();
					}
				}

				if (x >= 0 && y >= 0)
				{
					if ((x + y * gameplayFeaturesCount) < (gameplayFeaturesCount * playerTypesCount))
					{
						// number cell
						EditorGUILayout.BeginHorizontal();
						tableOfFeatures.GetArrayElementAtIndex(x + y * gameplayFeaturesCount).intValue = (int)Mathf.Clamp(EditorGUILayout.IntField(tableOfFeatures.GetArrayElementAtIndex(x + y * gameplayFeaturesCount).intValue, options), data.FindProperty("MinCellValue").floatValue, data.FindProperty("MaxCellValue").floatValue);
						EditorGUILayout.EndHorizontal();
					}
					else
					{
						EditorGUILayout.BeginVertical();
						GUIModularTextField("Error");
						EditorGUILayout.EndHorizontal();
					}
				}
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	private void GUITableSize()
	{
		bool canEditDuringPlayMode = data.FindProperty("CanEditDuringPlayMode").boolValue;
		SerializedProperty playerTypesCount = data.FindProperty("PlayerTypesCount");
		SerializedProperty gameplayFeaturesCount = data.FindProperty("GameplayFeaturesCount");

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal();

		GUIModularTextField(ObjectNames.NicifyVariableName(playerTypesCount.name));
		GUI.enabled = false;
		EditorGUILayout.IntField(playerTypesCount.intValue, GUILayout.Width(24.0f));
		GUI.enabled = true;

		if (Application.isPlaying && !canEditDuringPlayMode)
			GUI.enabled = false;

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();

		GUIModularTextField(ObjectNames.NicifyVariableName(gameplayFeaturesCount.name));
		GUI.enabled = false;
		EditorGUILayout.IntField(gameplayFeaturesCount.intValue, GUILayout.Width(24.0f));
		GUI.enabled = true;

		if (Application.isPlaying && !canEditDuringPlayMode)
			GUI.enabled = false;

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
	}

	private void GUICellValues()
	{
		SerializedProperty minCellValue = data.FindProperty("MinCellValue");
		SerializedProperty maxCellValue = data.FindProperty("MaxCellValue");

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal();

		GUIModularTextField(ObjectNames.NicifyVariableName(minCellValue.name));
		minCellValue.floatValue = EditorGUILayout.FloatField(minCellValue.floatValue, GUILayout.Width(24.0f));

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();

		GUIModularTextField(ObjectNames.NicifyVariableName(maxCellValue.name));
		maxCellValue.floatValue = EditorGUILayout.FloatField(maxCellValue.floatValue, GUILayout.Width(24.0f));

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();

		var boxStyle = new GUIStyle("box");
		boxStyle.padding = new RectOffset(10, 10, 10, 10);
		EditorGUILayout.BeginVertical(boxStyle);

		GUIModularTextField("Warning: Min Cell Value and Max Cell Value are the minimum and maximum any cell in the table can be.");
		GUIModularTextField("VERY DANGEROUS.");

		EditorGUILayout.EndVertical();
	}

	private void GUICanEditDuringPlayMode()
	{
		SerializedProperty canEditDuringPlayMode = data.FindProperty("CanEditDuringPlayMode");

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal();

		GUIModularTextField(ObjectNames.NicifyVariableName(canEditDuringPlayMode.name + ":"));
		canEditDuringPlayMode.boolValue = EditorGUILayout.Toggle(canEditDuringPlayMode.boolValue);

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

	private static string PlayerTypeIndexToTitle(int index)
	{
		var title = ((PlayerTypes)index).ToString().ToLower();
		var split = title.Split('_');
		title = "";

		foreach (var i in split)
			title += i.FirstCharacterToUpper() + " ";

		return title;
	}
}
