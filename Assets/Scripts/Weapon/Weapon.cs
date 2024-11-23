using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float coolDown;
    
    public bool canFire;
    
    //isPlayer is only true for the weapons used by the player
    public virtual void Fire(Transform shootpoint,float range,bool isPlayer)
    {

    }
}
