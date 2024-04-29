using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Player player;
	[SerializeField] private Boss boss;
	
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
