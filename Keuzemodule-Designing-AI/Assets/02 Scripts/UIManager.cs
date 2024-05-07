using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider bossBar;
    [SerializeField] private Slider hatBar;
    
	private PlayerHealth player;
	private Boss boss;
	private BossHat hat;
	
	private void Awake()
	{
		player = FindObjectOfType<PlayerHealth>();
		boss = FindObjectOfType<Boss>();
		hat = FindObjectOfType<BossHat>();

		hatBar.maxValue = hat.maxHealth;
		hatBar.value = hat.Health;
		hat.OnHealthChanged += UpdateHatHealthBar;
		boss.OnHatActive += UpdateHatBarVisible;
		
		bossBar.maxValue = boss.maxHealth;
		bossBar.value = boss.maxHealth;
		boss.OnHealthChanged += UpdateBossBar;
		
		playerHealthBar.maxValue = player.maxHealth;
		playerHealthBar.value = player.maxHealth;
		player.OnHealthChanged += UpdatePlayerHealthBar;
	}
	
	private void OnDisable()
	{
		player.OnHealthChanged -= UpdatePlayerHealthBar;
		boss.OnHealthChanged -= UpdateBossBar;
		hat.OnHealthChanged -= UpdateHatHealthBar;
		boss.OnHatActive -= UpdateHatBarVisible;
	}

	private void Update()
	{
		playerHealthBar.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, 0);
		if (hatBar == null) return;
		hatBar.transform.position = new Vector3(hat.transform.position.x, hat.transform.position.y + 1, 0);
	}

	private void UpdateHatBarVisible(bool _newValue)
	{
		hatBar.gameObject.SetActive(_newValue);
	}

	private void UpdateHatHealthBar(int _newHealth)
	{
		hatBar.value = _newHealth;
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
