using Unity.VisualScripting;
using UnityEngine;

public class OverConfident : Augment
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public override void OnPickup()
    {
        GameManager.instance.playerScript.overConfident = true;
    }
}
