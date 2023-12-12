using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
	[SerializeField] private GameObject missilePrefab;
	[SerializeField] private float spawnInterval = 3f;
	private Transform player;
	private Blackboard blackboard;

	private void Start()
	{
		player = blackboard.GetVariable<Transform>(VariableNames.PlayerTransform);

		if (player == null)
		{
			Debug.LogError("Player transform not found in the blackboard.");
		}
		else
		{
			InvokeRepeating(nameof(SpawnMissile), 0f, spawnInterval);
		}
	}

	public void SetupBlackBoard(Blackboard _blackboard)
	{
		blackboard = _blackboard;
	}

	private void SpawnMissile()
	{
		if (missilePrefab != null && player != null)
		{
			GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
			MissilleController missileScript = missile.GetComponent<MissilleController>();
			Debug.Log(missileScript);
			if (missileScript != null) { missileScript.Player = player; }
		}
	}
}