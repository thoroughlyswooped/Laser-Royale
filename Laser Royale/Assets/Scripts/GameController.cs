using UnityEngine;

public class GameController : MonoBehaviour
{
    AudioManager audioManager;
    private void Start()
    {
        if(AudioManager.instance != null)
        {
            audioManager = AudioManager.instance;
        }
    }

    public void PlaySound(string name)
    {
        audioManager.Play(name);
    }
}
