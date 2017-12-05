using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAntics : MonoBehaviour {

    public GameObject drgnPrefab;
    public GameObject archerPrefab;
    public GameObject peasantPrefab;
    public GameObject gerblinPrefab;

    public float minGerblinSpawn;
    public float maxGerblinSpawn;
    public float nextGerblin;

    public float minArcherSpawn;
    public float maxArcherSpawn;
    public float nextArcher;

    public float minPeasantSpawn;
    public float maxPeasantSpawn;
    public float nextPeasant;

    public float spawnPos = 25f;

    void Update() {
        nextGerblin -= Time.deltaTime;
        nextPeasant -= Time.deltaTime;
        nextArcher -= Time.deltaTime;

        if (nextGerblin < 0) {
            nextGerblin = Random.Range(minGerblinSpawn, maxGerblinSpawn);
            Spawn(gerblinPrefab);
        }

        if (nextArcher < 0) {
            nextArcher = Random.Range(minArcherSpawn, maxArcherSpawn);
            Spawn(archerPrefab);
        }

        if (nextPeasant < 0) {
            nextPeasant = Random.Range(minPeasantSpawn, maxPeasantSpawn);
            Spawn(peasantPrefab);
        }
    }

    void Spawn(GameObject prefab) {
        float x = Random.Range(0f, 1f);
        Vector3 pos = Vector3.zero;
        bool left = false;

        if (x < 0.5f) {
            //left side
            x = -spawnPos;
            left = true;
        } else {
            //right side
            x = spawnPos;
        }

        pos.x = x;

        GameObject g = (GameObject)Instantiate(prefab, pos, Quaternion.identity);

        if (!left) {
            g.transform.localScale = new Vector3(-1, 1, 1);
        }

        TitleRunner r = g.GetComponent<TitleRunner>();
        r.left = left;
    }
}
