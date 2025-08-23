using UnityEngine;

public class CreditMover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform canvas = GetComponentInParent<Canvas>().transform;
      transform.position = new Vector3(canvas.position.x,canvas.position.y,canvas.position.z);  
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
    }
}
