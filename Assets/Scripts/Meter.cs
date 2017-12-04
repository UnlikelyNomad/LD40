using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meter : MonoBehaviour {

    public int startValue;
    public int value;

    Color frameColor;
    Color pipColor;
    Color iconColor;

    public Image icon;
    public Image frame;
    public Image[] pips;

    public void setValue(int v) {
        value = v;

        if (value > 10) value = 10;
        else if (value < 0) value = 0;

        for (int i = 0; i < pips.Length; ++i) {
            pips[i].enabled = (i < value);
        }
    }

    public void setIconTint(Color c) {
        icon.color = c;
    }

    public void resetIconTint() {
        icon.color = iconColor;
    }

    public void setFrameColor(Color c) {
        frame.color = c;
    }

    public void resetFrameColor() {
        frame.color = frameColor;
    }

    public void setPipColor(Color c) {
        foreach (Image i in pips) {
            i.color = c;
        }
    }

    public void resetPipColor() {
        foreach(Image i in pips) {
            i.color = pipColor;
        }
    }

    public int getValue() {
        return value;
    }

    public int decrement() {
        setValue(value - 1);
        return value;
    }

    public int increment() {
        setValue(value + 1);
        return value;
    }

	// Use this for initialization
	void Start () {
        iconColor = icon.color;
        frameColor = frame.color;
        pipColor = pips[0].color;

        setValue(startValue);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
