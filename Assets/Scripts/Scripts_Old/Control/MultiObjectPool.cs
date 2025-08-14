using System.Collections.Generic;
using UnityEngine;

public class MultiObjectPool : MonoBehaviour
{
    public GameObject[] prefabs;  // Array of prefabs to pool
    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    void Start()
    {
        // Initialize pools for each prefab
        foreach (var prefab in prefabs)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            pools.Add(prefab, pool);

            // Prepopulate the pool with inactive objects
            for (int i = 0; i < 10; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }
    }

    // Get an object from the pool for a specific prefab
    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (pools.ContainsKey(prefab))
        {
            Queue<GameObject> pool = pools[prefab];

            // Check if there is an object available in the pool
            if (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                obj.transform.SetPositionAndRotation(position, rotation);
                obj.SetActive(true);
                return obj;
            }

            // If no inactive object, instantiate a new one (optional, can expand the pool)
            GameObject newObj = Instantiate(prefab, position, rotation);
            return newObj;
        }

        return null;  // If no pool exists for the prefab
    }

    // Return an object to the pool
    public void ReturnToPool(GameObject prefab, GameObject obj)
    {
        if (pools.ContainsKey(prefab))
        {
            obj.SetActive(false);
            pools[prefab].Enqueue(obj);
        }
        else
        {
            Destroy(obj);  // If no pool for this object, destroy it
        }
    }
}