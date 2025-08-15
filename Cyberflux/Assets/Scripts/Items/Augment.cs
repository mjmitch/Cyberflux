using UnityEngine;

public abstract class Augment : MonoBehaviour
{
    
    [SerializeField] public Sprite icon;
    [SerializeField] public string itemDescription;
    [SerializeField] public string itemStats;
    [SerializeField] public bool multipleCopies;
    

    
    public abstract void OnPickup();
    

}
