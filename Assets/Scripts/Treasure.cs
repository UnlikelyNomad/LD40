using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour {

    public float value;

    float maxSize = 2.5f;

    public void setValue(float amount) {
        value = amount;

        amount /= maxSize;
        if (amount > 1) amount = 1;
        amount = Mathf.Sqrt(amount) * maxSize;

        transform.localScale = new Vector3(amount, amount, amount);
    }

    /*void OnCollisionEnter2D(Collision2D col) {
        GameController gc = GameController.Instance;
        gc.collectGold(value, col.gameObject.tag.CompareTo("Player") == 0);
        Destroy(gameObject);
    }*/

    void OnTriggerEnter2D(Collider2D col) {
        GameController gc = GameController.Instance;
        gc.collectGold(value, col.gameObject.tag.CompareTo("Player") == 0);
        Destroy(gameObject);
    }
}
