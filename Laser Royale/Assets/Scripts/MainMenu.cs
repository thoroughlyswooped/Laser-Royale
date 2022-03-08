using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
    
    public void Play(){
        SceneManager.LoadScene("Assets/Scenes/Level1.unity");
    }


    //Mario's Script
    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();

    }
}
