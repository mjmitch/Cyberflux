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
        sensXValue.text = (GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().sensX / 100).ToString();
        sensXSlider.value = (GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().sensX / 100);
        sensYValue.text = (GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().sensY / 100).ToString();
        sensYSlider.value = (GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().sensY / 100);
        sensXSlider.onValueChanged.AddListener(SensXChanged);
        sensYSlider.onValueChanged.AddListener (SensYChanged);
    }
    private void Update()
    {
        
    }
    public void SensXChanged(float sens)
    {
        GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().sensX = 100 * sens;
        
        sensXValue.text = sens.ToString();
    }
    public void SensYChanged(float sens)
    {
        GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().sensY = 100 * sens;

        sensYValue.text = sens.ToString();
    }

    
}
