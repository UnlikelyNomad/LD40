using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    static GameController _instance;

    public GameObject villagePrefab;

    public float goldToVillageRatio;
    public float startVillageSize;

    public float gold = 0;
    public float goldCollected = 0;

    public VillageGeneration village;

    public static GameController Instance {
        get { return _instance;}
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log(scene.name);

        if (scene.name.CompareTo("_Raid") == 0) {
            startRaid();
        } else if (scene.name.CompareTo("_Lair") == 0) {
            startLair();
        }
    }

    void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void startRaid() {
        float villageSize = (gold * goldToVillageRatio) + startVillageSize;
        GameObject g = (GameObject)Instantiate(villagePrefab);
        VillageGeneration vg = g.GetComponent<VillageGeneration>();
        vg.SpawnVillage(villageSize);

        village = vg;
    }

    void startLair() {
        
    }

    public void collectGold(float amount) {
        gold += amount;
        goldCollected += amount;
    }

    public void LeaveRaid() {
        SceneManager.LoadScene(1);
    }
}
