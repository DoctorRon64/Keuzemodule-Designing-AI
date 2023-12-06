using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider bossBar;
    [SerializeField] private Slider playerHealthBar;
	[SerializeField] private BossAgent bossAgent;
	[SerializeField] private Player player;

	private void Awake()
	{
		playerHealthBar.maxValue = player.MaxHealth;
		bossBar.maxValue = bossAgent.MaxHealth;
		bossAgent.onHealthChanged += UpdateBossBar;
		player.onHealthChanged += UpdatePlayerHealthBar;
	}

	private void OnDisable()
	{
		bossAgent.onHealthChanged -= UpdateBossBar;
		player.onHealthChanged -= UpdateBossBar;
	}

	private void Update()
	{
		playerHealthBar.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, 0);
	}

	private void UpdatePlayerHealthBar(int _newHealth)
	{
		playerHealthBar.value = _newHealth;
	}

	private void UpdateBossBar(int _newHealth)
	{
		bossBar.value = _newHealth;
	}
}
