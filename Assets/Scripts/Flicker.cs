using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour {

    public float movement = 0.2f;
    public float flicker = 1f;

    float startRange;
    float startIntensity;

    Vector3 startPos;

    Light l;

	// Use this for initialization
	void Start () {
        l = GetComponent<Light>();

        startPos = transform.position;
        startRange = l.range;
        startIntensity = l.intensity;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = startPos + new Vector3(Random.Range(-movement, movement), Random.Range(-movement, movement), Random.Range(-movement, movement));
        l.range = startRange + Random.Range(0, flicker);
        l.intensity = startIntensity + Random.Range(0, flicker);
	}
}
