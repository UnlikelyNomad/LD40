using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    Transform targetTransform;
    Rigidbody2D targetRB;

    public GameObject target;

    public float velFactor;

	// Use this for initialization
	void Start () {
        targetTransform = target.transform;
        targetRB = target.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 pos = transform.position;
        pos.x = targetTransform.position.x + (targetRB.velocity.x * velFactor);
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
	}
}
