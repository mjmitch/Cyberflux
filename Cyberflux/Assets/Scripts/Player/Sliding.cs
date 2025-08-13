using JetBrains.Annotations;
using UnityEngine;

public class Sliding : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerObject;
    private Rigidbody rb;
    private PlayerController playerScript;

    [Header("Sliding")]
    [SerializeField] float maxSlideTime;
    [SerializeField] float slideForce;
    private float slideTimer;

    [SerializeField] float slideHeight;
    private float startingHeight;


    [Header("Input")]
    [SerializeField] KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GetComponent<PlayerController>();

        //Grab the height
        startingHeight = playerObject.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0) && playerScript.grounded)
        {
            StartSlide();
        }

        if(Input.GetKeyUp(slideKey) && playerScript.sliding) 
        {
            StopSlide();
        }
    }
    
    //Fixed update is for Physics, not input handling
    private void FixedUpdate()
    {
        if(playerScript.sliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        playerScript.sliding = true;

        playerObject.localScale = new Vector3(playerObject.localScale.x, slideHeight, playerObject.localScale.z);
        //Add down force so player doesn't float after changing height
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Normal Sliding
        if (!playerScript.OnSlope() || rb.linearVelocity.y > -0.1f)
        {
            rb.AddForce(inputDir.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        //If sliding on a slope
        else
        {
            rb.AddForce(playerScript.GetSlopeMoveDirection(inputDir) * slideForce, ForceMode.Force);
        }


        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        playerScript.sliding = false;

        playerObject.localScale = new Vector3(playerObject.localScale.x, startingHeight, playerObject.localScale.z);
    }
}
