using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Slider bgmSlider;
    public AudioSource bgmAudio;
    public Text bgmText;

    public Slider effectSlider;
    public AudioSource effectAudio;
    public Text effectText;
    // Start is called before the first frame update
    void Start()
    {
        float volume = PlayerPrefs.GetFloat(Consts.BGMPlayerPrefTag, 50);
        bgmSlider.value = volume;
        SetBGMVolume();

        volume = PlayerPrefs.GetFloat(Consts.EffectPlayerPrefTag, 80);
        effectSlider.value = volume;
        SetEffectVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBGMVolume()
    {
        float volume = bgmSlider.value;
        PlayerPrefs.SetFloat(Consts.BGMPlayerPrefTag, volume);
        PlayerPrefs.Save();
        bgmAudio.volume = volume/100;
        bgmText.text = ((int)(bgmAudio.volume * 100)).ToString();
    }

    public void SetEffectVolume()
    {
        float volume = effectSlider.value;
        PlayerPrefs.SetFloat(Consts.EffectPlayerPrefTag, volume);
        PlayerPrefs.Save();
        effectAudio.volume = volume / 100;
        effectText.text = ((int)(effectAudio.volume * 100)).ToString();
    }
}
