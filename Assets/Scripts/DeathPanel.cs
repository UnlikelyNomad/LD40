using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPanel : MonoBehaviour {

    public void PlayAgain() {
        GameController.Instance.Restart();
    }
}
