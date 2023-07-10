using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Referencable("OnDeath")]
public class Bullet : MonoBehaviour, IReferencable
{
    public float damage = 10f;
    public float speed = 20f;
    public float maximumLifetime = 10f;
    private bool _canCollide = true;
    public float collisionDisableDuration = 0.1f;
    public Rigidbody rbody;
    private Coroutine _collisionDisableCoroutine;
    private Coroutine _bulletDissappearCoroutine;
    private Vector3 _direction = Vector3.zero;
    public void Start() {
        rbody = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        if (_collisionDisableCoroutine != null) {
            StopCoroutine(_collisionDisableCoroutine);
        }
        if (_bulletDissappearCoroutine != null) {
            StopCoroutine(_bulletDissappearCoroutine);
        }
        _collisionDisableCoroutine = StartCoroutine(DisableCollisionsForDuration());
        _bulletDissappearCoroutine = StartCoroutine(DissappearAfterDuration());
    }
    private IEnumerator DisableCollisionsForDuration()
    {
        // Disable collisions
        _canCollide = false;

        // Wait for the specified duration
        yield return new WaitForSeconds(collisionDisableDuration);

        // Enable collisions
        _canCollide = true;
    }    
    private IEnumerator DissappearAfterDuration()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(maximumLifetime);
        SelfDestruct();
    }
    public void Update() {
        transform.Translate(_direction * (speed * Time.deltaTime), Space.World); 
    }

    private void OnCollisionEnter(Collision other) {
        if (!_canCollide) {
            return;
        }
        Health otherHealth;
        if (other.gameObject.TryGetComponent(out otherHealth)) {
            otherHealth.applyDamage(damage);
        }

        SelfDestruct();
    }
    public void SelfDestruct() {
        ObjectPool.SharedInstance.Digest(gameObject);
    }

    public void Invoke(string refTag) {
        if (refTag == "OnDeath") {
            SelfDestruct();
        }
    }

    public void Shoot(float baseDamage, float fireForce, Vector3 direction) {
        damage = baseDamage;
        speed = fireForce;
        _direction = direction;
    }
}
