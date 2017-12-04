using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanel : MonoBehaviour {

    public void buyGerblin() {
        GameController.Instance.buyGerblin();
    }

    public void nextLevel() {
        GameController.Instance.startNextLevel();
    }
}
