using System.Collections;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    public float sensX;
    public float sensY;

    public Camera cam;

    public Transform orientation;

    float xRotation;
    float yRotation;


    [Header("Weapon Animations")]
    [SerializeField] ScytheCombat scytheScript;
    [SerializeField] Animator scytheAnimator;

    private void Start()
    {
        //Keeps cursor locked and turns it off
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        //Store the Rotation from the mouse input
        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotate Camera and oriendtataion
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);


        //Animations 
        ScytheAnimations();

    }


    private void ScytheAnimations()
    {
        if(Input.GetKeyDown(scytheScript.attackKey))
        {
            scytheAnimator.SetTrigger("onAttack");
        }
        
        if(Input.GetKeyDown(scytheScript.slashKey))
        {
            scytheAnimator.SetTrigger("onSlash");
        }
        
        if(Input.GetKeyDown(scytheScript.specialAttackKey))
        {
            scytheAnimator.SetTrigger("onMomentum");
        }
    }


}
