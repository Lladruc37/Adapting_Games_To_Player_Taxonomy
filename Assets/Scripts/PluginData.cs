using UnityEngine;

public enum PlayerTypes
{
	ACHIEVER,
	FREE_SPIRIT,
	PHILANTHROPIST,
	PLAYER,
	SOCIALISER,
	DISRUPTOR
}

[CreateAssetMenu(menuName = "PluginData/Table")]
public class PluginData : ScriptableObject
{
	public int PlayerTypesCount = staticPlayerTypesCount;
	private static int staticPlayerTypesCount = 6;

	public int GameplayFeaturesCount = staticGameplayFeaturesCount;
	private static int staticGameplayFeaturesCount = 26;

	public float MinCellValue = 0.0f;
	public float MaxCellValue = 10.0f;
	public bool CanEditDuringPlayMode = false;

	public GameplayFeature[] GameplayFeatures = new GameplayFeature[] { };
	public int[] TableOfFeatures = new int[staticPlayerTypesCount * staticGameplayFeaturesCount];
}