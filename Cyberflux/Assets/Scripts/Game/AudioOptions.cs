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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        masterValue.text = (GameManager.instance.playerScript.masterVol * 100).ToString();
        sfxValue.text = (GameManager.instance.playerScript.sfxVol * 100).ToString();
        musicValue.text = (GameManager.instance.playerScript.musicVol * 100).ToString();
        masterVolSlider.value = GameManager.instance.playerScript.masterVol;
        sfxVolSlider.value = GameManager.instance.playerScript.sfxVol;
        musicVolSlider.value = GameManager.instance.playerScript.musicVol;

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
        GameManager.instance.playerScript.masterVol = value;
    }
    void SFXVolChanged(float value)
    {
        GameManager.instance.playerScript.sfxVol = value;
    }
    void MusicVolChanged(float value)
    {
        GameManager.instance.playerScript.musicVol = value;
    }
}
