using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage, IHeal
{
    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float slideSpeed;
    [SerializeField] float wallrunSpeed;


   

    [SerializeField] float groundDrag;

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchHeight;
    private float startingHeight;


    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Stats")]
    [SerializeField] int HP;
    

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool slopeExit;


    [SerializeField] Transform orientation;

    float horizontalInput;
    float verticalInput;
    int HPOriginal;

    Vector3 moveDir;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        air
    }

    public bool wallrunning;
    public bool sliding;

    void Start()
    {
        HPOriginal = HP;
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startingHeight = transform.localScale.y;

    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        

        MyInput();
        SpeedControl();
        StateHandler();

        //Handle Drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
            
   
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //ready to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            //Enables continuous jumping by holding donw the key
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        
        //Start Crouch
        if(Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if(Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startingHeight, transform.localScale.z);
        }

    }

    private void StateHandler()
    {

        //WallRunning 
        if(wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }

        //Sliding
        if(sliding)
        {
            state = MovementState.sliding;

            if(OnSlope() && rb.linearVelocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed;
            }

        }

        //Crouching
        else if(Input.GetKeyDown(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        //Sprinting
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Walking
        else if(grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        //In the air
        else
        {
            state = MovementState.air;
        }

        //Check if the desired Speed has change drastically
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            //StartCoroutine(LerpMoveSpeed());
            
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }
    //Still working on this 

    //private IEnumerator LerpMoveSpeed()
    //{
    //    float time = 0;
    //    //Mathf.Abs = Absolute Value
    //    float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
    //    float startValue = moveSpeed;

    //    while (time < difference)
    //    {
    //        moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
    //        time += Time.deltaTime;
    //        yield return null;
    //    }

    //    moveSpeed = desiredMoveSpeed;
    //}


    private void MovePlayer()
    {
        //Calculate Movement Direction
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Debug.Log(moveSpeed);
        //Slope Handling

        if (OnSlope() && !slopeExit)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDir) * moveSpeed * 20f, ForceMode.Force);

            if(rb.linearVelocity.y < 0)
            {               
                rb.AddForce(Vector3.down * 100f, ForceMode.Force);
            }

        }              
        //Just like controller.Move()
        if (grounded)
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        else if (!grounded)
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //Turn off Gravity when on a slope (No more slidey slide)
        rb.useGravity = !OnSlope();
    }

    //To make sure you can't go faster than the selected move speed
    private void SpeedControl()
    {
        if(OnSlope() && !slopeExit)
        {
            //Making sure you can't move faster on a slope
            if(rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }          
           
        }
        //Making sure that you can't move faster on the ground
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            //Limit Velocity if you need to 
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        slopeExit = true;
        //Reset Y vel
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        //Impulse since it is a sinle action
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }


    private void ResetJump()
    {
        readyToJump = true;

        slopeExit = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    
    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    //Damage Interface

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
    }

    public int GetHP()
    {
        return HP;
    }

    public void TakeSlow()
    {
        walkSpeed /= 2;
        sprintSpeed /= 2;
        slideSpeed /= 2;
        crouchSpeed /= 2;
    }

    public void RemoveSlow()
    {
        walkSpeed *= 2;
        sprintSpeed *= 2;
        slideSpeed *= 2;
        crouchSpeed *= 2;
    }

    public bool Heal(int amount)
    {
        if (HP < HPOriginal)
        {
            //StartCoroutine(HealFlashScreen());
            HP += amount;

            if (HP > HPOriginal)
            {
                HP = HPOriginal;
            }
           // updatePlayerUI();
            return true;
        }
        return false;
    }
}
