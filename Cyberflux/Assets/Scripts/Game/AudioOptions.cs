using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class AudioOptions : MonoBehaviour
{
    public Slider masterVolSlider;
    public TMP_Text masterValue;
    public Slider sfxVolSlider;
    public TMP_Text sfxValue;
    public Slider musicVolSlider;
    public TMP_Text musicValue;

    AudioClip soundclip;
    [SerializeField] AudioSource audiosource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.LoadSettings();
        masterValue.text = ((int)(GameManager.instance.masterVol * 100)).ToString();
        sfxValue.text = ((int)(GameManager.instance.sfxVol * 100)).ToString();
        musicValue.text = ((int)(GameManager.instance.musicVol * 100)).ToString();
        masterVolSlider.value = GameManager.instance.masterVol;
        sfxVolSlider.value = GameManager.instance.sfxVol;
        musicVolSlider.value = GameManager.instance.musicVol;

        masterVolSlider.onValueChanged.AddListener(MasterVolChanged);
        sfxVolSlider.onValueChanged.AddListener(SFXVolChanged);
        musicVolSlider.onValueChanged.AddListener(MusicVolChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MasterVolChanged(float value)
    {
        GameManager.instance.masterVol = value;
        masterValue.text = ((int)(value * 100)).ToString();
        GameManager.instance.audioMixer.SetFloat("masterVolume", ((value*100) - 80));
      //  GameManager.instance.UIAudioSource.volume = GameManager.instance.playerScript.masterVol * GameManager.instance.playerScript.sfxVol;
        GameManager.instance.UIAudioSource.Play();
        GameManager.instance.SaveSettings();
    }
    void SFXVolChanged(float value)
    {
        
        sfxValue.text = ((int)(value * 100)).ToString();
        GameManager.instance.audioMixer.SetFloat("sfxVolume", ((value * 100) - 80));
      //  GameManager.instance.UIAudioSource.volume = GameManager.instance.playerScript.masterVol * GameManager.instance.playerScript.sfxVol;
        GameManager.instance.UIAudioSource.Play();
        
            GameManager.instance.sfxVol = value;
            
        
        GameManager.instance.SaveSettings();
    }
    void MusicVolChanged(float value)
    {
         GameManager.instance.musicVol = value;
        musicValue.text = ((int)(value * 100)).ToString();
        GameManager.instance.audioMixer.SetFloat("musicVolume", ((value * 100) - 80));
      //  GameManager.instance.UIAudioSource.volume = GameManager.instance.playerScript.masterVol * GameManager.instance.playerScript.sfxVol; ;
        GameManager.instance.UIAudioSource.Play();
        GameManager.instance.SaveSettings();
    }

    public void SwapToControls()
    {
       gameObject.SetActive(false);
        GameManager.instance.optionsControls.SetActive(true);
        GameManager.instance.UIAudioSource.Play();
        GameManager.instance.SaveSettings();
    }
}
