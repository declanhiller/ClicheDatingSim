using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    

    public void MilfScene() {
        
    }

    public void BadBoyScene() {
        
    }

    public void StartMenu() {
        SceneManager.LoadScene("StartingMenu");
    }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }

    public void Exit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    
    
}
