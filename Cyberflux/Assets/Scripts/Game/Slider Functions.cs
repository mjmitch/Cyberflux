using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderFunctions : MonoBehaviour
{
    public Slider slider;
    public TMP_Text value;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);
    }
    private void Update()
    {
        
    }
    public void OnValueChanged(float sens)
    {
        GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().sensX = 400 * sens;
        GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>().sensY = 400 * sens;
        float temp = sens * 400 / 12;
        value.text = ((int)temp).ToString();
    }

    
}
