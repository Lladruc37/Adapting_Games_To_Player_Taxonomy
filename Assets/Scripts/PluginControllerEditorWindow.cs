using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PluginController))]
public class PluginControllerEditorWindow : Editor
{
	private Vector2 horizontalScroll = Vector2.zero;
	private PluginController controller;
	private PlayerProfileData unsavedProfileData;

	private void OnEnable()
	{
		controller = (PluginController)target;
	}

	public override void OnInspectorGUI()
	{
		GUILayoutOption[] options = { GUILayout.MinWidth(28.0f), GUILayout.MaxWidth(240.0f) };

		if (Application.isPlaying)
			GUI.enabled = false;

		EditorGUILayout.Space();

		GUITitle();

		EditorGUILayout.Space();

		//player profile
		GUIPlayerProfile(options);

		EditorGUILayout.Space();
		EditorGUILayout.Separator();
		EditorGUILayout.Space();

		//button
		if (GUILayout.Button("Apply Profile Changes", options))
		{
			PluginController.Instance.PlayerProfileData = unsavedProfileData;
			//controller.PlayerProfileData = unsavedProfileData;
			DataPersistenceManager.Instance.SaveProfile();
			unsavedProfileData = null;
		}

		EditorGUILayout.Space();

		//features coefficient
		GUIFeaturesCoefficient();

		EditorGUILayout.Space();

		var a = new SerializedObject(controller);
		EditorGUILayout.ObjectField(a.FindProperty("pluginData"));
		//GUICanEditDuringPlayMode();
		if (GUI.changed)
			a.ApplyModifiedProperties();

		if (GUILayout.Button("Update Features Coefficients", options))
			PluginController.Instance.UpdateFeaturesCoefficient();

		EditorGUILayout.Space();
		EditorGUILayout.Separator();
		EditorGUILayout.Space();

		//gameplayFeaturesList.DoLayoutList();

		EditorGUILayout.Space();

		//if (GUI.changed)
		//	data.ApplyModifiedProperties();
	}

	private static void GUITitle()
	{
		var titleLabelStyle = new GUIStyle();
		titleLabelStyle.fontStyle = FontStyle.BoldAndItalic;
		titleLabelStyle.fontSize = 14;
		EditorGUILayout.LabelField("Plugin Controller", titleLabelStyle);
	}

	private void GUIPlayerProfile(GUILayoutOption[] options)
	{
		if (unsavedProfileData == null || Application.isPlaying)
		{
			unsavedProfileData = new PlayerProfileData();
			for (var key = 0; key < PluginController.Instance.PlayerProfileData.Profile.Count; key++)
				unsavedProfileData.Profile[(PlayerTypes)key] = PluginController.Instance.PlayerProfileData.Profile[(PlayerTypes)key];
		}

		EditorGUILayout.BeginHorizontal();
		for (var key = 0; key < PluginController.Instance.PlayerProfileData.Profile.Count; key++)
		{
			EditorGUILayout.BeginVertical();

			GUIModularTextField(PlayerTypeToTitle((PlayerTypes)key));

			unsavedProfileData.Profile[(PlayerTypes)key] = EditorGUILayout.FloatField(unsavedProfileData.Profile[(PlayerTypes)key], options);

			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndHorizontal();
	}

	private void GUIFeaturesCoefficient()
	{
		var boxStyle = new GUIStyle("box");
		boxStyle.padding = new RectOffset(10, 10, 10, 10);

		EditorGUILayout.BeginVertical(boxStyle);
		horizontalScroll = GUILayout.BeginScrollView(horizontalScroll);
		EditorGUILayout.BeginHorizontal();

		foreach (var feature in PluginController.Instance.FeaturesCoefficient)
		{
			EditorGUILayout.BeginVertical();

			GUIModularTextField(feature.Key.FeatureName);
			GUI.enabled = false;
			EditorGUILayout.FloatField(feature.Value);
			if (!Application.isPlaying)
				GUI.enabled = true;

			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	//private void GUICanEditDuringPlayMode()
	//{
	//	SerializedProperty canEditDuringPlayMode = data.FindProperty("CanEditDuringPlayMode");

	//	EditorGUILayout.BeginHorizontal();
	//	EditorGUILayout.BeginHorizontal();

	//	GUIModularTextField(ObjectNames.NicifyVariableName(canEditDuringPlayMode.name + ":"));
	//	canEditDuringPlayMode.boolValue = EditorGUILayout.Toggle(canEditDuringPlayMode.boolValue);

	//	EditorGUILayout.EndHorizontal();
	//	EditorGUILayout.BeginHorizontal();
	//	EditorGUILayout.Space();
	//	EditorGUILayout.EndHorizontal();
	//	EditorGUILayout.EndHorizontal();
	//}

	private static void GUIModularTextField(string title, GUIStyle style = null)
	{
		var label = new GUIContent(title);

		if (style != null)
			EditorGUILayout.LabelField(title, style, GUILayout.Width(GUI.skin.label.CalcSize(label).x));
		else
			EditorGUILayout.LabelField(title, GUILayout.Width(GUI.skin.label.CalcSize(label).x));
	}

	private static string PlayerTypeToTitle(PlayerTypes playerType)
	{
		var title = playerType.ToString().ToLower();
		var split = title.Split('_');
		title = "";

		foreach (var i in split)
			title += i.FirstCharacterToUpper() + " ";

		return title;
	}
}
