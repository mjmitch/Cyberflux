using UnityEngine;

public class Ambiance : MonoBehaviour
{

    public AudioSource musicPlayer;

    [SerializeField] AudioClip musicClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // musicPlayer = GetComponent<AudioSource>();


       // musicPlayer.clip = musicClip;
        //musicPlayer.volume = GameManager.instance.playerScript.masterVol * GameManager.instance.playerScript.mus
       // musicPlayer.Play();
    }



}
