using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceWithRubble : MonoBehaviour {

    public GameObject rubblePrefab;
    public ParticleSystem destroyEffect;
    public GameObject toHide;

    float timeToReplace;

    bool crumbling = false;
    bool replaced = false;

    public void crumble() {
        timeToReplace = destroyEffect.main.startLifetime.constant / 2f;
        destroyEffect.gameObject.SetActive(true);
        crumbling = true;
        Debug.Log("crumbling");
    }
	
	// Update is called once per frame
	void Update () {
        if (crumbling) {
            timeToReplace -= Time.deltaTime;

            if (!replaced && timeToReplace <= 0) {
                Debug.Log("replacing");
                toHide.SetActive(false);
                replaced = true;
                Instantiate(rubblePrefab, transform.position, transform.rotation, transform.parent);
            }

            if (timeToReplace < destroyEffect.main.startLifetime.constant / -2f) {
                Destroy(gameObject);
            }
        }
	}
}
