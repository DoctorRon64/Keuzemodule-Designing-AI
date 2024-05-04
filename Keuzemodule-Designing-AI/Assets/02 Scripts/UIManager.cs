using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider bossBar;
    
	private PlayerHealth player;
	private Boss boss;
	
	private void Awake()
	{
		player = FindObjectOfType<PlayerHealth>();
		boss = FindObjectOfType<Boss>();

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
