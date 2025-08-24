using UnityEngine;

public abstract class Augment : MonoBehaviour
{
    public int ID;
    [SerializeField] public Sprite icon;
    [SerializeField] public string itemDescription;
    [SerializeField] public string itemStats;
    [SerializeField] public bool multipleCopies;
    

    
    public abstract void OnPickup();
    
    public int GetID()
    {
        return ID;
    }
}
