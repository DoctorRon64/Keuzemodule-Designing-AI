using System;
using UnityEngine;

public class BossFollowingRocket : BossProjectile<BossFollowingRocket>, IDamagableBoss
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private int damage;
        private Transform Player = null;

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
            
        }

        public override void SetRotation(Vector2 _direction)
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public int Health { get; set; }
        public void TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
            if (Health < 0)
            {
                DisablePoolable();
                objectPool.DeactivateItem(this);
            }
        }
    }