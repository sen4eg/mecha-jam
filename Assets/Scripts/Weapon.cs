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
    public AnimationCurve weaponShakeCurve;
    public float weaponShakeDuration = 0.5f;// should be somewhat close to fireRate
    private Coroutine weaponCoroutine = null;
    public float shakyShaky = 0.1f;
    private IEnumerator Shaking()
    {
        Vector3 originalPosition = transform.localPosition;
        float time = 0f;
        while (time < weaponShakeDuration)
        {
            time += Time.deltaTime;
            float displacement = weaponShakeCurve.Evaluate(time/weaponShakeDuration);
            transform.localPosition = originalPosition + Random.insideUnitSphere * (displacement * shakyShaky);
            yield return null;
        }
        
        transform.localPosition = originalPosition;
    }
    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
    }

    public void Fire(Vector3 direction) {
        Debug.DrawLine(firePoint.position, firePoint.position + direction * 10f, Color.red, 1f);
        if(fireTimer > 0) {
            return;
        }
        GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
        if (bullet != null) {
            bullet.transform.position = firePoint.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            targetRotation *= Quaternion.Euler(90, 0, 0);
            bullet.transform.rotation = targetRotation;
            bullet.SetActive(true);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            bulletComponent.Shoot(baseDamage, fireForce, direction);
            fireTimer = fireRate;
        }
        if (weaponCoroutine != null) {
            StopCoroutine(weaponCoroutine);
        }
        weaponCoroutine = StartCoroutine(Shaking());
    }
}
