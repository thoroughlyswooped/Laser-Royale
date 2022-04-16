using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public void LoadLevel(TMP_Text Text)
    {
        int level = int.Parse(Text.text);
        if(LevelLoader.instance != null)
        {
            LevelLoader.instance.LoadLevelCaller(level);
            return;
        }

        SceneManager.LoadScene(level);
    }
}
