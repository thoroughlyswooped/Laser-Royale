using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : HittableObject
{

    public override Vector2[] Hit(Vector2 dir, RaycastHit2D hitInfo, float maxCastRange, GameObject laser = null)
    {
        //TODO: Ezra: implement scene change/game end menu here.
        Debug.Log("Win");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex < (SceneManager.sceneCountInBuildSettings - 1))
        {
            PlayerPrefs.SetInt("CurrentLevel", currentSceneIndex + 1);
            int MaxLevel = PlayerPrefs.GetInt("MaxLevel", 1);
            
            PlayerPrefs.SetInt("MaxLevel", Mathf.Max(currentSceneIndex + 1, MaxLevel));

            if (levelLoader != null)
            {
                levelLoader.LoadLevelCaller(currentSceneIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
        }
        else
        {
            // Reset the current level and go back to the main menu
            PlayerPrefs.SetInt("CurrentLevel", 1);

            if (levelLoader != null)
            {
                levelLoader.LoadLevelCaller(0);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

        return base.Hit(dir, hitInfo, maxCastRange);
    }
}
