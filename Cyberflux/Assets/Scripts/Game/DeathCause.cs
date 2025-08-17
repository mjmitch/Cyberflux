using UnityEngine;

public class DeathCause : MonoBehaviour
{

    [Header("What should the Lose Menu say if this kills the player?")] 
    [SerializeField] private string causeMessage;

    public string GetMessage()
    {
        return causeMessage;
    }
}
