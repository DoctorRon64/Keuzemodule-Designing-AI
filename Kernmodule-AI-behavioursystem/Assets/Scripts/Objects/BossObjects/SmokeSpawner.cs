using UnityEngine;

public class SmokeSpawner : MonoBehaviour
{
	[SerializeField] private GameObject missilePrefab;
	[SerializeField] private float spawnInterval = 2f;
	[SerializeField] private float spawnXMin = -5f;
	[SerializeField] private float spawnXMax = 5f;
	[SerializeField] private float spawnY = 5f;

	private void Start()
	{
		//super handige monobehaviour method ik las dat er minder null calls waren inplaats van void Update
		InvokeRepeating(nameof(SpawnSmoke), 0f, spawnInterval);
	}

	private void SpawnSmoke()
	{
		if (gameObject.activeSelf && gameObject.activeInHierarchy)
		{
			float randomX = Random.Range(spawnXMin, spawnXMax);
			Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);
			Instantiate(missilePrefab, spawnPosition, Quaternion.identity);
		}
	}
}
