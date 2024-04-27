using System;
using System.Collections.Generic;
using UnityEngine;

public class PluginController : MonoBehaviour, IDataPersistence
{
	// singleton instance
	public static PluginController Instance { get; private set; }

	// player profile data
	public PlayerProfileData PlayerProfileData;

	// plugin data
	[SerializeField] private PluginData pluginData;

	//current gameplay feature coefficient
	public Dictionary<GameplayFeature, float> FeaturesCoefficient = new Dictionary<GameplayFeature, float>();

	// event
	public event Action OnFeaturesCoefficientUpdated;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	// trigger
	public void PlayerTypeTrigger(Dictionary<PlayerTypes, float> changesToUpdate)
	{
		// TODO: Update the profile & act accordingly
		foreach (var profileChange in changesToUpdate)
			PlayerProfileData.Profile[profileChange.Key] += profileChange.Value;

		UpdateFeaturesCoefficient();
	}

	public void UpdateFeaturesCoefficient()
	{
		if (pluginData == null)
		{
			Debug.LogWarning("Trying to update features coefficient but plugin data was null.\nAborting...");
			return;
		}

		FeaturesCoefficient.Clear();

		for (int i = 0; i < pluginData.GameplayFeatures.Length; i++)
		{
			GameplayFeature feature = pluginData.GameplayFeatures[i];
			if (feature.FeatureEnabled)
			{
				float newCoef = 0.0f;

				foreach (var profilePlayerType in PlayerProfileData.Profile)
					newCoef += pluginData.TableOfFeatures[i + (int)profilePlayerType.Key * pluginData.GameplayFeaturesCount] * profilePlayerType.Value;

				newCoef = ((newCoef / 300.0f) - 0.5f) * 2;
				FeaturesCoefficient.Add(feature, newCoef);
			}
		}

		OnFeaturesCoefficientUpdated?.Invoke(); // segons el player type fes x o y ???
	}

	public void LoadProfile(PlayerProfileData data)
	{
		PlayerProfileData = data;
		UpdateFeaturesCoefficient();
	}

	public void SaveProfile(ref PlayerProfileData data)
	{
		data = PlayerProfileData;
	}
}
