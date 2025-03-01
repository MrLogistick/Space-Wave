using System.Collections;
using UnityEngine;

public class SonarBlast : MonoBehaviour
{
    [SerializeField] Vector2 cutoffSize;
    [SerializeField] float rotationSpeed;
    [SerializeField] float sonarDuration = 2f;
    float timeElapsed = 0f;

    void Start() {
        rotationSpeed *= Random.value < 0.5 ? -1 : 1;
    }

    void Update() {
        if (timeElapsed < sonarDuration)
        {
            Vector2 newScale = Vector2.Lerp(new Vector2(0, 0), cutoffSize, timeElapsed / sonarDuration);
            transform.localScale = Vector2.Scale(Vector2.one, newScale);

            transform.Rotate(Vector3.forward * rotationSpeed * GameState.instance.slowDown * Time.deltaTime);

            timeElapsed += Time.deltaTime;
        }
        else
        {
            transform.localScale = Vector3.zero;
            StartCoroutine(DisableDelay());
        }
    }

    IEnumerator DisableDelay() {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}