using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float fireForce = 10f;
    public float baseDamage = 10f;
    public float fireTimer = 0f;
    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
    }

    public void Fire(Vector3 direction) {
        if(fireTimer > 0) {
            return;
        }
        GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
        if (bullet != null) {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = Quaternion.identity;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation *= Quaternion.Euler(90, 0, 0);
            bullet.transform.rotation = targetRotation;
            bullet.SetActive(true);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent.damage = baseDamage;
            bulletComponent.speed = fireForce;
            fireTimer = fireRate;
        }
    }
}
