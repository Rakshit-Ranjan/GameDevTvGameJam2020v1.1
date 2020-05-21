using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour {
    
    public void LoadStoryLineScene() {
        SceneManager.LoadScene("Storyline");
        FindObjectOfType<AudioManager>().Play("MouseClick");
    }

    public void LoadControlScene() {
        FindObjectOfType<AudioManager>().Play("MouseClick");
        SceneManager.LoadScene("Control");
    }

    public void LoadPastLevel() {
        FindObjectOfType<AudioManager>().Play("MouseClick");
        SceneManager.LoadScene("Past");
    }

    public void Quit() {
        FindObjectOfType<AudioManager>().Play("MouseClick");
        Application.Quit();
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
        FindObjectOfType<AudioManager>().Play("MouseClick");
    }
}
