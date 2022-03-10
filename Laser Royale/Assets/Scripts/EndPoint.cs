using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : HittableObject
{
    public override Vector2[] Hit(Vector2 dir, RaycastHit2D hitInfo, float maxCastRange)
    {
        //TODO: Ezra: implement scene change/game end menu here.
        Debug.Log("Win");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex < (SceneManager.sceneCountInBuildSettings - 1))
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }

        return base.Hit(dir, hitInfo, maxCastRange);
    }
}
