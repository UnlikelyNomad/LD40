using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerController : MonoBehaviour {

    public float flapImpulse;
    public float directionFactor;

    public bool mouseTurn = true;

    public float fireCooldown = 3f;

    float flapForce;
    float sideForce;

    float fireTime;

    bool left;

    Vector3 scale;

    bool flap;

    public float mouseX;

    public SpriteRenderer sr;

    public GameObject projectile;
    public Vector2 fireVelocity;

    public Transform projectileEffector;
    public Transform projectileParent;

    public Animator anim;

    int flapHash;
    int idleHash;

    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        scale = Vector3.one;

        flapHash = Animator.StringToHash("Flap");
        idleHash = Animator.StringToHash("Base Layer.Idle");
	}
	
	// Update is called once per frame
    void Update () {

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if (info.fullPathHash == idleHash) {
            if (Input.GetButton("FlapUp")) {
                anim.SetTrigger(flapHash);
                flapForce = flapImpulse;
            } else if (Input.GetButton("FlapLeft")) {
                anim.SetTrigger(flapHash);
                scale.x = -1;
                left = true;
                transform.localScale = scale;
                flapForce = flapImpulse;
                sideForce = -1 * directionFactor;
            } else if (Input.GetButton("FlapRight")) {
                anim.SetTrigger(flapHash);
                scale.x = 1;
                left = false;
                transform.localScale = scale;
                flapForce = flapImpulse;
                sideForce = directionFactor;
            }
        }

        if (fireTime <= 0) {
            if (Input.GetButton("Fire")) {
                fireTime = fireCooldown;
                GameObject p = (GameObject)Instantiate(projectile, projectileEffector.position, Quaternion.identity, projectileParent);
                float vel = fireVelocity.x;
                if (left) vel *= -1;

                Vector2 pV = new Vector2(vel, fireVelocity.y) + rb.velocity;
                p.GetComponent<Rigidbody2D>().velocity = pV;
            }
        } else {
            fireTime -= Time.deltaTime;
        }
	}

    void FixedUpdate() {
        if (flapForce > 0.01) {
            rb.AddForce(new Vector2(sideForce, flapForce));
            flapForce *= 0.5f;
            sideForce *= 0.5f;
        }
    }
}
