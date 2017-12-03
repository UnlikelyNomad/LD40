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

    public Transform villageParent;
    public Transform backdropParent;

    public VillageAsset[] assets;

    public VillageAsset[] backdropAssets;
    public float[] backdropLayers;

    public GameObject[] clouds;
    public float[] heights;
    public float[] layers;

    public GameObject archerPrefab;

    public float villageSize;

	// Use this for initialization
    public void SpawnVillage (float size) {

        villageSize = size;

        Vector3 pos = Vector3.zero;

        for (float x = 0; x < villageSize;) {

            float totalChance = 0f;
            for (int i = 0; i < assets.Length; ++i) {
                totalChance += assets[i].chance;
            }

            float c = Random.Range(0f, totalChance);

            bool placed = false;
            for (int i = 0; i < assets.Length; ++i) {
                if (c > assets[i].chance) {
                    c -= assets[i].chance;
                    continue;
                }

                placed = true;
                pos.x = x + assets[i].spacing / 2f;

                x += assets[i].spacing;

                Instantiate(assets[i].prefab, pos, Quaternion.identity, villageParent);
                break;
            }

            if (!placed) {
                x += 10f;
            }
        }

        for (float x = 0; x < villageSize;) {

            float totalChance = 0f;
            for (int i = 0; i < backdropAssets.Length; ++i) {
                totalChance += backdropAssets[i].chance;
            }

            float c = Random.Range(0f, totalChance);

            bool placed = false;
            for (int i = 0; i < backdropAssets.Length; ++i) {
                if (c > backdropAssets[i].chance) {
                    c -= backdropAssets[i].chance;
                    continue;
                }

                placed = true;
                pos.x = x + backdropAssets[i].spacing / 2f;

                pos.z = backdropLayers[Random.Range(0, backdropLayers.Length)];

                x += backdropAssets[i].spacing;

                GameObject g = (GameObject)Instantiate(backdropAssets[i].prefab, pos, Quaternion.identity, backdropParent);
                g.layer = 9;
                break;
            }

            if (!placed) {
                x += 10f;
            }
        }

        for (float x = 0; x < villageSize; x += 20f) {
            pos = new Vector3();
            pos.x = x;
            pos.y = heights[Random.Range(0, heights.Length)];
            pos.z = layers[Random.Range(0, layers.Length)];

            Quaternion r = Quaternion.Euler(0, Random.Range(0.0f, 360f), 0);
            Instantiate(clouds[Random.Range(0, clouds.Length)], pos, r, backdropParent);
        }

        int numArchers = (int)(villageSize * startSizeToArchers);
        float startX = villageSize / 2;
        startX -= (numArchers / 2) * 10;
        for (int i = 0; i < numArchers; ++i) {
            float x = startX + (i * 10f);

            Instantiate(archerPrefab, new Vector3(x, 0, 0), Quaternion.identity);
        }
	}
}
