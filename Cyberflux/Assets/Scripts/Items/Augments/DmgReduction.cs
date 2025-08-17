using UnityEngine;

public class DmgReduction : Augment
{
    public override void OnPickup()
    {
        GameManager.instance.playerScript.dmgReduction += .15f;
    }
}
