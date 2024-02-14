using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Player player;
	[SerializeField] private BossAgent bossAgent;

	private void OnEnable()
	{
		bossAgent.onBossDied += GameOver;
		player.onPlayerDied += GameOver;
	}

	private void OnDisable()
	{
		bossAgent.onBossDied -= GameOver;
		player.onPlayerDied -= GameOver;
	}

	private void GameOver(int _sceneChange)
	{
		//sceneCHange
		SceneManager.LoadScene(_sceneChange);
	}
}
