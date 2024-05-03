using System;
using UnityEngine;

public class BossFollowingRocket : BossProjectile<BossFollowingRocket>, IDamagableBoss
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private int damage;
        [SerializeField] private float followDelay = 2f;
        private Transform player = null;
        private bool isFollowing = false;
        private float followTimer = 0f;

        private void Awake()
        {
            player = FindObjectOfType<Player>().transform;
        }

        private void Update()
        {
            if (player == null) return;
            if (!isFollowing)
            {
                transform.Translate(Vector2.up * speed * Time.deltaTime);
                followTimer += Time.deltaTime;
                if (followTimer >= followDelay)
                {
                    isFollowing = true;
                }
            }
            else
            {

                Vector3 direction = (player.position - transform.position).normalized;
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

        public override void DisablePoolable()
        {
            base.DisablePoolable();
            isFollowing = false;
        }

        public int Health { get; set; }
        public void TakeDamage(int _damageAmount)
        {
            Health -= _damageAmount;
            if (Health >= 0) return;
            DisablePoolable();
            ObjectPool.DeactivateItem(this);
        }
    }