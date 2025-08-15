using UnityEngine;

public class Dashing : MonoBehaviour
{


    [Header("Dashing")]
    [SerializeField] float dashForce;
    [SerializeField] float maxDashTime;
    //[SerializeField] int dashMax;
    [SerializeField] float dashCooldown;
    //private int dashCount;
    private float dashTimer;
    private float dashCooldownTimer;



    [Header("Input")]
    [SerializeField] KeyCode dashKey = KeyCode.E;
    //So you can dash in anyway that you'd like
    private float horizontalInput;
    private float verticalInput;


    [Header("References")]
    [SerializeField] Transform orientation;
    PlayerController playerScript;
    Rigidbody rb;

    private void Start()
    {
        playerScript = GetComponent<PlayerController>();   
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        Debug.Log(dashCooldownTimer);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(dashKey) && (horizontalInput != 0 || verticalInput != 0) && dashCooldownTimer <= 0)
        {
            StartDash();
        }

        if (Input.GetKeyUp(dashKey) && playerScript.sliding)
        {
            EndDash();
            
        }

       
        dashCooldownTimer -= Time.deltaTime;
            
            
    }

    private void FixedUpdate()
    {
        if (playerScript.dashing)
        {
            DashMovement();
        }
    }

    private void StartDash()
    {
        playerScript.dashing = true;
        dashTimer = maxDashTime;
        dashCooldownTimer = dashCooldown;
        //dashCount++;
    }


    private void DashMovement()
    {
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Dashing Movement 
        rb.AddForce(inputDir.normalized * dashForce, ForceMode.Impulse);

        dashTimer -= Time.deltaTime;

        if(dashTimer <= 0)
        {
            EndDash();
           
            
        }
    }


    private void EndDash()
    {   
        playerScript.dashing = false;
        
    }


}
