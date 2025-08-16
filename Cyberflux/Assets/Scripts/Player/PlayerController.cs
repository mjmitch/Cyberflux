using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;
using UnityEngine.UI; 

public class PlayerController : MonoBehaviour, IDamage, IHeal
{
    [Header("Movement")]
    private float moveSpeed;
    public float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float slideSpeed;
    [SerializeField] float wallrunSpeed;
    [SerializeField] float dashSpeed;
    [Range(0.01f, 0.99f)] [SerializeField] private float slowModifier;
    public Vector3 gravityOrig;
    

    [Header("Camera")]
    public float NormalFov;
    public Camera cam;
    [SerializeField] float slidingFOV;
    [SerializeField] float wallRunningFov;
    [SerializeField] float sprintingFov;
    [SerializeField] float dashingFov;
    public float fovChangeSpeed;
    private float TargetFov;
    [SerializeField] float tiltAmount;
    [SerializeField] float tiltTime;
    private float currentTilt;
    private float targetTilt;
    private Coroutine tiltRoutine;
    private WallRunning wallRunScript;
    private ScytheCombat scytheScript;
    


    [SerializeField] float groundDrag;

    [Header("Jumping")]
    [SerializeField] int jumpCount;
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

    [Header("Stat Modifiers")]
    //  [SerializeField] float sdf;

    [Header("Item Stuff")]
    public int keys = 0;
    [SerializeField] public List<Augment> playerItems;
    public bool brokenClock = false;


    [Header("Ground Check")]
    public float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool slopeExit;

    [Header("Audio")]
    [SerializeField] public AudioSource audioPlayer;
   // [SerializeField] public AudioMixer audioMixer;
    //[SerializeField] public AudioMixerGroup masterVol;
    //[SerializeField] public AudioMixerGroup musicVol;
    //[SerializeField] public AudioMixerGroup sfxVol;
   
    //Dont know what all audio we are going to have, just putting these here for now.
    [SerializeField] AudioClip[] audJump;
    [SerializeField] AudioClip[] audHurt;
    [SerializeField] AudioClip[] audStep;


    [Range(0,1)] [SerializeField] public float masterVol;
    [Range(0, 1)][SerializeField] public float musicVol;
    [Range(0, 1)][SerializeField] public float sfxVol;

    

    [SerializeField] Transform orientation;

    float horizontalInput;
    float verticalInput;
    
 
    private int stamina;
    private int staminaMax = 100;
    int HPOriginal;

    Vector3 moveDir;

    Rigidbody rb;

    [Header("Movement States")]

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        dashing,
        air
    }

    public bool wallrunning;
    public bool sliding;
    public bool sprinting;
    public bool dashing;
    

    void Start()
    {
        HPOriginal = HP;
        stamina = staminaMax;
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startingHeight = transform.localScale.y;
        NormalFov = cam.fieldOfView;
        cam = Camera.main.GetComponent<Camera>();
        wallRunScript = this.GetComponent<WallRunning>();
        gravityOrig = Physics.gravity;
        

        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(grounded);


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

        // TEMP: test health bar by pressing H to take 10 damage
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }

        // TEMP: test heal by pressing J to heal 5 HP
        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(5);
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

        //dashing
        if(dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, dashingFov, Time.deltaTime * fovChangeSpeed);
        }

        //WallRunning 
        else if(wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;

            if (wallRunScript.wallRight)
            {
                cam.transform.Rotate(0, 0, tiltAmount);
            }
            else if (!wallRunScript.wallRight)
            {
                cam.transform.Rotate(0, 0, -tiltAmount);
            }

            if (wallRunScript.wallLeft)
            {
                cam.transform.Rotate(0, 0, -tiltAmount);
            }
            else if (!wallRunScript.wallLeft)
            {
                cam.transform.Rotate(0, 0, tiltAmount);
            }

            //Below is the In-Progress Lerp Code

            //if (wallRunScript.wallRight)
            //{
            //    targetTilt = tiltAmount;
            //}
               
            //else if (wallRunScript.wallLeft)
            //{
            //    targetTilt = -tiltAmount;
            //}
                

            //// Smoothly approach target every frame
            //currentTilt = Mathf.MoveTowards(currentTilt, targetTilt, tiltTime * Time.deltaTime);

            //// Apply to camera
            //Vector3 camEuler = cam.transform.localEulerAngles;
            //camEuler.z = currentTilt;
            //cam.transform.localEulerAngles = camEuler;

        }

        //Sliding
        else if(sliding)
        {
            state = MovementState.sliding;

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, slidingFOV, Time.deltaTime * fovChangeSpeed);

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

        // Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sprintingFov, Time.deltaTime * fovChangeSpeed);
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

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, NormalFov, Time.deltaTime * fovChangeSpeed);

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
        if(!wallrunning) rb.useGravity = !OnSlope();
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
        if (HP < 0) HP = 0;


        GameManager.instance.UpdateHealthUI(HP, HPOriginal);

        if (HP <= 0)
            GameManager.instance.YouLose();
    }

    public int GetHP()
    {
        return HP;
    }

    public void TakeSlow()
    {
        walkSpeed *= slowModifier;
        sprintSpeed *= slowModifier;
        slideSpeed *= slowModifier;
        crouchSpeed *= slowModifier;
    }

    public void RemoveSlow()
    {
        walkSpeed /= slowModifier;
        sprintSpeed /= slowModifier;
        slideSpeed /= slowModifier;
        crouchSpeed /= slowModifier;
    }

    public bool Heal(int amount)
    {
        if (HP < HPOriginal)
        {
            HP += amount;
            if (HP > HPOriginal) HP = HPOriginal;

            GameManager.instance.UpdateHealthUI(HP, HPOriginal);
            return true;
        }
        return false;
    }

    public void SetMaxHP(int val)
    {
        HPOriginal = HPOriginal + val;
        HP += val;
    }

    public void AddItem(Augment item)
    {
        playerItems.Add(item);
    }
}
