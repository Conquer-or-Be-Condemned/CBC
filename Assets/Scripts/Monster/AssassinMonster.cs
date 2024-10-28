using UnityEngine;

public class AssassinMonster : Monster
{
    protected override void Start()
    {
        monsterName = "Assassin";
        maxHealth = 60f;
        attackDamage = 15f;
        moveSpeed = 8f;
        attackRange = 1f;
        base.Start();
    }
}