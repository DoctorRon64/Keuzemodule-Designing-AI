using System;
using UnityEngine;
using System.Collections;

public class BossBullets : BossProjectile<BossBullets>
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator anim;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void SetDirection(Vector2 _direction, float _speed)
    {
        Rb.velocity = _direction.normalized * _speed;
    }

    public override void SetRotation(Vector2 _direction)
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(DamageValue);
        }

        if (!_other.gameObject.TryGetComponent<IObstacle>(out IObstacle _wallable)) return;
        
        Rb.velocity = Vector2.zero;
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        anim.Play("smokebullet");

        StartCoroutine(WaitUntilAnimationFinished());
    }

    IEnumerator WaitUntilAnimationFinished()
    {
        yield return new WaitUntil(() => !anim.GetCurrentAnimatorStateInfo(0).IsName("shoot")); //stopped playing
        yield return new WaitForSeconds(0.2f);
        ObjectPool.DeactivateItem(this);
    }

    public void SetPosition(Vector2 _position)
    {
        transform.position = _position;
    }

    public void DisablePoolable()
    {
        Rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
        if (spriteRenderer != null) spriteRenderer.enabled = true;
    }

    public void EnablePoolable()
    {
        SetDirection(Vector2.left, 10f);
        gameObject.SetActive(true);
    }
}
