using UnityEngine;

public class TankerMonster : Monster
{
    protected override void Start()
    {
        monsterName = "Tanker";
        maxHealth = 200f;
        attackDamage = 5f;
        moveSpeed = 3f;
        attackRange = 1f;
        base.Start();
    }
}

