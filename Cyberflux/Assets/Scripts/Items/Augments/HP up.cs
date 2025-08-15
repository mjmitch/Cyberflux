using Unity.VisualScripting;
using UnityEngine;

public class HPup : Augment
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
   
    public override void OnPickup()
    {
        GameManager.instance.playerScript.SetMaxHP(50);
    }
}
