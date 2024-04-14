using UnityEngine;

[CreateAssetMenu(menuName = "PluginData/GameplayFeature")]
public class GameplayFeature : ScriptableObject
{
	public bool featureEnabled = false;
	public string featureName;
}
