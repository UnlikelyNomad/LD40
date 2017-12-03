using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LairController : MonoBehaviour {

    public Text goldText;

	// Use this for initialization
	void Start () {
        goldText.text = "Gold: " + GameController.Instance.gold;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
