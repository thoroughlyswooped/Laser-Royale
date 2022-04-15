using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class CustomSlider : MonoBehaviour
{
    public TMP_Text textVal;
    public string paramString;
    public string playerPrefString;
    public string prevValString;

    public Toggle muteToggle;

    public AudioMixerGroup mixerGroup;

    [SerializeField]
    float prevVal;
    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        float vol = PlayerPrefs.GetFloat(playerPrefString, 0f);

        if (vol == -80)
        {
            prevVal = PlayerPrefs.GetFloat(prevValString, 0f);
            GetComponent<Slider>().value = prevVal;
            textVal.text = prevVal.ToString("0.0");
            Mute(true);
        }
        else
        {
            GetComponent<Slider>().value = vol;
        }
    }

    public void SetValue(float val)
    {
        textVal.text = val.ToString("0.0");

        mixerGroup.audioMixer.SetFloat(paramString, val);

        PlayerPrefs.SetFloat(playerPrefString, val);
    }

    public void Mute(bool mute)
    {
        muteToggle.isOn = mute;
        Slider slider = GetComponent<Slider>();
        if (mute)
        {
            float val = slider.value;
            prevVal = val;
            PlayerPrefs.SetFloat(prevValString, val);

            //mute audiomixer and set player pref manually
            mixerGroup.audioMixer.SetFloat(paramString, -80f);
            PlayerPrefs.SetFloat(playerPrefString, -80f);

            slider.interactable = false;
        }
        else
        {
            slider.interactable = true;
            if (slider.value == prevVal)
            {
                //set value manually because slider value has not changed
                SetValue(prevVal);
            }
            else
            {
                slider.value = prevVal;
            }
        }


    }
}
