using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{

    public int maxHealth;
    public int currentHealth;
    public float baseMovemntSpeed;
    public float movementSpeed;
    public float baseJumpForce;
    public float jumpForce;
    public float baseSlideSpeed;
    public float slideSpeed;
    public float baseWallrunSpeed;
    public float wallrunningSpeed;


    public void ResetAllStats()
    {
        currentHealth = maxHealth;
        movementSpeed = baseMovemntSpeed;
        jumpForce = baseJumpForce;
        slideSpeed = baseSlideSpeed;
        wallrunningSpeed = baseWallrunSpeed;
    }


}
