using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
	private Rigidbody2D rb;

	public bool Active { get; set; }

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void SetPosition(Vector2 position)
	{
		transform.position = position;
	}

	public void DisablePoolabe()
	{
		gameObject.SetActive(false);
		rb.velocity = Vector2.zero;
	}

	public void EnablePoolabe()
	{
		gameObject.SetActive(true);
		rb.velocity = Vector2.right * 10f;
	}
}
