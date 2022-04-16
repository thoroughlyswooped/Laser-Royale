using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform contentTransform;

    public void Awake()
    {
        int maxLevelIndex = PlayerPrefs.GetInt("MaxLevel", 1);
        //get all the scenes except the first and last
        for (int i = 1; i <= maxLevelIndex; i++)
        {
            //Create a button for each level
            GameObject newButton = Instantiate(levelButtonPrefab, contentTransform);
            TMP_Text text = newButton.GetComponentInChildren<TMP_Text>();
            if (text)
            {
                text.text = i.ToString();
            }
        }
    }
}
