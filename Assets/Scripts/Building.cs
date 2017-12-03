using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public float health = 2f;
    public ParticleSystem onFireEffect;
    public float fireDamage = 0.4f;
    public float fireLength = 5f;

    public GameObject destroyEffect;

    public bool onFire = false;

    float totalFireDamage;

    public GameObject coinPrefab;
    public float coinValue;

    bool crumbling = false;

    ParticleSystem.EmissionModule em;

	// Use this for initialization
	void Start () {
        em = onFireEffect.emission;
	}
	
	// Update is called once per frame
	void Update () {
        if(onFire) {
            float damage = fireDamage * (Time.deltaTime / fireLength);
            if (totalFireDamage < damage) {
                damage = totalFireDamage;
                onFire = false;
            }

            totalFireDamage -= damage;
            health -= damage;
        }

        if (health < 0.01f && !crumbling) {
            ReplaceWithRubble rubble = GetComponent<ReplaceWithRubble>();

            if (rubble == null) {
                Destroy(gameObject);
            } else {
                rubble.crumble();
                crumbling = true;
            }

            GameObject coin = (GameObject)Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Treasure c = coin.GetComponent<Treasure>();
            c.setValue(coinValue);

            return;
        }

        em.enabled = onFire;
	}

    public void takeDamage(float amount) {
        health -= amount;

        if (!onFire) {
            onFire = true;
            totalFireDamage = fireDamage;
        }
    }
}
