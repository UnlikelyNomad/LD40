using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float health = 10f;

    public void takeDamage(float amount) {
        health -= amount;
        if (health < 0) {
            health = 0;
            Die();
        }
    }

    void Die() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
