using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        Health hlth = GetComponent<Health>();
        hlth.health = Random.Range(5, 200);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
