using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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
    public float movementMult = 1;
    [Range(0.01f, 0.99f)] [SerializeField] private float slowModifier;
    public Vector3 gravityOrig;
    float walkSpeedOriginal;
    float sprintSpeedOriginal;
    float slideSpeedOriginal;
    float wallrunSpeedOriginal;
    float dashSpeedOriginal;
    float crouchSpeedOriginal;

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
    public ScytheCombat scytheScript;
    


    [SerializeField] float groundDrag;

    [Header("Jumping")]
    [SerializeField] int jumpCount;
    public float jumpForce;
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
    public PlayerStats stats;
    [SerializeField] int HP;

    [Header("Stat Modifiers")]
    //  [SerializeField] float sdf;
    public float dmgReduction = 0;

    [Header("Item Stuff")]
    public int keys = 0;
    [SerializeField] public List<Augment> playerItems;
    public bool brokenClock = false;
    public bool overConfident = false;


    [Header("Ground Check")]
    public float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool slopeExit;

    [Header("Audio")]
    [Range(0.25f, 1f)][SerializeField] private float footstepDelay;
    [SerializeField] public AudioSource audioPlayer;
    [SerializeField] AudioClip[] audDeath;
    [SerializeField] AudioClip[] audJump;
    [SerializeField] AudioClip[] audHurt;
    [SerializeField] AudioClip[] audStep;
    [SerializeField] AudioClip[] audDash;
    [SerializeField] AudioClip audSlide;
    [SerializeField] AudioClip audWallrun;
    private float originalDashDelay;
    private float dashTimer;
    private float originalFootstepDelay;
    private float footstepTimer;


    //[Range(0,1)] [SerializeField] public float masterVol;
    //[Range(0, 1)][SerializeField] public float musicVol;
    //[Range(0, 1)][SerializeField] public float sfxVol;


    
    

    [SerializeField] Transform orientation;

    float horizontalInput;
    float verticalInput;


    private float deathTimer;

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
        //If player is on Main Menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }
        stats.ResetAllStats();
        HP = stats.maxHealth;
        
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startingHeight = transform.localScale.y;
        NormalFov = cam.fieldOfView;
        cam = Camera.main.GetComponent<Camera>();
        wallRunScript = this.GetComponent<WallRunning>();
        gravityOrig = Physics.gravity;
       // scytheScript = GetComponentInChildren<ScytheCombat>();


        originalFootstepDelay = footstepDelay;
        originalDashDelay = 5f;


        walkSpeedOriginal = walkSpeed;
        sprintSpeedOriginal = sprintSpeed;
        slideSpeedOriginal = slideSpeed;
        wallrunSpeedOriginal = wallrunSpeed;
        dashSpeedOriginal = dashSpeed;
        crouchSpeedOriginal = crouchSpeed;

        LoadKeyBinds();
        LoadItems();
        
    }

    // Update is called once per frame
    void Update()
    {
        //If player is on Main Menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);


        if (GameManager.instance.playerScript != null)
        {
            MyInput();
            SpeedControl();
            StateHandler();
        }

        //Handle Drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }

        //if (horizontalInput == 0 && verticalInput == 0)
        //{
        //    deathTimer = Time.time;
        //    DamageIfStill(1);
        //}

        //if (horizontalInput != 0 || verticalInput != 0)
        //{
        //    deathTimer = 0f;
        //}
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

        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.instance.ShowTutorial("Press Shift to Sprint!", 3f);
        }
    }

    private void FixedUpdate()
    {
        //If player is on Main Menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        footstepTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        UpdateFootstepDelay();



        //ready to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
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

        if(grounded)
        {
            if(horizontalInput != 0 ||  verticalInput != 0)
            {
                if(footstepTimer >= footstepDelay)
                {
                    PlayFootstep();
                    footstepTimer = 0f;
                }
            }
            else
            {
                footstepTimer = 0f;
            }
        }
       
        if(dashing)
        {
            if (dashTimer >= 2f)
            {
                PlayDash();
                dashTimer = 0f;
            }
        }
    }

    private void StateHandler()
    {
        
        //dashing
        if(dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed * movementMult;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, dashingFov, Time.deltaTime * fovChangeSpeed);
        }

        //WallRunning 
        else if(wallrunning)
        {
            state = MovementState.wallrunning;  
            desiredMoveSpeed = wallrunSpeed * movementMult; 
            //Should be working, I don't know why It wouldn't be 

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
                desiredMoveSpeed = slideSpeed * movementMult;
            }
            else
            {
                desiredMoveSpeed = sprintSpeed * movementMult;
            }

        }

        //Crouching
        else if(Input.GetKeyDown(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed * movementMult;
        }

        // Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sprintingFov, Time.deltaTime * fovChangeSpeed);
            desiredMoveSpeed = sprintSpeed * movementMult;

            
        }

        // Walking
        else if(grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed * movementMult;
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

    private void PlayDash()
    {
        if (audDash.Length > 0)
        {
            audioPlayer.PlayOneShot(audDash[UnityEngine.Random.Range(0, audDash.Length)]);
        }
    }

    private void PlayFootstep()
    {
        if(audStep.Length > 0)
        {
            audioPlayer.PlayOneShot(audStep[UnityEngine.Random.Range(0, audStep.Length)]);
        }    
    }
    void UpdateFootstepDelay()
    {
        if (state == MovementState.sprinting)
            footstepDelay = originalFootstepDelay / 2;
        else if (state == MovementState.walking)
            footstepDelay = originalFootstepDelay;
        else if (state == MovementState.crouching)
            footstepDelay = originalFootstepDelay * 2;
        else if (state == MovementState.sliding)
            footstepDelay = 50f;
        else if (state == MovementState.wallrunning)
            footstepDelay = 50f;
        else
            footstepDelay = originalFootstepDelay;// Default
    }

    private void Jump()
    {
        slopeExit = true;
        //Reset Y vel
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        
        audioPlayer.PlayOneShot(audJump[UnityEngine.Random.Range(0, audJump.Length)]);

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


    

    public void LoadKeyBinds()
    {
        int keycount = 0;
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (vKey.ToString() == PlayerPrefs.GetString("Jump Key", KeyCode.Space.ToString())) {
                jumpKey = vKey;
                keycount++;
            }
            if (vKey.ToString() == PlayerPrefs.GetString("Crouch Key", KeyCode.LeftControl.ToString())) {
                crouchKey = vKey;
                keycount++;
            }
            if (vKey.ToString() == PlayerPrefs.GetString("Sprint Key", KeyCode.LeftShift.ToString())) {
                sprintKey = vKey;
                keycount++;
            }
            if (vKey.ToString() == PlayerPrefs.GetString("Attack Key", KeyCode.Mouse0.ToString())) {
                scytheScript.attackKey = vKey;
                keycount++;
            }
            if (vKey.ToString() == PlayerPrefs.GetString("Slash Key", KeyCode.Q.ToString())) {
                scytheScript.slashKey = vKey;
                keycount++;
            }
            if (vKey.ToString() == PlayerPrefs.GetString("Slam Key", KeyCode.Mouse1.ToString())) {
                scytheScript.slamAttackKey = vKey;
                keycount++;
            }
            if (vKey.ToString() == PlayerPrefs.GetString("Special Key", KeyCode.Mouse2.ToString())) {
                scytheScript.specialAttackKey = vKey;
                keycount++;
            }
            if (keycount == 7)
            {
                break;
            }
        }
    }
    //public void SaveSettings()
    //{
    //    PlayerPrefs.SetFloat("Master Volume", masterVol);
    //    PlayerPrefs.SetFloat("SFX Volume", sfxVol);
    //    PlayerPrefs.SetFloat("Music Volume", musicVol);
    //}

    //public void LoadSettings()
    //{
    //    masterVol = PlayerPrefs.GetFloat("Master Volume", 50);
    //    sfxVol = PlayerPrefs.GetFloat("SFX Volume", 50);
    //    musicVol = PlayerPrefs.GetFloat("Music Volume", 50);
    //}


    //Damage Interface

    //public void DamageIfStill(int dmg)
    //{

    //    if(deathTimer >= 3f)
    //    {
    //        TakeDamage(dmg, "You Can't Stay Still Homie");
    //    }
        
    //}

    public void TakeDamage(int dmg)
    {
        TakeDamage(dmg, "Unknown");
    }

    // 2) Custom, adds cause of death text
    public void TakeDamage(int dmg, string cause)
    {
        float temp = dmg * (1 - dmgReduction);
        HP -= (int)temp;
        if (HP < 0) HP = 0;

        stats.currentHealth = HP;
        GameManager.instance.UpdateHealthUI(HP, stats.maxHealth);
        audioPlayer.PlayOneShot(audHurt[UnityEngine.Random.Range(0, audHurt.Length)]);

        if (HP <= 0)
        {
            audioPlayer.PlayOneShot(audDeath[UnityEngine.Random.Range(0, audDeath.Length)]);
            GameManager.instance.YouLose(cause);
            GameManager.instance.SaveSettings();
        }
    }
    public int GetHP()
    {
        return HP;
    }

    public int GetMaxHP()
    {
        return stats.maxHealth;
    }

    public void TakeSlow()
    {
        walkSpeed *= slowModifier;
        sprintSpeed *= slowModifier;
        slideSpeed *= slowModifier;
        crouchSpeed *= slowModifier;
        wallrunSpeed += slowModifier;
        dashSpeed *= slowModifier;
        StartCoroutine(ResetSpeeds());
    }

    IEnumerator ResetSpeeds()
    {
        yield return new WaitForSeconds(2);
        RemoveSlow();
    }

    public void RemoveSlow()
    {
        walkSpeed = walkSpeedOriginal;
        sprintSpeed = sprintSpeedOriginal;
        slideSpeed = slideSpeedOriginal;
        crouchSpeed = crouchSpeedOriginal;
        wallrunSpeed = wallrunSpeedOriginal;
        dashSpeed = dashSpeedOriginal;
    }

    public bool Heal(int amount)
    {
        if (HP < stats.maxHealth)
        {
            HP += amount;
            if (HP > stats.maxHealth) HP = stats.maxHealth;

            GameManager.instance.UpdateHealthUI(HP, stats.maxHealth);
            return true;
        }
        return false;
    }

    public void SetMaxHP(int val)
    {
        stats.maxHealth = stats.maxHealth + val;
        stats.currentHealth += val;
        HP += val;
    }

    public void IncreaseDMG(int val)
    {
        scytheScript.attackDamage += val;
    }

    public void AddItem(Augment item)
    {
       playerItems.Add(item);

        item.OnPickup();
        
        if (!item.multipleCopies)
        {
            GameManager.instance.itemPool.Remove(item);
        }
    }

    public void SaveItem()
    {
       
        if (SceneManager.GetActiveScene().buildIndex > 0)
            PlayerPrefs.SetInt("Item " + (SceneManager.GetActiveScene().buildIndex - 1) + " ID", playerItems[SceneManager.GetActiveScene().buildIndex-1].ID);
        
    }
    public void LoadItems()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 )
        {
            return;
        }
        for (int i = 0;i < SceneManager.GetActiveScene().buildIndex - 1 ;i++)
        {
            int id = PlayerPrefs.GetInt("Item " + i + " ID", 0);
            if (id != 0)
            {
                if (GameManager.instance.itemPool.Find(x => x.GetID() == id))
                AddItem(GameManager.instance.itemPool.Find(x => x.GetID() == id));
               
            }
        }
    }

}
