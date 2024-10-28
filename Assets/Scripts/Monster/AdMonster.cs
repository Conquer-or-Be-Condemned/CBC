using UnityEngine;

public class AdMonster : Monster
{
    protected override void Start()
    {
        monsterName = "AD";
        maxHealth = 80f;
        attackDamage = 12f;
        moveSpeed = 4f;
        attackRange = 5f;
        base.Start();
    }

    protected override void Attack()
    {
        // 원거리 공격 로직 구현
        // 여기에 원거리 공격에 대한 특별한 로직을 추가할 수 있습니다.
    }
}