using UnityEngine;

public interface IPoolable
{
	bool Active { get; set; }
	void DisablePoolable();
	void EnablePoolable();
	void SetPosition(Vector2 _pos);
}
