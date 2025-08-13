using UnityEngine;

public interface IDamage
{
    void TakeDamage(int dmg);
    void TakeSlow();
    void RemoveSlow();
    int GetHP();
}
