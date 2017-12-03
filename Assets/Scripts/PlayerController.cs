using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float flapImpulse;
    public float directionFactor;

    public bool mouseTurn = true;

    public float fireCooldown = 3f;
    public float fireDelay = 0.3f;
    public float speedLimit;

    public float heightFactor = -1f;

    public float villageLimit = 25f;

    float flapForce;
    float sideForce;

    public float fireTime;

    bool left;

    Vector3 scale;

    bool flap;
    bool fire;

    public GameObject projectile;
    public Vector2 fireVelocity;

    public Transform projectileEffector;
    public Transform projectileParent;

    public Animator anim;

    int flapHash;
    int idleHash;
    int fireHash;

    public GameObject leavingText;

    Rigidbody2D rb;

    VillageGeneration village;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        scale = Vector3.one;

        flapHash = Animator.StringToHash("Flap");
        fireHash = Animator.StringToHash("Fire");
        idleHash = Animator.StringToHash("Base Layer.Idle");
        village = GameController.Instance.village;
	}
	
	// Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        float velX = rb.velocity.x;
        if (Mathf.Abs(velX) > 0.01f) {
            left = velX < 0f;
            scale.x = Mathf.Sign(velX);
            transform.localScale = scale;
        }

        if (info.fullPathHash == idleHash) {
            if (Input.GetButton("FlapLeft")) {
                anim.SetTrigger(flapHash);
                doFlap();
                //flapForce = flapImpulse;
                sideForce = -1 * directionFactor;
            } else if (Input.GetButton("FlapRight")) {
                anim.SetTrigger(flapHash);
                doFlap();
                sideForce = directionFactor;
            }
        }

        if (fireTime <= 0) {
            fire = false;
            if (Input.GetButton("Fire")) {
                anim.SetTrigger(fireHash);
                fireTime = fireCooldown;

            }
        } else {
            fireTime -= Time.deltaTime;

            if (!fire && fireCooldown - fireTime > fireDelay) {
                fire = true;
                GameObject p = (GameObject)Instantiate(projectile, projectileEffector.position, Quaternion.identity, projectileParent);
                float vel = fireVelocity.x;
                if (left) vel *= -1;

                Vector2 pV = new Vector2(vel, fireVelocity.y) + rb.velocity;
                p.GetComponent<Rigidbody2D>().velocity = pV;
            }
        }

        float x = transform.position.x;

        if (x < -villageLimit || x > village.villageSize + villageLimit) {
            leavingText.SetActive(true);
        } else {
            leavingText.SetActive(false);
        }

        if (x < (villageLimit * -2f) || x > village.villageSize + (villageLimit * 2f)) {
            GameController.Instance.LeaveRaid();
        }
	}

    void doFlap() {
        float f = flapImpulse - (heightFactor * transform.position.y);
        if (f > flapForce) flapForce = f;
    }

    void FixedUpdate() {
        if (flapForce > 0.01) {
            rb.AddForce(new Vector2(sideForce, flapForce));
            flapForce *= 0.5f;
            sideForce *= 0.5f;
        }

        Vector2 vel = rb.velocity;

        if (vel.x > speedLimit) {
            vel.x = speedLimit;
        } else if (vel.x < -speedLimit) {
            vel.x = -speedLimit;
        }

        if (vel.y > speedLimit) {
            vel.y = speedLimit;
        } else if (vel.y < -speedLimit) {
            vel.y = -speedLimit;
        }

        /*if (transform.position.y > maxHeight) {
            Vector3 pos = transform.position;
            pos.y = maxHeight;
            transform.position = pos;
            vel.y = 0.1f;
        }*/

        rb.velocity = vel;
    }
}
