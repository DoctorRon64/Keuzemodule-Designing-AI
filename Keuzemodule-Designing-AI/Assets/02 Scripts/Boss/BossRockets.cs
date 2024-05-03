using UnityEngine;

public class BossRockets : BossProjectile<BossRockets>
{
    public override void SetDirection(Vector2 _direction, float _speed)
    {
        Rb.velocity = _direction.normalized * _speed;
    }

    public override void SetRotation(Vector2 _direction)
    {
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}