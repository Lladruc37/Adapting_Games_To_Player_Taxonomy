using System;
using UnityEngine;

public class SurveyController : MonoBehaviour
{
	public int RunNumber { get; private set; }
	public bool profileQuestion = false;
	public bool takingSurvey = false;
	public char testGroup;
	public SurveyData surveyData;

	public static SurveyController Instance { get; private set; }
	public static bool Initialized = false;

	private void Awake()
	{
		Initialized = true;
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void SetupRun(int number)
	{
		takingSurvey = true;
		RunNumber = number;
		switch (RunNumber)
		{
			case 1: //base game
				profileQuestion = true;
				PluginController.Instance.ResetProfile();
				PluginController.Instance.pluginEnabled = false;
				surveyData = new SurveyData();
				surveyData.testGroup = MathF.Floor(UnityEngine.Random.Range(0f, 2.99999999f)) switch
				{
					0 => 'A',
					1 => 'B',
					2 => 'C',
					_ => '-',
				};
				break;
			case 2: //A: no changes, base game / B: using player's answers / C: different profile
				profileQuestion = false;
				PluginController.Instance.pluginEnabled = true;
				switch (surveyData.testGroup)
				{
					case 'A':
						Debug.Log("Test group A");
						PluginController.Instance.ResetProfile();
						PluginController.Instance.pluginEnabled = false;
						break;
					case 'B':
						{
							Debug.Log("Test group B");
							var profile = surveyData.runs[0].finalProfile.Profile;
							for (int i = 0; i < profile.Count; i++)
							{
								Debug.Log((PlayerTypes)i);
								PluginController.Instance.PlayerProfileData.Profile[(PlayerTypes)i] = profile[(PlayerTypes)i];
								Debug.Log(profile[(PlayerTypes)i]);
							}

							PluginController.Instance.UpdateFeaturesCoefficient();
							break;
						}
					case 'C':
						{
							Debug.Log("Test group C");
							var profile = surveyData.runs[0].finalProfile.Profile;
							for (int i = 0; i < profile.Count; i++)
							{
								Debug.Log((PlayerTypes)i);
								PluginController.Instance.PlayerProfileData.Profile[(PlayerTypes)i] = (10f - profile[(PlayerTypes)i]);
								Debug.Log(profile[(PlayerTypes)i]);
							}

							PluginController.Instance.UpdateFeaturesCoefficient();
							break;
						}
					default:
						break;
				}
				break;
			default:
				break;
		}
		var runData = new SurveyDataRun
		{
			initialProfile = new PlayerProfileData(PluginController.Instance.PlayerProfileData)
		};
		surveyData.runs.Add(runData);
	}

	public void SetupNextRun()
	{
		RunNumber++;
		SetupRun(RunNumber);
	}
}
