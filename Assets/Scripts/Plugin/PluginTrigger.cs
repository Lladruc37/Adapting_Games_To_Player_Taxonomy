using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluginData/PluginTrigger")]
public class PluginTrigger : ScriptableObject
{
	// what triggers the modifications
	public PluginTriggerType triggerType;

	// what player types are affected
	public List<PlayerTypes> playerTypes;

	// how are player types affected
	public List<float> modifications;
}