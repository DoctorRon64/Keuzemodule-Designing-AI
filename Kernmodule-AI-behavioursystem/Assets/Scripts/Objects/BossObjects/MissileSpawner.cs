using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
	[SerializeField] private GameObject missilePrefab;
	[SerializeField] private float spawnInterval = 3f;
	private Transform player;
	private Blackboard blackboard;

	private void Start()
	{
		InvokeRepeating(nameof(SpawnMissile), 0f, spawnInterval);
	}

	public void SetupBlackBoard(Blackboard _blackboard)
	{
		blackboard = _blackboard;
		PlayerSetup();
	}

	private void PlayerSetup()
	{
		player = blackboard.GetVariable<Transform>(VariableNames.PlayerTransform);

		if (player == null)
		{
			Debug.LogError("Player transform not found in the blackboard.");
		}
	}

	private void SpawnMissile()
	{
		if (gameObject.activeSelf && gameObject.activeInHierarchy)
		{
			if (missilePrefab != null && player != null)
			{
				GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
				MissileController missileScript = missile.GetComponent<MissileController>();
				if (missileScript != null) { missileScript.Player = player; }
			}
		}
	}
}