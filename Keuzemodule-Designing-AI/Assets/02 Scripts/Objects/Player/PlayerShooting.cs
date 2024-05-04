// Define a separate class for handling player shooting

using System;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting")] 
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float bulletSpawnDistance = 1.0f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private int bulletAmount = 10;
    private bool isShooting;
    private float nextFireTime;
    private ObjectPool<Bullet> bulletPool;
    
    private void Start()
    {
        shootingPoint.localPosition = new Vector3(bulletSpawnDistance, 0f, 0f);
        bulletPool = new ObjectPool<Bullet>(bulletPrefab.GetComponent<Bullet>());
        for (int i = 0; i < bulletAmount; i++)
        {
            Bullet bullet = (Bullet)bulletPool.AddNewItemToPool();
            bullet.SetupBullet(bulletPool);
        }
    }

    private void FixedUpdate()
    {
        if (isShooting && Time.time > nextFireTime)
        {
            ShootBullet();
        }
    }
    
    private void ShootBullet()
    {
        nextFireTime = Time.time + fireRate;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector3 shootingPointPosition = transform.position + (Vector3)direction * bulletSpawnDistance;

        shootingPointPosition = transform.position +
                                (shootingPointPosition - transform.position).normalized * bulletSpawnDistance;
        shootingPoint.position = shootingPointPosition;
        isShooting = false;
        
        Bullet bullet = bulletPool.RequestObject(shootingPoint.position) as Bullet;
        if (bullet == null) return;
        bullet.SetDirection(direction, bulletSpeed);
        bullet.SetRotation(direction);
    }
    
    public void SetIsShooting(bool shooting)
    {
        isShooting = shooting;
    }
}