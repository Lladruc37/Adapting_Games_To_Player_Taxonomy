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
	public int playerTypesCount = StaticPlayerTypesCount;
	private static int StaticPlayerTypesCount = 6;

	public int gameplayFeaturesCount = StaticGameplayFeaturesCount;
	private static int StaticGameplayFeaturesCount = 15;

	public float minCellValue = 0.0f;
	public float maxCellValue = 10.0f;
	public bool canEditDuringPlayMode = false;

	public GameplayFeature[] gameplayFeatures = new GameplayFeature[] { };
	public int[] tableOfFeatures = new int[StaticPlayerTypesCount * StaticGameplayFeaturesCount];

	/*void Start()
	{
		SetTableData();
	}

	private void SetTableData()
	{
		if (setDefault)
		{
			for (int i = 0; i < gameplayFeatures.Count; i++)
			{
				int[] feature = { 5, 5, 5, 5, 5, 5 };
				tableOfFeatures[i] = feature;
			}
		}
	}

	void Update()
	{

	}
	*/
}

/* filter only enabled features
 
		int featuresCount = 0;
        foreach (var feature in gameplayFeatures)
        {
            if(feature.featureEnabled)
				featuresCount++;
        }
 
 */