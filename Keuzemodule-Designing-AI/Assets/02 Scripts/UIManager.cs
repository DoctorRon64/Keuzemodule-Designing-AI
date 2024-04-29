using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider playerHealthBar;
	[SerializeField] private Player player;
	private void Awake()
	{
		playerHealthBar.maxValue = player.maxHealth;
		playerHealthBar.value = player.maxHealth;
		player.onHealthChanged += UpdatePlayerHealthBar;
	}
	
	private void OnDisable()
	{
		player.onHealthChanged -= UpdatePlayerHealthBar;
	}

	private void Update()
	{
		playerHealthBar.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, 0);
	}

	private void UpdatePlayerHealthBar(int _newHealth)
	{
		playerHealthBar.value = _newHealth;
	}
}
