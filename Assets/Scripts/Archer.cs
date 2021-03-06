﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour {

    public float runSpeed = 0.1f;
    public bool run;
    public bool shoot;

    public float playerDistRun = 10f;
    public float playerMaxDist = 200f;

    public float shootChance = 0.8f;
    public float speedFactor = 0.02f;

    public float zDepth;

    public bool left;

    public Animator anim;

    public Transform arrowEffector;
    public float arrowVelocity;
    public float arrowDelay;
    public GameObject arrowPrefab;

    public bool ready;

    int runHash;
    int shootHash;
    int shootingHash;
    int runningHash;

    float shootTimer;
    bool firing = false;

    Rigidbody2D rb;

    GameObject player;

    Transform playerTransform;

    void fireArrow() {
        GameObject arrow = (GameObject)Instantiate(arrowPrefab, arrowEffector.transform.position, arrowEffector.transform.rotation);
        Rigidbody2D arb = arrow.GetComponent<Rigidbody2D>();
        Vector2 vel = new Vector2(0, arrowVelocity);
        vel.x = (player.transform.position.x - transform.position.x) / 2f;
        if (vel.x < -10f) vel.x = -10f;
        else if (vel.x > 10f) vel.x = 10f;
        arb.velocity = vel;
    }

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        runHash = Animator.StringToHash("Moving");
        shootHash = Animator.StringToHash("Shoot");
        shootingHash = Animator.StringToHash("Base Layer.Shooting");
        runningHash = Animator.StringToHash("Base Layer.Running");

        rb = GetComponent<Rigidbody2D>();

        playerTransform = player.transform;
        runSpeed *= (1 + Random.Range(-speedFactor, speedFactor));
	}
	
	// Update is called once per frame
	void Update () {
        if (ready) {
            bool inRange = Mathf.Abs(playerTransform.position.x - transform.position.x) < playerDistRun;

            if (inRange) {
                if (Random.Range(0, 1f) < shootChance) {
                    shoot = true;
                } else {
                    run = true;
                }
            } else {
                left = playerTransform.position.x < transform.position.x;
                run = true;
            }

            Vector3 scale = Vector3.one;

            if (left) {
                scale.x = -1;
            }

            transform.localScale = scale;

            anim.SetBool(runHash, run);

            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

            if (firing) {
                shootTimer -= Time.deltaTime;
                if (shootTimer < 0) {
                    firing = false;
                    fireArrow();
                }
            }

            if (shoot && info.fullPathHash != shootingHash) {
                shoot = false;
                firing = true;
                shootTimer = arrowDelay;

                anim.SetTrigger(shootHash);
            }
        }
	}

    void FixedUpdate() {
        if (ready) {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

            Vector2 vel = Vector2.zero;
            if (info.fullPathHash == runningHash) {
                vel.x = runSpeed;

                if (left) vel.x *= -1;
            }

            rb.velocity = vel;
        } else {
            //exiting building
            anim.SetBool(runHash, true);
            transform.rotation = Quaternion.Euler(0, 90, 0);
            Vector3 pos = transform.position;
            pos.z -= runSpeed * Time.fixedDeltaTime;

            if (pos.z < zDepth) {
                pos.z = zDepth;
                ready = true;
                transform.rotation = Quaternion.identity;
            }

            transform.position = pos;
        }
    }
}
