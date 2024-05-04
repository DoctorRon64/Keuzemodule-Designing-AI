using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private PlayerHealth player;
	private Boss boss;

	private void Awake()
	{
		player = FindObjectOfType<PlayerHealth>();
		boss = FindObjectOfType<Boss>();
	}

	private void OnEnable()
	{
		player.OnPlayerDied += GameOver;
		boss.OnBossDied += GameOver;
	}

	private void OnDisable()
	{
		player.OnPlayerDied -= GameOver;
		boss.OnBossDied -= GameOver;
	}

	private void GameOver(int _sceneChange)
	{
		//sceneCHange
		SceneManager.LoadScene(_sceneChange);
	}
}
