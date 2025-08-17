using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PlayerItems : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public List<Augment> playeritems;

    public void ReloadItems()
    {
        for (int i = 0; i < playeritems.Count; i++)
        {
            playeritems[i].OnPickup();
        }
    }
}
