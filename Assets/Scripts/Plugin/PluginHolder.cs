using UnityEngine;

public class PluginHolder : MonoBehaviour
{
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
}
