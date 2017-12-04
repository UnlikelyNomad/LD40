using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gerblin : MonoBehaviour {

    public float zLimit;

    public float runSpeed = 8f;

    public ParticleSystem ps;

    Rigidbody2D rb;

    public Animator anim;

    int grabHash;
    int grabbedHash;

    public bool coin = false;
    public bool leaving = false;

    Vector2 vel;

    VillageGeneration village;

    void Start() {
        rb = GetComponent<Rigidbody2D>();

        grabHash = Animator.StringToHash("Grab");
        grabbedHash = Animator.StringToHash("Base Layer.CarryCoin");

        float v = runSpeed;
        if (transform.position.x >  0) {
            //right side of town

            transform.rotation = Quaternion.Euler(0, 180, 0);
            v *= -1;
        } else {
            //left side of town
        }

        vel = new Vector2(v, 0);
        Vector3 pos = transform.position;
        pos.z = -6.4f;
        transform.position = pos;

        village = GameController.Instance.village;
    }

    void Update() {
        if (coin) {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
            if (info.fullPathHash == grabbedHash) {
                leaving = true;
            }
        }
    }

    void FixedUpdate() {
        if (!coin) {
            rb.velocity = vel;
            if (transform.position.x < (GameController.Instance.villageLimit * -2f) ||
                transform.position.x > (village.villageSize + (GameController.Instance.villageLimit * 2f))) {
                GameController.Instance.gerblins++;
                Destroy(gameObject);
            }
        } else if (leaving) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            Vector3 pos = transform.position;

            pos.z -= runSpeed * Time.fixedDeltaTime;

            transform.position = pos;

            if (pos.z < zLimit) {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        //pickup coin - coin is a trigger
        rb.velocity = Vector2.zero;

        anim.SetTrigger(grabHash);
        GetComponent<CapsuleCollider2D>().enabled = false;
        coin = true;
    }
}
