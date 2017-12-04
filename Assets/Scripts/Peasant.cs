using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasant : MonoBehaviour {

    public bool runAway;
    public float runSpeed;
    public float zDepth;
    public float houseHeading = 90f;
    public bool left = false;

    public GameObject player;

    float villageSize;

    Vector2 vel;

    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        Vector3 heading = new Vector3(0, houseHeading, 0);
        transform.rotation = Quaternion.Euler(heading);
        rb = GetComponent<Rigidbody2D>();
        runAway = false;
        player = GameObject.FindGameObjectWithTag("Player");
        villageSize = GameController.Instance.village.villageSize;
        GetComponent<CapsuleCollider2D>().enabled = false;
	}
	
	// Update is called once per frame
    void Update () {
        GetComponent<CapsuleCollider2D>().enabled = true;
        float x = transform.position.x;

        if (x < -25f || x > villageSize + 25f) {
            Destroy(gameObject);
        }
	}

    void FixedUpdate() {
        if (runAway) {
            rb.velocity = vel;
        } else {
            //run towards camera
            Vector3 pos = transform.position;
            pos.z -= runSpeed * Time.fixedDeltaTime;

            if (pos.z < zDepth) {
                runAway = true;

                pos.z = zDepth;

                float runHeading = 0f;

                if (player.transform.position.x > transform.position.x) {
                    runHeading = 180f;
                    left = true;
                    GetComponent<CapsuleCollider2D>().enabled = true;
                }

                transform.rotation = Quaternion.Euler(0, runHeading, 0);

                float v = runSpeed;
                if (left) {
                    v *= -1;
                }

                vel = new Vector2(v, 0);
                rb.velocity = Vector2.zero;
            }

            transform.position = pos;
        }
    }
}
