using UnityEditor;
using UnityEngine;

public class PluginDataEditorWindow : EditorWindow
{
	Vector2 scrollPos;
	string t = "This is a string inside a Scroll view!";
	float columnHeight = 10.0f;

	[MenuItem("Tools/Plugin Data Editor Window")]
	static void Init()
	{
		GetWindow<PluginDataEditorWindow>();
	}

	void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		scrollPos =
			EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(100), GUILayout.Height(100));
		GUILayout.Label(t);
		EditorGUILayout.EndScrollView();
		if (GUILayout.Button("Add More Text", GUILayout.Width(100), GUILayout.Height(100)))
			t += " \nAnd this is more text!";
		EditorGUILayout.EndHorizontal();
		if (GUILayout.Button("Clear"))
			t = "";
	}
}
