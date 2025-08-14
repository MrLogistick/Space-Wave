using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public Vector2 rotationSpeedRange;
    public int objID;

    [HideInInspector] public AsteroidGenerator generator;
    [HideInInspector] public MultiObjectPool pool;
    [HideInInspector] public GameObject[] prefabs;

    void Awake() {
        generator = FindFirstObjectByType<AsteroidGenerator>();
        pool = FindFirstObjectByType<MultiObjectPool>();

        prefabs = pool.prefabs;

        if (speed > maxSpeed) {
            speed -= speed - maxSpeed;
        }
    }

    public void DisableObject(GameObject prefab, GameObject obj) {
        pool.ReturnToPool(prefab, obj);
    }
}