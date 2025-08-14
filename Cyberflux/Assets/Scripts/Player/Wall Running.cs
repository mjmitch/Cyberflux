using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("WallRunning")]
    [SerializeField] LayerMask whatIsWall;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float wallRunForce;
    [SerializeField] float wallJumpUpForce;
    [SerializeField] float wallJumpSideForce;
    [SerializeField] float wallClimbSpeed;
    [SerializeField] float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode upwardsRunKey = KeyCode.LeftShift;
    [SerializeField] KeyCode downwardsRunKey = KeyCode.LeftControl;
    bool upwardsRunning;
    bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    [SerializeField] float wallCheckDistance;
    [SerializeField] float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    public bool wallLeft;
    public bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    [SerializeField] float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    [SerializeField] float gravityCounterForce;


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

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        //State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            //Start Wallrunning
            if(!playerScript.wallrunning)
            {
                StartWallRun();
            }

            if(wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }

            if(wallRunTimer <=0 && playerScript.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if(Input.GetKeyDown(jumpKey))
            {
                WallJump();
            }

        }
        //State 2
        else if(exitingWall)
        {
            if(playerScript.wallrunning)
            {
                StopWallRun();
            }

            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            if(exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }

        //State 3
        else
        {
            if (playerScript.wallrunning)
            {
                StopWallRun();
            }
        }

    }

    private void StartWallRun()
    {
        playerScript.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;
        

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        //Forward Force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        
        
        //If you Uncomment these lines it will add back the controlled Wall Running
        //if(upwardsRunning)
        //{
        //    rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbSpeed, rb.linearVelocity.z);
        //}
        //if (downwardsRunning)
        //{
        //    rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallClimbSpeed, rb.linearVelocity.z);
        //}
        //Push towards wall
        
        
        
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        //weaken the gravity so it won't instantly pull you down
        if(useGravity)
        {
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }

    }     

    private void StopWallRun()
    {
        playerScript.wallrunning = false;
    }

    private void WallJump()
    {
        //Exiting Wall State
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //Reset the Y vel and add force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }


}
