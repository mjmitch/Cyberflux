using TMPro;
using UnityEngine;

public class ScorePopUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    TextMeshPro text;
    void Start()
    {
        text = transform.GetComponent<TextMeshPro>();
        text.alpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
        transform.Translate(Vector3.up * Time.deltaTime);
        text.color = new Color(text.color.r, text.color.g, text.color.b,text.alpha -.01f);
        if (text.alpha <= 0 )
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        transform.GetComponent<TextMeshPro>().text = text;
    }
}
