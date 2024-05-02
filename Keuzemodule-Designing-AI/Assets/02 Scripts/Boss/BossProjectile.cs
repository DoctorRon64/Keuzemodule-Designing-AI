using UnityEngine;

public abstract class BossProjectile<T> : MonoBehaviour, IBossable, IPoolable where T : BossProjectile<T>
{
    protected Rigidbody2D rb;
    protected ObjectPool<T> objectPool;
    public bool Active { get; set; }
    private readonly int damageValue = 1;

    public void Setup(ObjectPool<T> _pool)
    {
        rb = GetComponent<Rigidbody2D>();
        objectPool = _pool;
    }

    public abstract void SetDirection(Vector2 _direction, float _speed);
    public abstract void SetRotation(Vector2 _direction);

    protected virtual void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.TryGetComponent(out IDamagable idamagable))
        {
            idamagable.TakeDamage(damageValue);
        }

        if (!_other.gameObject.TryGetComponent(out Iwallable shootable)) return;
        DisablePoolable();
        objectPool.DeactivateItem((T)this);
    }

    public void DisablePoolable()
    {
        rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    public void EnablePoolable()
    {
        SetDirection(Vector2.left, 10f);
        gameObject.SetActive(true);
    }

    public void SetPosition(Vector2 _position)
    {
        transform.position = _position;
    }
}