using UnityEngine;

public class GameController : MonoBehaviour
{
    AudioManager audioManager;

    public GameObject settingsMenu;

    private void Start()
    {
        if(AudioManager.instance != null)
        {
            audioManager = AudioManager.instance;
        }

        CustomSlider[] sliders = settingsMenu.GetComponentsInChildren<CustomSlider>();
        foreach (var slider in sliders)
        {
            slider.Initialize();
        }

    }

    public void PlaySound(string name)
    {
        audioManager.Play(name);
    }
}
