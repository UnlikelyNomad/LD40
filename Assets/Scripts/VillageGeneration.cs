using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageGeneration : MonoBehaviour {

    [System.Serializable]
    public struct VillageAsset {
        public GameObject prefab;
        public float spacing;
        public float chance;
        public bool outside;
        public bool inside;
        public bool wall;
        public bool keep;
    }

    public float startSizeToArchers;
    public float step = 15f;

    public Transform villageParent;
    public Transform backdropParent;

    public GameObject keepPrefab;
    public GameObject gatePrefab;
    public GameObject hutPrefab;

    public GameObject mountainPrefab;

    public float mountainSpacing;

    public GameObject roadPrefab;
    public float roadSpacing;

    public GameObject farmPrefab;
    public float farmDepth;
    public float farmSpacing;

    public GameObject[] backdropPrefabs;

    public GameObject[] clouds;
    public float[] heights;
    public float[] layers;

    public GameObject archerPrefab;

    public float villageSize;

    public float keepFactor;
    public float gateFactor;

	// Use this for initialization
    public void SpawnVillage (float size) {

        villageSize = size;

        Vector3 pos = Vector3.zero;

        int numKeeps = (int)(size * keepFactor);
        int numGates = (int)(size * gateFactor);

        float gateStep = 0;

        int firstHalfKeeps = numKeeps / 2;
        int secondHalfKeeps = numKeeps - firstHalfKeeps;

        float nextGate = 0;

        for (float x = villageSize / 2f; x > -1; x -= step) {
            pos = new Vector3(x, 0, 0);

            if (firstHalfKeeps > 0) {
                //place a keep
                firstHalfKeeps--;
                Instantiate(keepPrefab, pos, Quaternion.identity, villageParent);

                if (firstHalfKeeps == 0) {
                    gateStep = x / (numGates + 1);
                    nextGate = x - gateStep;
                }
                continue;
            }

            if (x <= nextGate) {
                nextGate -= gateStep;
                Instantiate(gatePrefab, pos, Quaternion.identity, villageParent);
                continue;
            }

            Instantiate(hutPrefab, pos, Quaternion.identity, villageParent);
        }

        for (float x = villageSize / 2f + step; x <= villageSize + 1; x += step) {
            pos = new Vector3(x, 0, 0);

            if (secondHalfKeeps > 0) {
                secondHalfKeeps--;
                Instantiate(keepPrefab, pos, Quaternion.identity, villageParent);

                if (secondHalfKeeps == 0) {
                    gateStep = (villageSize - x) / (numGates + 1);
                    nextGate = x + gateStep;
                }
                continue;
            }

            if ( x >= nextGate) {
                nextGate += gateStep;
                Instantiate(gatePrefab, pos, Quaternion.identity, villageParent);
                continue;
            }

            Instantiate(hutPrefab, pos, Quaternion.identity, villageParent);
        }

        //backdrops
        for (float x = 0; x < villageSize; x += 10f) {

            GameObject item = backdropPrefabs[Random.Range(0, backdropPrefabs.Length)];
            float z = Random.Range(30, 300);
            float a = Random.Range(0, 360);

            Instantiate(item, new Vector3(x, 0, z), Quaternion.Euler(0, a, 0), villageParent);
        }

        //clouds
        for (float x = 0; x < villageSize; x += 20f) {
            pos = new Vector3();
            pos.x = x;
            pos.y = heights[Random.Range(0, heights.Length)];
            pos.z = layers[Random.Range(0, layers.Length)];

            Quaternion r = Quaternion.Euler(0, Random.Range(0.0f, 360f), 0);
            Instantiate(clouds[Random.Range(0, clouds.Length)], pos, r, backdropParent);
        }

        //starting archers
        int numArchers = (int)(villageSize * startSizeToArchers);
        float startX = villageSize / 2;
        startX -= (numArchers / 2) * 10;
        for (int i = 0; i < numArchers; ++i) {
            float x = startX + (i * 10f);

            GameObject g = (GameObject)Instantiate(archerPrefab, new Vector3(x, 0, 0), Quaternion.identity);
            Archer a = g.GetComponent<Archer>();
            a.ready = true;
        }

        //mountains
        float mountainX = 0f;
        while (mountainX < (villageSize + mountainSpacing / 2f)) {
            Instantiate(mountainPrefab, new Vector3(mountainX, 0, 0), Quaternion.identity, villageParent);
            mountainX += mountainSpacing;
        }

        //roads
        float roadX = -50f;
        while (roadX + roadSpacing < villageSize + 50f) {
            Instantiate(roadPrefab, new Vector3(roadX, 0, 0), Quaternion.identity, villageParent);
            roadX += roadSpacing;
        }

        //farms
        /*float farmX = 0;
        while (farmX < villageSize) {
            Instantiate(farmPrefab, new Vector3(farmX, 0, farmDepth), Quaternion.identity, villageParent);
            farmX += farmSpacing;
        }*/
	}
}
