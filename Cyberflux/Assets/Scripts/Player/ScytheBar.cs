using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UI;

public class ScytheBar : MonoBehaviour
{

    [SerializeField] Slider slider;
    [SerializeField] AudioClip clip;
    [SerializeField] ScytheCombat scytheScript;

    private AudioSource audioPlayer;
    private bool playedSound = false;

    private void Start()
    {
        scytheScript.OnSlash += ResetBar;
        audioPlayer = GameManager.instance.playerScript.audioPlayer;
    }


    private void Update()
    {
        float timeSinceLastSlash = Time.time - (scytheScript.nextSlashTime - scytheScript.slashRechargeTime);
        slider.value = Mathf.Clamp01(timeSinceLastSlash / scytheScript.slashRechargeTime);
        
        if(slider.value == slider.maxValue && !playedSound)
        {
            audioPlayer.PlayOneShot(clip);
            playedSound = true;
        }
        if(slider.value < slider.maxValue)
        {
            playedSound = false;
        }
    }

    void ResetBar()
    {
        slider.value = 0f;
    }
}
