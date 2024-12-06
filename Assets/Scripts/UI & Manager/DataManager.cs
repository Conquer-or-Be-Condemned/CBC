using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header("About Game")] public static int CurStage = 4;
    
    [Header("Shop")] public static int Coin = 30;

    public const int LEVEL_MAX = 3;

    public static int PlayerHpLv = 1;
    public static int PlayerBulletLv = 1;
    public static int TurretBulletLv = 1;
    public static int TurretMissileLv = 1;
    public static int ControlUnitPowerLv = 1;
    public static int ControlUnitHpLv = 1;

    public static int[] LevelList = { 1, 1, 1, 1, 1, 1 };

    public static int PlayerHp = 200;
    public static int PlayerBullet = 3;
    public static int TurretBullet = 10;
    public static int TurretMissile = 15;
    public static int ControlUnitPower = 200;
    public static int ControlUnitHp = 1500;

    public static int[] CostList = { 10, 20, 30, 40, 50 };

    public static void ApplyLevelingSystem()
    {
        //  Player
        PlayerHp = PlayerHpLv * 50 + 150;
        PlayerBullet = PlayerBulletLv + 2;

        //  Turret
        TurretBullet = TurretBulletLv * 5 + 5;
        TurretMissile = TurretMissileLv * 5 + 10;

        //  Control Unit
        ControlUnitHp = ControlUnitHpLv * 500 + 1000;
        ControlUnitPower = ControlUnitPowerLv * 50 + 150;

        /*
         * Player Hp : 200 250 300 (+ 50)
         * Player Bullet : 3 4 5 (+ 1)
         * Turret Bullet : 10 15 20 (+ 5)
         * Turret Missile : 15 20 25 (+ 5)
         * Control Unit Power : 225 270 315 (+ 45)
         * Control Unit Hp : 1500 2000 2500 (+ 500)
         */
    }

    public static bool Upgrade(int mode)
    {
        if (Coin < CostList[LevelList[mode] - 1])
        {
            Debug.Log("돈 없다 돈 가져와라");
            return false;
        }
        
        if (LevelList[mode] > LEVEL_MAX)
        {
            Debug.LogError("Level Boundary Error");
            return false;
        }

        Coin -= CostList[LevelList[mode] - 1];
        
        LevelList[mode]++;

        ValidateLevelList();
        ApplyLevelingSystem();

        return true;
    }

    public static int GetCost(int mode)
    {
        return CostList[LevelList[mode] - 1];
    }

    public static int GetMargin(int mode)
    {
        int res = 0;
        switch (mode)
        {
            case 0:
                res = 50;
                break;
            case 1:
                res = 1;
                break;
            case 2:
                res = 5;
                break;
            case 3:
                res = 5;
                break;
            case 4:
                res = 500;
                break;
            case 5:
                res = 45;
                break;
        }

        return res;
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