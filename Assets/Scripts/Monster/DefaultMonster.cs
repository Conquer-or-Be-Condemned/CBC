using UnityEngine;

public class DefaultMonster : Monster
{
    protected override void Start()
    {
        monsterName = "Default";
        maxHealth = 100f;
        attackDamage = 10f;
        moveSpeed = 5f;
        attackRange = 1f;
        base.Start();
    }
}