using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
	[Header("File Storage Config")]

	// file name of the save file
	[SerializeField] private string profileFileName;
	[SerializeField] private string surveyFileName;

	// boolean to enable encryption
	[SerializeField] private bool useEncryption;

	// objects that make use of the save file
	private List<IDataPersistence> dataPersistenceObjects;

	// singleton instance
	public static DataPersistenceManager Instance { get; private set; }

	private PlayerProfileData profileData;
	private FileDataHandler profileDataHandler;
	private FileDataHandler surveyDataHandler;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	// initiate data handler and load profile
	private void Start()
	{
		Debug.Log(Application.persistentDataPath);
		Debug.Log(Application.dataPath);
		if (!string.IsNullOrEmpty(profileFileName))
		{
			var surveyPath = Path.Combine(Application.dataPath, surveyFileName);
			profileDataHandler = new FileDataHandler(Application.persistentDataPath, profileFileName, useEncryption);
			surveyDataHandler = new FileDataHandler(Application.dataPath, surveyFileName, useEncryption);
			try
			{
				if (File.Exists(surveyPath))
					File.Delete(surveyPath);
			}
			catch (Exception e)
			{
				Debug.LogError($"Error trying to delete survey file in: {surveyPath}\n {e}");
			}

			dataPersistenceObjects = FindAllDataPersistenceObjects();
			LoadProfile();
		}
	}

	// load profile in directory
	public void LoadProfile()
	{
		profileData = profileDataHandler.Load();

		if (profileData == null)
		{
			Debug.Log("No profile was found. Initializing profile to default values.");
			profileData = new PlayerProfileData();
		}

		if (dataPersistenceObjects.Count > 0)
		{
			foreach (var obj in dataPersistenceObjects)
				obj.LoadProfile(profileData);
		}

		// TODO: TAKE THIS OUT
		Debug.Log("Loaded profile");
		Debug.Log("DEBUG:");
		foreach (var i in profileData.Profile)
			Debug.Log(i);
	}

	// save profile in directory
	public void SaveProfile()
	{
		if (profileDataHandler == null)
		{
			Debug.Log("Error trying to save profile. Data Handler was null");
			return;
		}

		if (dataPersistenceObjects.Count > 0)
		{
			foreach (var obj in dataPersistenceObjects)
				obj.SaveProfile(ref profileData);
		}

		profileDataHandler.Save(profileData);

		// TODO: TAKE THIS OUT
		Debug.Log("Saved profile");
		Debug.Log("DEBUG:");
		foreach (var i in profileData.Profile)
			Debug.Log(i);
	}

	public void SaveSurveyRun(SurveyDataRun data)
	{
		surveyDataHandler.Save(data);
	}

	// on application closed save profile in directory
	private void OnApplicationQuit()
	{
		SaveProfile();
	}

	private List<IDataPersistence> FindAllDataPersistenceObjects()
	{
		IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
		return new List<IDataPersistence>(dataPersistenceObjects);
	}
}
