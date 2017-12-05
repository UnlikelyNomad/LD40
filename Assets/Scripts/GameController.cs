using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    static GameController _instance;

    public AudioClip music;
    public AudioClip levelEndMusic;
    public AudioClip deadMusic;

    public AudioClip coinClip;

    bool ready = false;

    public GameObject villagePrefab;

    public float goldToVillageRatio;
    public float startVillageSize;

    public float villageLimit = 25f;

    public float gold = 0;
    public float goldCollected = 0;

    Meter healthMeter;
    Meter staminaMeter;
    Meter goldMeter;

    public float staminaDecayRate;
    public float currentStaminaDecay;
    public float staminaDamageRate;
    public float currentDamageTimer;

    GameObject levelPanel;
    GameObject deathPanel;
    Text currentGoldText;
    Text totalGoldText;
    Text gerblinCount;
    Text finalGold;
    Text gerblinsAvailable;

    GameObject leavingText;

    public GameObject coinPrefab;

    public float gerblinCost = 8f;

    public int gerblins = 0;
    public int totalGerblins = 0;

    public GameObject gerblinPrefab;

    GameObject player;

    AudioSource src;

    public Image damageOverlay;

    public float damageOpacity;
    public float damageLength;
    float damageTimer;

    [HideInInspector]
    public VillageGeneration village;

    public static GameController Instance {
        get { return _instance;}
    }

    public void startNextLevel() {
        SceneManager.LoadScene("_Raid");
    }

    public void buyGerblin() {
        if (gold >= gerblinCost) {
            gold -= gerblinCost;
            currentGoldText.text = gold + "G";
            totalGerblins++;

            gerblinCount.text = "" + totalGerblins;
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name.CompareTo("_Raid") == 0) {
            startRaid();
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

    public void SpawnCoin(float amount, Vector3 position) {
        GameObject coin = (GameObject)Instantiate(coinPrefab, position, Quaternion.identity);
        Treasure c = coin.GetComponent<Treasure>();
        c.setValue(amount);

        spawnGerblin(position);
    }

    void spawnGerblin(Vector3 position) {
        if (gerblins > 0) {
            gerblins--;

            float startX;
            if (position.x < (village.villageSize / 2f)) {
                //left
                startX = villageLimit * -2f;
            } else {
                //right
                startX = village.villageSize + (villageLimit * 2f);
            }

            Vector3 spawnPos = new Vector3(startX, 0, 0);
            Instantiate(gerblinPrefab, spawnPos, Quaternion.identity);
        }
    }

    void startRaid() {
        levelPanel = GameObject.FindGameObjectWithTag("LevelPanel");
        deathPanel = GameObject.FindGameObjectWithTag("DeathPanel");
        currentGoldText = GameObject.FindGameObjectWithTag("GoldAmount").GetComponent<Text>();
        totalGoldText = GameObject.FindGameObjectWithTag("TotalAmount").GetComponent<Text>();
        gerblinCount = GameObject.FindGameObjectWithTag("GerblinCount").GetComponent<Text>();
        finalGold = GameObject.FindGameObjectWithTag("FinalGold").GetComponent<Text>();
        leavingText = GameObject.FindGameObjectWithTag("LeavingText");
        gerblinsAvailable = GameObject.FindGameObjectWithTag("GoblinsAvailable").GetComponent<Text>();
        damageOverlay = GameObject.FindGameObjectWithTag("DamageOverlay").GetComponent<Image>();

        levelPanel.SetActive(false);
        deathPanel.SetActive(false);
        leavingText.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");

        healthMeter = GameObject.FindGameObjectWithTag("HeartMeter").GetComponent<Meter>();
        staminaMeter = GameObject.FindGameObjectWithTag("StaminaMeter").GetComponent<Meter>();
        goldMeter = GameObject.FindGameObjectWithTag("GoldMeter").GetComponent<Meter>();
        healthMeter.setValue(8);
        staminaMeter.setValue(8);
        goldMeter.setValue(0);

        float villageSize = (goldCollected * goldToVillageRatio) + startVillageSize;
        GameObject g = (GameObject)Instantiate(villagePrefab);
        VillageGeneration vg = g.GetComponent<VillageGeneration>();
        vg.SpawnVillage(villageSize);

        village = vg;

        gerblins = totalGerblins;

        currentStaminaDecay = staminaDecayRate;
        ready = true;

        src = GetComponent<AudioSource>();
        src.clip = music;
        src.Play();

    }

    public void collectGold(float amount, bool player) {

        src.PlayOneShot(coinClip, 0.4f);

        gold += amount;
        goldCollected += amount;

        if (player) {
            goldMeter.increment();
        }
    }

    public void LeaveRaid() {
        levelPanel.SetActive(true);
        player.SetActive(false);

        updateLevelPanel();

        src.clip = levelEndMusic;
        src.Play();
    }

    void updateLevelPanel() {
        currentGoldText.text = gold + "g";
        totalGoldText.text = goldCollected + "g";
        gerblinCount.text = "" + totalGerblins;
    }

    public void damagePlayer() {
        healthMeter.decrement();
        damageTimer = damageLength;
    }

    public void Eat() {
        staminaMeter.setValue(10);
        healthMeter.increment();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("_Title");
            Destroy(gameObject);
            return;
        }

        gerblinsAvailable.text = "" + gerblins;

        if (ready) {
            int goldPips = goldMeter.getValue();

            if (goldPips > 0 && staminaMeter.getValue() > 0) {
                float decay = Time.deltaTime / (11 - goldPips);
                currentStaminaDecay -= decay;

                if (currentStaminaDecay < 0) {
                    staminaMeter.decrement();
                    currentStaminaDecay = staminaDecayRate;

                    if (staminaMeter.getValue() == 0) {
                        currentDamageTimer = staminaDamageRate;
                    }
                }
            }

            if (staminaMeter.getValue() == 0) {
                currentDamageTimer -= Time.deltaTime;

                if (currentDamageTimer < 0) {
                    currentDamageTimer = staminaDamageRate;
                    healthMeter.decrement();
                }
            }

            if (player.activeInHierarchy && healthMeter.getValue() == 0) {
                killPlayer();
            }



            float x = player.transform.position.x;

            if (x < -villageLimit || x > village.villageSize + villageLimit) {
                leavingText.SetActive(true);
            } else {
                leavingText.SetActive(false);
            }

            if (player.activeInHierarchy) {
                if (x < (villageLimit * -2f) || x > village.villageSize + (villageLimit * 2f)) {
                    LeaveRaid();
                }
                if (damageTimer > 0) {
                    damageTimer -= Time.deltaTime;
                }

                float o = damageOpacity * (damageTimer / damageLength);
                if (o < 0) o = 0;
                Color c = damageOverlay.color;
                c.a = o;
                damageOverlay.color = c;
            } else {
                Color c = damageOverlay.color;

                if (healthMeter.getValue() == 0) {
                    c.a = damageOpacity;
                } else {
                    c.a = 0;
                }

                damageOverlay.color = c;
            }
        }
    }

    void killPlayer() {
        player.SetActive(false);
        deathPanel.SetActive(true);
        finalGold.text = goldCollected + "g";

        src.clip = deadMusic;
        src.Play();
    }

    public void Restart() {
        gold = 0;
        goldCollected = 0;
        gerblins = 0;
        totalGerblins = 0;
        SceneManager.LoadScene("_Raid");
    }
}
