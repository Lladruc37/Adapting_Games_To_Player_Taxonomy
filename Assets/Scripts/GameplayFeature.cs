using UnityEngine;

[CreateAssetMenu(menuName = "PluginData/GameplayFeature")]
public class GameplayFeature : ScriptableObject
{
	// name of the feature
	public string FeatureName;

	// wether the feature will be considered or not
	public bool FeatureEnabled = true;
}
