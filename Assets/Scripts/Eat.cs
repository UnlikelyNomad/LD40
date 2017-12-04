using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour {

    public bool canEat = false;
    public float eatDuration = 0.5f;

    void OnTriggerEnter2D(Collider2D col) {
        if (canEat) {
            canEat = false;
            GameObject other = col.gameObject;
            Destroy(other);

            GameController.Instance.Eat();
            gameObject.SetActive(false);
        }
    }

    IEnumerator stopEating() {
        yield return new WaitForSeconds(eatDuration);

        gameObject.SetActive(false);
    }

    void OnEnable() {
        canEat = true;
        StartCoroutine(stopEating());
    }

    void OnDisable() {
        canEat = false;
    }
}
