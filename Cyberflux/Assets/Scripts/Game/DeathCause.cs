using UnityEngine;

public class DeathCause : MonoBehaviour
{

    [Tooltip("What should the Lose Menu say if this kills the player?")]
    public string causeMessage = "Unknown";

    public string GetMessage()
    {
        return causeMessage;
    }
}
