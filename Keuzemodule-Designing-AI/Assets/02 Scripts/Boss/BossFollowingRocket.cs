using System;
using UnityEngine;

public class BossFollowingRocket : BossProjectile<BossFollowingRocket>
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private int damage;
        public Transform Player = null;

        private void Awake()
        {
            Player = FindObjectOfType<Player>().transform;
        }

        private void Update()
        {
            if (Player != null)
            {
                Vector3 direction = (Player.position - transform.position).normalized;
                transform.Translate(direction * speed * Time.deltaTime, Space.World);

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        public override void SetDirection(Vector2 _direction, float _speed)
        {
            rb.velocity = _direction.normalized * _speed;
        }

        public override void SetRotation(Vector2 _direction)
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<Player>(out Player _player))
            {
                _player.TakeDamage(damage);
            }
        }
    }