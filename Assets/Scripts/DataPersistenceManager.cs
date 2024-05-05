using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
	[Header("File Storage Config")]

	// file name of the save file
	[SerializeField] private string fileName;

	// boolean to enable encryption
	[SerializeField] private bool useEncryption;

	// objects that make use of the save file
	private List<IDataPersistence> dataPersistenceObjects;

	// singleton instance
	public static DataPersistenceManager Instance { get; private set; }

	private PlayerProfileData profileData;
	private FileDataHandler dataHandler;
	[SerializeField] private PluginController pluginController;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	// initiate data handler and load profile
	private void Start()
	{
		Debug.Log(Application.persistentDataPath);
		dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
		dataPersistenceObjects = FindAllDataPersistenceObjects();
		LoadProfile();
	}

	// empty new default profile
	public void NewProfile()
	{
		profileData = new PlayerProfileData();
	}

	// load profile in directory
	public void LoadProfile()
	{
		profileData = dataHandler.Load();

		if (profileData == null)
		{
			Debug.Log("No profile was found. Initializing profile to default values.");
			NewProfile();
		}

		if (dataPersistenceObjects.Count > 0)
		{
			foreach (var obj in dataPersistenceObjects)
				obj.LoadProfile(profileData);
		}

		Debug.Log("Loaded profile");
		Debug.Log("DEBUG:");
		foreach (var i in profileData.Profile)
			Debug.Log(i);
	}

	// save profile in directory
	public void SaveProfile()
	{
		if (dataHandler == null)
		{
			Debug.Log("Error trying to save profile. Data Handler was null");
			return;
		}

		if (dataPersistenceObjects.Count > 0)
		{
			foreach (var obj in dataPersistenceObjects)
				obj.SaveProfile(ref profileData);
		}

		dataHandler.Save(profileData);

		Debug.Log("Saved profile");
		Debug.Log("DEBUG:");
		foreach (var i in profileData.Profile)
			Debug.Log(i);
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
