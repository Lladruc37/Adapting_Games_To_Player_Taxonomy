//public class AssetHandler
//{
//	[OnOpenAsset()]
//	public static bool OpenEditor(int instanceId, int line)
//	{
//		PluginData obj = EditorUtility.InstanceIDToObject(instanceId) as PluginData;
//		if (obj != null)
//		{
//			PluginDataEditorWindow.Open(obj);
//			return true;
//		}
//		return false;
//	}
//}

//[CustomEditor(typeof(PluginData))]
//public class PluginDataCustomEditor : Editor
//{
//	public override void OnInspectorGUI()
//	{
//		if (GUILayout.Button("Open Editor"))
//		{
//			PluginDataEditorWindow.Open((PluginData)target);
//		}
//	}
//}
