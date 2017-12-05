using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public AudioClip flapClip;
    public AudioClip fireballClip;
    public AudioClip chompClip;

    public float flapClipMin = 0.4f;
    public float flapClipMax = 0.8f;

    public float flapImpulse;
    public float directionFactor;

    public float fireCooldown = 3f;
    public float fireDelay = 0.3f;
    public float speedLimit;

    public float groundLimit = 1.5f;

    public float heightFactor = -1f;

    public float forceDecay;

    float flapForce;
    float sideForce;

    public float fireTime;

    public float landedAngle = 30f;
    public float landingTurnSpeed = 3f;

    public GameObject eatObject;

    bool left;

    Vector3 scale;

    bool flap;
    bool fire;

    public GameObject projectile;
    public Vector2 fireVelocity;

    public Transform projectileEffector;
    public Transform projectileParent;

    public Animator anim;

    public float eatDelay;
    float eatDelayTimer;

    public Meter heartMeter;
    public Meter staminaMeter;
    public Meter goldMeter;

    int flapHash;
    int idleHash;
    int fireHash;
    int groundHash;
    int chompHash;
    int chompingHash;
    int landedHash;

    Rigidbody2D rb;

    public float upEncumbrence;
    public float sideEncumbrence;

    AudioSource src;

	// Use this for initialization
	void Start () {
        src = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();
        scale = Vector3.one;

        flapHash = Animator.StringToHash("Flap");
        fireHash = Animator.StringToHash("Fire");
        groundHash = Animator.StringToHash("OnGround");
        chompHash = Animator.StringToHash("Chomp");
        chompingHash = Animator.StringToHash("Base Layer.Chomping");
        idleHash = Animator.StringToHash("Base Layer.Gliding");
        landedHash = Animator.StringToHash("Base Layer.Landed");
	}
	
	// Update is called once per frame
    void Update () {

        bool grounded = transform.position.y < groundLimit;
        anim.SetBool(groundHash, grounded);

        float rotateAngle = 0;
        if (grounded) {
            rotateAngle = landedAngle;

            if (left) rotateAngle *= -1;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotateAngle, 0), Time.deltaTime * landingTurnSpeed);

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        bool chomping = info.fullPathHash == chompingHash;

        if (chomping) {
            if (eatDelayTimer > 0) {
                eatDelayTimer -= Time.deltaTime;
                if (eatDelayTimer < 0) {
                    eatObject.SetActive(true);
                }
            }
        } else {
            eatObject.SetActive(false);
        }

        //sets the facing
        float velX = rb.velocity.x;
        if (Mathf.Abs(velX) > 0.01f) {
            left = velX < 0f;
            scale.x = Mathf.Sign(velX);
            transform.localScale = scale;
        }


        //get inputs if in glide state or landed
        if (info.fullPathHash == idleHash || info.fullPathHash == landedHash) {
            if (Input.GetButton("FlapLeft")) {
                src.PlayOneShot(flapClip, Random.Range(flapClipMin, flapClipMax));
                anim.SetTrigger(flapHash);
                doFlap();
                float encumbrence = goldMeter.getValue() * sideEncumbrence;
                sideForce = -1 * (directionFactor - encumbrence);
            } else if (Input.GetButton("FlapRight")) {
                src.PlayOneShot(flapClip, Random.Range(flapClipMin, flapClipMax));
                anim.SetTrigger(flapHash);
                doFlap();
                float encumbrence = goldMeter.getValue() * sideEncumbrence;
                sideForce = directionFactor - encumbrence;
            }
        }

        if (fireTime <= 0) {
            fire = false;
            if (!grounded && Input.GetButton("Fire")) {
                anim.SetTrigger(fireHash);
                fireTime = fireCooldown;
                src.PlayOneShot(fireballClip, 0.4f);
            } else if (grounded && Input.GetButtonDown("Eat")) {
                if (info.fullPathHash != chompingHash) {
                    src.PlayOneShot(chompClip, 0.4f);
                    anim.SetTrigger(chompHash);
                    eatDelayTimer = eatDelay;
                }
            }
        } else {
            fireTime -= Time.deltaTime;

            if (!fire && fireCooldown - fireTime > fireDelay) {
                fire = true;
                GameObject p = (GameObject)Instantiate(projectile, projectileEffector.position, Quaternion.identity, projectileParent);
                float vel = fireVelocity.x;
                if (left) vel *= -1;
                Vector2 pV = new Vector2(vel + rb.velocity.x, fireVelocity.y);
                p.GetComponent<Rigidbody2D>().velocity = pV;
            }
        }
	}

    void doFlap() {
        float encumbrence = goldMeter.getValue() * upEncumbrence;
        float f = flapImpulse - (heightFactor * transform.position.y) - encumbrence;
        if (f > flapForce) flapForce = f;
    }

    void FixedUpdate() {
        if (flapForce > 0.01) {

            Vector2 f = new Vector2(sideForce, flapForce);

            rb.AddForce(f);

            flapForce *= forceDecay;
            sideForce *= forceDecay;
        }
    }
}
