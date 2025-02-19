using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SoundSliderType
{
    bgm,effect,neither
}
public class SoundSliderScript : MonoBehaviour
{
    public SoundSliderType sliderType;
    public Slider slider;
    public Text valueText;
    
    AudioSource bgmAudio;
    AudioSource effectAudio;

    // Start is called before the first frame update
    void Start()
    {
        bgmAudio = GameObject.Find("BGMSound").GetComponent<AudioSource>();
        effectAudio = GameObject.Find("EffectSound").GetComponent<AudioSource>();

        if (sliderType == SoundSliderType.bgm)
        {
            slider.value = PlayerPrefs.GetFloat(Consts.BGMPlayerPrefTag, 50f);
            SetBgm();
        }
        else if (sliderType == SoundSliderType.effect)
        {
            slider.value = PlayerPrefs.GetFloat(Consts.EffectPlayerPrefTag, 80f);
            SetEffect();
        }
        else
        {
            float volume = PlayerPrefs.GetFloat(Consts.BGMPlayerPrefTag, 50f);
            bgmAudio.volume = volume / 100;
            volume = PlayerPrefs.GetFloat(Consts.EffectPlayerPrefTag, 80f);
            effectAudio.volume = volume / 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBgm()
    {
        float volume = slider.value;
        PlayerPrefs.SetFloat(Consts.BGMPlayerPrefTag, volume);
        PlayerPrefs.Save();
        bgmAudio.volume = volume / 100;
        valueText.text = ((int)(bgmAudio.volume * 100)).ToString();
    }

    public void SetEffect()
    {
        float volume = slider.value;
        PlayerPrefs.SetFloat(Consts.EffectPlayerPrefTag, volume);
        PlayerPrefs.Save();
        effectAudio.volume = volume / 100;
        valueText.text = ((int)(effectAudio.volume * 100)).ToString();
    }
}
