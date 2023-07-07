using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab;
    public int minBoxes = 5;
    public int maxBoxes = 10;
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float minAngularVelocity = 15f;
    public float maxAngularVelocity = 50f;
    public float boxSpawnRadius = 5f;

    void Start()
    {
        int numBoxes = Random.Range(minBoxes, maxBoxes + 1);

        for (int i = 0; i < numBoxes; i++)
        {
            Vector3 position = transform.position + new Vector3(Random.Range(-boxSpawnRadius, boxSpawnRadius), Random.Range(1f, 5f), Random.Range(-boxSpawnRadius, boxSpawnRadius));
            GameObject box = Instantiate(boxPrefab, position, Quaternion.identity);

            float size = Random.Range(minSize, maxSize);
            box.transform.localScale = new Vector3(size, size, size);

            Rigidbody rigidbody = box.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                float angularVelocity = Random.Range(minAngularVelocity, maxAngularVelocity);
                rigidbody.angularVelocity = Random.insideUnitSphere * angularVelocity;
                
                rigidbody.angularDrag = 0f; // Set angular drag to zero
            }
        }
    }
}
