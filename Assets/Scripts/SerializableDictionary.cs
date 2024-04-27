using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	// keys & values of the dictionary
	[SerializeField] private List<TKey> keys = new List<TKey>();
	[SerializeField] private List<TValue> values = new List<TValue>();

	public void OnAfterDeserialize()
	{
		this.Clear();

		if (keys.Count != values.Count)
		{
			Debug.LogError($"Error trying to deserialize. Keys and Values count don't match. Values respectively are: {keys.Count} and {values.Count}");
			return;
		}

		for (int i = 0; i < keys.Count; i++)
			this.Add(keys[i], values[i]);
	}

	public void OnBeforeSerialize()
	{
		keys.Clear();
		values.Clear();

		foreach (var pair in this)
		{
			keys.Add(pair.Key);
			values.Add(pair.Value);
		}
	}
}
