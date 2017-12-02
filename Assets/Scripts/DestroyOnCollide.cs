﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour {

    public LayerMask destroyLayers;

    void OnCollisionEnter2D(Collision2D col) {
        Destroy(gameObject);
        GameObject other = col.gameObject;

        if (((1 << other.layer) & destroyLayers.value) > 0) {
            Destroy(other.transform.root.gameObject);
        }
    }
}
