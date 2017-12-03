using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    Transform targetTransform;

    public GameObject target;

    public float velFactor;
    public float verticalLerp;

    public float minHeight;

    public Vector3 offset;
    public Vector3 targetStart;
    public Vector3 cameraStart;

	// Use this for initialization
	void Start () {
        targetTransform = target.transform;

        targetStart = targetTransform.position;
        cameraStart = transform.position;

        offset = cameraStart - targetStart;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 pos = targetTransform.position + offset;
        float y = Mathf.Lerp(transform.position.y, pos.y, verticalLerp);

        if (y < minHeight) y = minHeight;

        pos.y = y;
        transform.position = pos; //Vector3.Lerp(transform.position, pos, Time.deltaTime);
	}
}
