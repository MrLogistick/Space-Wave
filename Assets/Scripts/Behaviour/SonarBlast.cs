using System.Collections;
using UnityEngine;

public class SonarBlast : MonoBehaviour
{
    [SerializeField] float sonarSpeed;
    [SerializeField] float sonarSizeRate;

    IEnumerator Start() {
        transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(5f);

        if (GameState.instance.isDead) yield break;
        
        Destroy(gameObject);
    }

    void Update() {
        transform.position += Vector3.right * sonarSpeed * GameState.instance.slowDown * Time.deltaTime;
        transform.localScale += Vector3.one * sonarSizeRate * GameState.instance.slowDown * Time.deltaTime;
    }
}
