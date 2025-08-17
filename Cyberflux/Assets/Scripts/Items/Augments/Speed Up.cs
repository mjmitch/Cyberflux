using UnityEngine;

public class SpeedUp : Augment
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnPickup()
    {

        GameManager.instance.playerScript.movementMult += .2f;
        
       // GameManager.instance.playerScript.stats.UpdateStats();
    }
}
