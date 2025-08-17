using UnityEngine;

public class DMGUp : Augment
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnPickup()
    {
        GameManager.instance.playerScript.IncreaseDMG(5);
    }
}
