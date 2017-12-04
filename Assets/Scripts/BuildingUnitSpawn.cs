using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnitSpawn : MonoBehaviour {

    public GameObject prefab;
    public Transform spawn;
    public int spawnLimit;

    bool spawned;

    Building bldg;

	// Use this for initialization
	void Start () {
        bldg = GetComponent<Building>();
	}
	
	// Update is called once per frame
	void Update () {
        if (bldg.onFire) {
            if (!spawned) {
                spawned = true;

                Instantiate(prefab, spawn.position, Quaternion.identity);
                spawnLimit--;

                if (spawnLimit == 0) {
                    this.enabled = false;
                }
            }
        } else {
            spawned = false;
        }
	}
}
