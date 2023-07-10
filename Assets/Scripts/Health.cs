using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Health : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public bool isDead = false;
    public bool isInvincible = false;
    private List<IReferencable> _referencables;
    private void Start() {
        _referencables = ReferenceUtils.GetReferencablesWithTag(gameObject, "OnDeath"); 
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            isDead = true;
            if (_referencables.Count > 0)
            {
                foreach (IReferencable referencable in _referencables)
                {
                    referencable.Invoke("OnDeath");
                }
            }
            else {
                Destroy(gameObject);
            }
        }
    }
    public void applyDamage(float damage)
    {
        if (!isInvincible)
        {
            health -= damage;
        }
    }
}
