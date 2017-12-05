using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRunner : MonoBehaviour {

    public bool left = false;
    public float runSpeed;

    public Animator anim;

    void FixedUpdate() {
        if (transform.position.x < -25f || transform.position.x > 25f) {
            Destroy(gameObject);
        }

        float x = runSpeed * Time.fixedDeltaTime;
        if (!left) x = -x;

        Vector3 pos = transform.position;
        pos.x += x;

        transform.position = pos;
    }
}
