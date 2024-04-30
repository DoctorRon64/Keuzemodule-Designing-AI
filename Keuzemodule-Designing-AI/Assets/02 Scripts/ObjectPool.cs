using System.Collections.Generic;
using UnityEngine;
public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
	private readonly List<IPoolable> activePool = new List<IPoolable>();
	private readonly List<IPoolable> inactivePool = new List<IPoolable>();
	private readonly List<T> prefabs;

	public ObjectPool(T _prefab)
	{
		prefabs = new List<T>
		{
			_prefab
		};
	}
	
	public ObjectPool(List<T> _prefabs)
	{
		prefabs = _prefabs;
	}

	public IPoolable RequestObject(Vector2 _position)
	{
		if (inactivePool.Count <= 0)
		{
			Debug.LogError("No More Inactive Pool Items. Increase Pool Size");
			return null;
		}
		else
		{
			IPoolable currentPool = inactivePool[0];
			currentPool.SetPosition(_position);
			ActivateItem(currentPool);
			return currentPool;
		}
	}

	public IPoolable AddNewItemToPool()
	{
		T prefab = prefabs[Random.Range(0, prefabs.Count)];
		T instance = Object.Instantiate(prefab);
		instance.gameObject.SetActive(false);
		inactivePool.Add(instance);
		return instance;
	}

	private IPoolable ActivateItem(IPoolable _item)
	{
		_item.EnablePoolable();
		_item.Active = true;
		int index = inactivePool.IndexOf(_item);
		if (index != -1)
		{
			inactivePool.RemoveAt(index);
		}
		activePool.Add(_item);
		return _item;
	}

	public void DeactivateItem(IPoolable _item)
	{
		int index = activePool.IndexOf(_item);
		if (index != -1)
		{
			activePool.RemoveAt(index);
		}
		_item.DisablePoolable();
		_item.Active = false;
		inactivePool.Add(_item);
	}
}
