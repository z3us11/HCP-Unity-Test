using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants 
{

    //Layers and Tags
    public const string playerLayer="Player";
    public const string enemyLayer="Enemy";
    public const string playerTag = "Player";
    public const string patrolTag = "Patrol";// Hacky workaround instead of refactoring collision for other enemies

    //Attack
    public const int meleeAttackLimit = 3;
    public const string attackTrigger="Attack";

    public const int startLevel = 2;
    public const int endLevel = 5;
    public const int thanksLevel = 1;
}
