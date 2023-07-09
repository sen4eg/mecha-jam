using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Referencable("OnDeath")]
public class Bullet : MonoBehaviour, IReferencable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Invoke(string refTag) {
        if (refTag == "OnDeath") {
            ObjectPool.SharedInstance.Digest(gameObject);
        }
    }
}
