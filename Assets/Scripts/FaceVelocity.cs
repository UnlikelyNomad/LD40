using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceVelocity : MonoBehaviour {

    Rigidbody2D rb;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 vel = ((Vector3)rb.velocity) + offset;

        Quaternion r = Quaternion.LookRotation(vel);
        transform.rotation = r;
	}
}
