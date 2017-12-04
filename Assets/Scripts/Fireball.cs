using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
    public float damage = 1f;
    public float lifetime = 3f;
    public float minScale;
    float remaining;

    public Transform rotateObject;

    public Vector3 rotateFactor;

    Vector3 startScale;

    void Start() {
        remaining = lifetime;
        startScale = rotateObject.localScale;
    }

    void OnCollisionEnter2D(Collision2D col) {
        GameObject other = col.gameObject;
        Building bldg = other.GetComponent<Building>();

        if (bldg != null) {
            bldg.takeDamage(damage);
        } else {
            //spawn ground fire effect / check for villagers
        }

        Destroy(gameObject);
    }

    void Update() {
        remaining -= Time.deltaTime;
        if (remaining < 0) {
            Destroy(gameObject);
            return;
        }

        float s = minScale + (remaining / lifetime);
        rotateObject.localScale = startScale * s;
    }

    void FixedUpdate() {
        Vector3 r = rotateFactor;

        r += rotateObject.rotation.eulerAngles;
        rotateObject.rotation = Quaternion.Euler(r);
    }
}
