using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Player player;

	private void OnEnable()
	{
		player.onPlayerDied += GameOver;
	}

	private void OnDisable()
	{
		player.onPlayerDied -= GameOver;
	}

	private void GameOver(int _sceneChange)
	{
		//sceneCHange
		SceneManager.LoadScene(_sceneChange);
	}
}
