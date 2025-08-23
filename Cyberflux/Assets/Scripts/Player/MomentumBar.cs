using UnityEngine;
using UnityEngine.UI;


public class MomentumBar : MonoBehaviour
{

    [SerializeField] Slider slider;

    private PlayerController playerScript;
    
    [SerializeField] ScytheCombat scytheScript;

    [SerializeField] AudioClip clip;
    private AudioSource audioPlayer;


    private float horizontalInput;
    private float verticalInput;

    private bool playedSound = false;



    void Start()
    {
        playerScript = GameManager.instance.GetComponent<PlayerController>();
        audioPlayer = GameManager.instance.playerScript.audioPlayer;

    }

    
    void Update()
    {
        //Needed to see if the player is moving or not.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        scytheScript.OnSpecialAttack += ResetBar;


        if ((horizontalInput != 0 || verticalInput != 0))
        {
            //0.0003f
            slider.value += 0.0003f;
        }

        if (slider.value >= slider.maxValue)
        {
            scytheScript.specialAttackReady = true;
            
        }
        else
        {
            scytheScript.specialAttackReady = false;
            
        }


        if (slider.value == slider.maxValue && !playedSound)
        {
            audioPlayer.PlayOneShot(clip);
            playedSound = true;
        }
        if (slider.value < slider.maxValue)
        {
            playedSound = false;
        }

    }

    void ResetBar()
    {
        slider.value = 0f;
        
    }

}
