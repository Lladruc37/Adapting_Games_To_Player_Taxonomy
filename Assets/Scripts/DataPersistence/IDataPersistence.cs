public interface IDataPersistence
{
	// load data from save file
	void LoadProfile(PlayerProfileData data);

	// save data to save file
	void SaveProfile(ref PlayerProfileData data);
}
