using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public float damage = 1f;

    void OnTriggerEnter2D(Collider2D col) {
        GameObject other = col.gameObject;

        if (other.tag.CompareTo("Player") == 0) {
            GameController.Instance.damagePlayer();
        }

        Destroy(gameObject);
    }
}
