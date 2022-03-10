using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
    
    //Load the next level
    public void Play(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    //Mario's Script
    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();

    }
}
