using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

struct StatsEnemy
{
    int m_TurretHealth;
    int m_TurretDamage;
    float m_TurretBulletSpeed;
    float m_TurretBulletLifeTime;
}

public class Config : MonoBehaviour
{
    public enum EnemyType
    {
        Robot,
        Slime,
        Turtle,
        Citizen
    }

    // public static Config m_instance{get; private set;}
    public static Config m_instance = null;
    [SerializeField] public static float minAngleCrashInto;
    [SerializeField] public static float maxAngleCrashInto;



    [Header("Player")]
    [SerializeField] public static int m_PlayerLives;
    [SerializeField] public static string m_tagPlayer;


    [Header("Common Attribute Enemy")]
    [SerializeField] public static float m_EnemyViewAngle;
    [SerializeField] public static float m_EnemyRadiusExplosion;
    [SerializeField] public static int m_EnemyDamageExplosion;
    [SerializeField] public static float m_EnemyMinIdleTime;
    [SerializeField] public static float m_EnemyMaxIdleTime;


    [Header("Turret")]
    [SerializeField] public static int m_TurretDamage;
    [SerializeField] public static int m_TurretHealth;
    [SerializeField] public static float m_TurretBulletSpeed;
    [SerializeField] public static float m_TurretBulletLifeTime;



    [Header("Robot")]
    [SerializeField] public static int m_RobotDamage;
    [SerializeField] public static int m_RobotHealth;
    [SerializeField] public static float m_RobotBulletSpeed;
    [SerializeField] public static float m_RobotBulletLifeTime;
    [SerializeField] public static float m_RobotRangeAtk;




    [Header("PistolEnemy")]
    [SerializeField] public static int m_PistolEnemyDamage;
    [SerializeField] public static int m_PistolEnemyHealth;
    [SerializeField] public static float m_PistolEnemyBulletSpeed;
    [SerializeField] public static float m_PistolEnemyBulletLifeTime;


    [Header("RifleEnemy")]
    [SerializeField] public static int m_RifleEnemyDamage;
    [SerializeField] public static int m_RifleEnemyHealth;
    [SerializeField] public static float m_RifleEnemyBulletSpeed;
    [SerializeField] public static float m_RifleEnemyBulletLifeTime;






    [Header("Barrel")]
    [SerializeField] public static int m_BarrelDamage;
    private void Awake()
    {
        // if(FindObjectsOfType(this.GetType()).Length > 1)
        //  if (m_instance != null && m_instance != this)
        if (FindObjectsByType(this.GetType(), FindObjectsSortMode.None).Length > 1)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        else
        {
            Init();
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }



    void Init()
    {

        //! Player
        m_PlayerLives = 2;
        m_tagPlayer = "Player";

        //Common Attribute Enemy
        m_EnemyViewAngle = 70f;
        m_EnemyRadiusExplosion = 2f;
        m_EnemyDamageExplosion = 100;
        m_EnemyMinIdleTime = 0.5f;
        m_EnemyMaxIdleTime = 2f;

        //! Bullet Turret
        m_TurretDamage = 30;
        m_TurretBulletSpeed = 15f;
        m_TurretBulletLifeTime = 1.5f;



        //! Barrel
        m_BarrelDamage = 100;


        //! Robot
        m_RobotDamage = 20;
        m_RobotHealth = 40;
        m_RobotBulletLifeTime = 2f;
        m_RobotRangeAtk = 1f;


        //! Pistol Enemy
        m_PistolEnemyDamage = 20;
        m_PistolEnemyHealth = 40;
        m_PistolEnemyBulletSpeed = 15f;
        m_PistolEnemyBulletLifeTime = 2f;

        //! Rife Enemy
        m_RifleEnemyDamage = 35;
        m_RifleEnemyHealth = 100;
        m_RifleEnemyBulletSpeed = 15f;
        m_RifleEnemyBulletLifeTime = 2f;











        Debug.Log("Init Config");
    }

    public static float GetRangeATKByEnemyType(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Robot:
                return m_RobotRangeAtk;
            case EnemyType.Slime:
                return 0f;
            case EnemyType.Turtle:
                return 0f;
            case EnemyType.Citizen:
                return 0f;
        }

            return 0f;
    }





}