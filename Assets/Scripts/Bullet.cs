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
    private Coroutine collisionDisableCoroutine;
    private Coroutine bulletDissappearCoroutine;

    public void Start() {
        rbody = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        if (collisionDisableCoroutine != null) {
            StopCoroutine(collisionDisableCoroutine);
        }
        if (bulletDissappearCoroutine != null) {
            StopCoroutine(bulletDissappearCoroutine);
        }
        collisionDisableCoroutine = StartCoroutine(DisableCollisionsForDuration());
        bulletDissappearCoroutine = StartCoroutine(DissappearAfterDuration());
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
        transform.Translate(Vector3.up * speed * Time.deltaTime); 
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
}
