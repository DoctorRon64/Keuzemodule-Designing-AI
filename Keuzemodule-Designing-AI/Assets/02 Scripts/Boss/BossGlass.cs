using UnityEngine;

public class BossGlass : BossProjectile<BossGlass> 
{
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float upwardForce = 5f;
    [SerializeField] private float spinForce = 5f;
    [SerializeField] private Vector2 throwDirection = Vector2.right;
    
    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void ThrowGlass()
    {
        rb2d.isKinematic = false;
        rb2d.AddForce(transform.up * upwardForce, ForceMode2D.Impulse);
        rb2d.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        rb2d.AddTorque(Random.Range(-spinForce, spinForce), ForceMode2D.Impulse);
    }

    public override void SetDirection(Vector2 _direction, float _speed)
    {
        rb2d.velocity = _direction * _speed;
    }

    public override void SetRotation(Vector2 _direction)
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        rb2d.rotation = angle;
    }
}