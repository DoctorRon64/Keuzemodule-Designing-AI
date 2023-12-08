using UnityEngine;

public class MissleSpawner : MonoBehaviour
{
	[SerializeField] private GameObject missilePrefab;
    [SerializeField] private float spawnInterval = 3f;
    private Transform player;
	private Blackboard blackboard;
	private float timeSinceLastSpawn;

	private void Start()
	{
		timeSinceLastSpawn = spawnInterval;
	}

	private void Update()
	{
		timeSinceLastSpawn += Time.deltaTime;

		if (timeSinceLastSpawn >= spawnInterval)
		{
			SpawnMissile();
			timeSinceLastSpawn = 0f;
		}
	}

	public void SetupBlackBoard(Blackboard _blackboard)
	{
		blackboard = _blackboard;
	}

	private void SpawnMissile()
	{
		player = blackboard.GetVariable<Transform>(VariableNames.PlayerTransform);
		if (missilePrefab != null && player != null)
		{
			GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);

			MissileController missileController = missile.GetComponent<MissileController>();
			if (missileController != null)
			{
				missileController.Player = player;
			}
		}
	}
}