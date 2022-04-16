using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    public static LevelLoader instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else 
        {
            Debug.Log("There is already a level loader, but you are trying to create another one!");
            Destroy(this);
            return;
        }
    }

    public void LoadLevelCaller(int levelIndex)
    {
        StartCoroutine(LoadLevel(levelIndex));
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        Debug.Log("Load Level Using Transition");

        //Play animation
        //TODO: Ezra: the player can still play the game while the scene is loading, this is not an issue but should be cleaned up at some point.
        transition.SetTrigger("Start");

        //Wait
        //TODO: Ezra: this is hacky, can play the animation at the end of the animation instead
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
