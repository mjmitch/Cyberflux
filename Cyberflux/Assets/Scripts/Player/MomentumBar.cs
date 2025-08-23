using UnityEngine;
using UnityEngine.UI;


public class MomentumBar : MonoBehaviour
{

    [SerializeField] Slider slider;

    private PlayerController playerScript;
    
    [SerializeField] ScytheCombat scytheScript;

    private float horizontalInput;
    private float verticalInput;

    public int momemtumMult = 1;

    void Start()
    {
        playerScript = GameManager.instance.GetComponent<PlayerController>();

        scytheScript.OnSpecialAttack += ResetBar;
    }

    
    void Update()
    {
        //Needed to see if the player is moving or not.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if((horizontalInput != 0 || verticalInput != 0))
        {
            //0.0003f
            slider.value += 0.0003f * momemtumMult;
        }

        if (slider.value == slider.maxValue)
        {
            scytheScript.specialAttackReady = true;
        }

    }

    void ResetBar()
    {
        slider.value = 0f;
    }

}
