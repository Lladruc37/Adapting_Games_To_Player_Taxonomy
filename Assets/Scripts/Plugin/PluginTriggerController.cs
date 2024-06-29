using System.Collections.Generic;
using UnityEngine;

public enum PluginTriggerType
{
	ROOM_CLEAR,
	ROOM_CLEAR_NO_DMG,
	ENEMY_DEATH,
	SECRET_ROOM_ENTER,
	SHOP_ENTER,
	SHOP_SKIP,
	WEAPON_ACQUIRE,
	ENVIRONMENT_BREAK,
	MONEY_USED,
	MONEY_EARNED
}

public class PluginTriggerController : MonoBehaviour
{
	// singleton instance
	public static PluginTriggerController Instance { get; private set; }
	public static bool Initialized = false;

	public bool CanEditDuringPlayMode = true;
	public List<PluginTrigger> triggers;

	private void Awake()
	{
		Initialized = true;
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void OnPluginTrigger(PluginTriggerType type, int amount = 1)
	{
		var trigger = triggers.Find(x => x.triggerType == type);
		if (trigger != null)
		{
			if (trigger.playerTypes.Count != trigger.modifications.Count) return;

			var changes = new Dictionary<PlayerTypes, float>();
			for (var i = 0; i < trigger.playerTypes.Count; i++)
			{
				var playerType = trigger.playerTypes[i];
				var mod = trigger.modifications[i] * amount;
				changes.Add(playerType, mod);
			}

			PluginController.Instance.PlayerTypeTrigger(changes);
		}
	}
}
