using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlOptions : MonoBehaviour
{
    public Slider sensXSlider;
    public TMP_Text sensXValue;
    public Slider sensYSlider;
    public TMP_Text sensYValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
        
            sensXValue.text = (PlayerPrefs.GetFloat("SensX", 5)).ToString();
            sensXSlider.value = PlayerPrefs.GetFloat("SensX", 5);
            sensYValue.text = (PlayerPrefs.GetFloat("SensY", 5)).ToString(); 
            sensYSlider.value = PlayerPrefs.GetFloat("SensY", 5);
            sensXSlider.onValueChanged.AddListener(SensXChanged);
            sensYSlider.onValueChanged.AddListener(SensYChanged);
        
    }
    private void Update()
    {
        
    }
    public void SensXChanged(float sens)
    {
        if (GameManager.instance.playerScript != null)
        GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().sensX = 100 * sens;

        PlayerPrefs.SetFloat("SensX", sens);
        PlayerPrefs.Save();

        sensXValue.text = sens.ToString();
    }
    public void SensYChanged(float sens)
    {
        if (GameManager.instance.playerScript != null)
            GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().sensY = 100 * sens;

        PlayerPrefs.SetFloat("SensY", sens);
        PlayerPrefs.Save();

        sensYValue.text = sens.ToString();
    }

    public void SwapToAudio()
    {
        gameObject.SetActive(false);
        GameManager.instance.optionsAudio.SetActive(true);
        GameManager.instance.UIAudioSource.Play();
    }


}
