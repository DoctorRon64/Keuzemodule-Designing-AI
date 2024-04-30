using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour, IPoolable
{
    private Rigidbody2D rb;
    private ObjectPool<Bullet> objectPool;
    public bool Active { get; set; }
    private readonly int damageValue = 1;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator anim;

    public void SetupBullet(ObjectPool<Bullet> _pool)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        objectPool = _pool;
    }

    public void SetDirection(Vector2 _direction, float _speed)
    {
        rb.velocity = _direction.normalized * _speed;
    }

    public void SetRotation(Vector2 _direction)
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.TryGetComponent(out IDamagableBoss damagableBoss))
        {
            damagableBoss.TakeDamage(damageValue);
        }

        if (!_other.gameObject.TryGetComponent(out IShootable _shootable) &&
            !_other.gameObject.TryGetComponent<Iwallable>(out Iwallable _wallable)) return;

        
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        anim.SetInteger("shoot", 1);

        StartCoroutine(WaitUntilAnimationFinished());
    }

    IEnumerator WaitUntilAnimationFinished()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetInteger("shoot", 0);
        objectPool.DeactivateItem(this);
    }

    public void SetPosition(Vector2 _position)
    {
        transform.position = _position;
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
}
