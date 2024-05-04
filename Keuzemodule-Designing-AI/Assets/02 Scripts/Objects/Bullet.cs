using System;
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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
            !_other.gameObject.TryGetComponent<IObstacle>(out IObstacle _wallable)) return;
        
        rb.velocity = Vector2.zero;
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        anim.Play("smokebullet");

        StartCoroutine(WaitUntilAnimationFinished());
    }

    IEnumerator WaitUntilAnimationFinished()
    {
        yield return new WaitUntil(() => !anim.GetCurrentAnimatorStateInfo(0).IsName("shoot")); //stopped playing
        yield return new WaitForSeconds(0.2f);
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
        if (spriteRenderer != null) spriteRenderer.enabled = true;
    }

    public void EnablePoolable()
    {
        SetDirection(Vector2.left, 10f);
        gameObject.SetActive(true);
    }
}
