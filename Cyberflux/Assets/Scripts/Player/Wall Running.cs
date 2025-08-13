using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("WallRunning")]
    [SerializeField] LayerMask whatIsWall;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float wallRunForce;
    [SerializeField] float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    [SerializeField] float wallCheckDistance;
    [SerializeField] float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    public Transform orientation;
    private PlayerController playerScript;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GetComponent<PlayerController>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(playerScript.wallrunning)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        //Getting Inputs 
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
        {
            //Start Wallrunning
            if(!playerScript.wallrunning)
            {
                StartWallRun();
            }

        }
        else
        {
            if(playerScript.wallrunning)
            {
                StopWallRun();
            }
        }

    }

    private void StartWallRun()
    {
        playerScript.wallrunning = true;
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        //Forward Force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        //Push towards wall
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

    }     

        

    private void StopWallRun()
    {
        playerScript.wallrunning = false;
    }
}
