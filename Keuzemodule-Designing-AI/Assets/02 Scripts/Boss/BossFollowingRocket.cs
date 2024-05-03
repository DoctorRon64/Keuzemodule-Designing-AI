using System;
using System.Collections;
using UnityEngine;

public class BossFollowingRocket : BossProjectile<BossFollowingRocket>, IDamagableBoss
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float followDelay = 2f;
        [SerializeField] private int health = 5;
        
        private Transform player = null;
        private bool isFollowing = false;
        private float followTimer = 0f;

        private void Awake()
        {
            Health = health;
            player = FindObjectOfType<Player>().transform;
        }

        private void Start()
        {
            StartCoroutine(StartFollowingDelay());
        }

        private IEnumerator StartFollowingDelay()
        {
            Vector2 initialDirection = Vector2.up * speed * Time.deltaTime;
            SetDirection(initialDirection, speed);
            SetRotation(initialDirection);
        
            yield return new WaitForSeconds(followDelay);
            isFollowing = true;
        }

        private void Update()
        {
            if (player == null || !isFollowing) return;

            Vector3 direction = (player.position - transform.position).normalized;
            SetDirection(direction, speed);
            SetRotation(direction);
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

        public override void DisablePoolable()
        {
            base.DisablePoolable();
            isFollowing = false;
        }

        protected override void OnTriggerEnter2D(Collider2D _other)
        {
            if (_other.gameObject.TryGetComponent(out IDamagable idamagable))
            {
                idamagable.TakeDamage(DamageValue);
            }
        }
        
        public int Health { get; set; }
        public void TakeDamage(int _damageAmount)
        {
            Health -= _damageAmount;
            if (Health > 0) return;
            DisablePoolable();
            ObjectPool.DeactivateItem(this);
        }
    }