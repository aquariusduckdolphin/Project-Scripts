using UnityEngine;

namespace CombatZone
{
    public enum Team 
    { 
        red = 0, 
        blue = 1,
        none = 2
    }

    [Tooltip("Attak is used for Ability Bots/Ability AI")]
    public enum Behavior 
    { 
        Chase,
        Attack,
        Crouch,
        UseAbility,
        Teabag,
        Retreat,
        Dead,
        None
    }
}