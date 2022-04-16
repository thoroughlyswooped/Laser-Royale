using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

    LevelLoader levelLoader;

    private void Start()
    {
        if(LevelLoader.instance != null)
        {
            levelLoader = LevelLoader.instance;
        }
    }

    //Load the next level
    public void Play(){
        int currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 1);

        if(levelLoader != null)
        {
            levelLoader.LoadLevelCaller(currentLevelIndex);
            return;
        }

        SceneManager.LoadScene(currentLevelIndex);
    }


    //Mario's Script
    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();

    }
}
