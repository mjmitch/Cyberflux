using UnityEngine;

public class FastCharge : Augment
{
    public override void OnPickup()
    {
        GameObject.FindWithTag("UI").GetComponentInChildren<MomentumBar>().momemtumMult *= 2;
    }
}
