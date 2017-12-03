using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public float damage = 1f;

    void OnCollisionEnter2D(Collision2D col) {
        GameObject other = col.gameObject;

        Health h = other.GetComponent<Health>();

        if (h != null) {
            h.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
