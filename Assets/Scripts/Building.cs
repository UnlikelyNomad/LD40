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

    public float coinValue;

    bool crumbling = false;

    ParticleSystem.EmissionModule em;

    AudioSource src;

	// Use this for initialization
	void Start () {
        em = onFireEffect.emission;
        src = GetComponent<AudioSource>();
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

            GameController.Instance.SpawnCoin(coinValue, transform.position);

            return;
        }

        em.enabled = onFire;

        if (onFire && !src.isPlaying) {
            src.Play();
        } else if (!onFire && src.isPlaying){
            src.Pause();
        }
	}

    public void takeDamage(float amount) {
        health -= amount;

        if (!onFire) {
            onFire = true;
            totalFireDamage = fireDamage;
        }
    }
}
