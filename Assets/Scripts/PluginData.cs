using System.Collections.Generic;
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

public class PluginData : MonoBehaviour
{
	public int playerTypesCount = 6;
	public List<GameplayFeature> gameplayFeatures = new List<GameplayFeature>();
	public int[][] tableOfFeatures;
	public bool setDefault = true;

	void Start()
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
}

/* filter only enabled features
 
		int featuresCount = 0;
        foreach (var feature in gameplayFeatures)
        {
            if(feature.featureEnabled)
				featuresCount++;
        }
 
 */