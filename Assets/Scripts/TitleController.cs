using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour {

    public HelpPanel helpPanel;

    public void Play() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("_Raid");
    }

    public void Help() {
        helpPanel.Help();
    }

    public void Quit() {
        Application.Quit();
    }
}
