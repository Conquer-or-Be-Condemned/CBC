using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //  기본 빌드용 값 1 / 개발 빌드용 값 4
    [Header("About Game")] public static int CurStage = 1;
    
    [Header("Shop")] public static int Coin = 0;

    public const int LEVEL_MAX = 3;

    public static int PlayerHpLv = 0;
    public static int PlayerBulletLv = 0;
    public static int TurretBulletLv = 0;
    public static int TurretMissileLv = 0;
    public static int ControlUnitPowerLv = 0;
    public static int ControlUnitHpLv = 0;

    public static int[] LevelList = { 0, 0, 0, 0, 0, 0 };

    public static int[] MarginList = { 75, 1, 4, 40, 500, 40 };

    public static int PlayerHp = 400;
    public static int PlayerBullet = 3;
    public static int TurretBullet = 12;
    public static int TurretMissile = 120;
    public static int ControlUnitPower = 250;
    public static int ControlUnitHp = 1500;

    public static int[] CostList = { 35, 50, 65, 0, 0 };

    public static void ApplyLevelingSystem()
    {
        //  Player
        PlayerHp = PlayerHpLv * MarginList[0] + 400;
        PlayerBullet = PlayerBulletLv *MarginList[1] + 3;

        //  Turret
        TurretBullet = TurretBulletLv * MarginList[2] + 12;
        TurretMissile = TurretMissileLv * MarginList[3] + 110;

        //  Control Unit
        ControlUnitHp = ControlUnitHpLv * MarginList[4] + 1500;
        ControlUnitPower = ControlUnitPowerLv * MarginList[5] + 250;

        /*
         * Player Hp : 400 475 550 625 (+ 75)
         * Player Bullet : 3 4 5 6 (+ 1)
         * Turret Bullet : 12 16 20 24 (+ 4)
         * Turret Missile : 110 150 190 230 (+ 40)
         * Control Unit Power : 250 290 330 370 (+ 40)
         * Control Unit Hp : 1500 2000 2500 3000 (+ 500)
         */
    }

    public static bool Upgrade(int mode)
    {
        if (Coin < CostList[LevelList[mode]])
        {
            // Debug.Log("돈 없다 돈 가져와라");
            return false;
        }
        
        if (LevelList[mode] > LEVEL_MAX)
        {
            // Debug.LogError("Level Boundary Error");
            return false;
        }

        Coin -= CostList[LevelList[mode]];
        
        LevelList[mode]++;

        ValidateLevelList();
        ApplyLevelingSystem();

        return true;
    }

    public static int GetCost(int mode)
    {
        return CostList[LevelList[mode]];
    }

    public static int GetMargin(int mode)
    {
        return MarginList[mode];
    }

    public static void ValidateLevelList()
    {
        PlayerHpLv = LevelList[0];
        PlayerBulletLv = LevelList[1];
        TurretBulletLv = LevelList[2];
        TurretMissileLv = LevelList[3];
        ControlUnitPowerLv = LevelList[4];
        ControlUnitHpLv = LevelList[5];
    }
}