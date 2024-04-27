using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
	// directory path where data will be stored
	private string dataDirPath = "";

	// file name of the save file
	private string dataFileName = "";

	// boolean to enable encryption
	private bool useEncryption = false;

	// encryption code word used to encrypt & decrypt
	private readonly string encryptionCode = "hexad";

	public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
	{
		this.dataDirPath = dataDirPath;
		this.dataFileName = dataFileName;
		this.useEncryption = useEncryption;
	}

	// loads save file from directory
	public PlayerProfileData Load()
	{
		var fullPath = Path.Combine(dataDirPath, dataFileName);
		PlayerProfileData data = null;

		if (File.Exists(fullPath))
		{
			try
			{
				var dataToLoad = "";
				using FileStream stream = new FileStream(fullPath, FileMode.Open);
				using StreamReader reader = new StreamReader(stream);
				dataToLoad = reader.ReadToEnd();

				if (useEncryption)
					dataToLoad = EncryptDecrypt(dataToLoad);

				data = JsonUtility.FromJson<PlayerProfileData>(dataToLoad);
			}
			catch (Exception e)
			{
				Debug.LogError($"Error trying to load data from file in: {fullPath}\n {e}");
			}
		}

		return data;
	}

	// saves save file to directory
	public void Save(PlayerProfileData data)
	{
		var fullPath = Path.Combine(dataDirPath, dataFileName);
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			var dataToStore = JsonUtility.ToJson(data, true);

			if (useEncryption)
				dataToStore = EncryptDecrypt(dataToStore);

			using FileStream stream = new FileStream(fullPath, FileMode.Create);
			using StreamWriter writer = new StreamWriter(stream);
			writer.Write(dataToStore);
		}
		catch (Exception e)
		{
			Debug.LogError($"Error trying to save data to file in: {fullPath}\n {e}");
		}
	}

	// encrypt & decrypt data using encryption code word
	private string EncryptDecrypt(string data)
	{
		string modifiedData = "";

		for (int i = 0; i < data.Length; i++)
			modifiedData += (char)(data[i] ^ encryptionCode[i % encryptionCode.Length]);

		return modifiedData;
	}
}
