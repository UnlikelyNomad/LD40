using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour {

    public GameObject helpPanel;
    public GameObject mainPanel;
    public Text infoText;
    public Text buttonText;

    int state;

    public void Help() {
        state = 0;
        Next();

        mainPanel.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Next() {
        switch (state) {
        case 0:
            //intro
            buttonText.text = "Next";
            infoText.text = "Welcome to DRGN!\n\nIt's gold hoarding time. Luckily there are several nearby villages to raid!\n\nBlow up the buildings, devour the inhabitants, and avoid the arrows.";
            break;
        case 1:
            //intro 2
            buttonText.text = "Next";
            infoText.text = "Once you've have your fill of the village fly off to the  left or right to leave and end the level.\n\nAfter each level you'll see how much gold you've amassed and have the options to use the gold to buy Gerblins that will automatically gather treasure for you!";
            break;
        case 2:
            //health
            buttonText.text = "Next";
            infoText.text = "In the bottom-left of the screen is your health meter. When this runs out the game is over.\n\nGetting hit by arrows hurts. Duh.\n\nYou can regain a health by eating a peasant or an archer!";
            break;
        case 3:
            //stamina
            buttonText.text = "Next";
            infoText.text = "The bottom middle of the screen has your stamina meter. When your stamina runs out you will start to lose life.\n\nEating a peasant or archer completely refills your stamina bar.";
            break;
        case 4:
            //gold
            buttonText.text = "Next";
            infoText.text = "The weight of the gold you're carrying is shown in the lower right. You can continue to collect coins even if the gold meter has filled.\n\nThe more you're carrying the faster your stamina will run out and the worse you'll fly.";
            break;
        case 5:
            //controls
            buttonText.text = "Done";
            infoText.text = "The default controls to fly are A and Left Arrow to fly up and to the left and D or Right Arrow to fly up and to the right.\n\nSpacebar will launch a fireball while in the air.\n\nS or Down Arrow will eat a village while on the ground.";
            break;
        case 6:
            //back to main panel
            gameObject.SetActive(false);
            mainPanel.SetActive(true);
            break;
        }

        state++;
    }
}
