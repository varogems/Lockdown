using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    public enum BulletType
    {
        BulletTurret = 0,
        BulletRobot,
        BulletHuman,


        None,
        Count,
    };


    public enum PlayerParticleType
    {
        PlayerHitEffect,
        Count
    };

    public enum EnemyParticleType
    {
        TurretHitEffect,
        RobotHitEffect,
        HumanHitEffect,
        ExplosionEffect,
        Count,
        None

    };

    public enum EnemyType
    {
        Barrel = 0,
        Robot,
        PistolEnemy,
        RifeEnemy,
        Count
    }


    //!------------------------------------------------------------------------
    [SerializeField] Transform m_transform;
    [SerializeField] List<GameObject> m_listBullet;
    [SerializeField] List<GameObject> m_listPlayerParticle;
    [SerializeField] List<GameObject> m_listEnemyParticle;
    [SerializeField] List<GameObject> m_listEnemy;


    static List<KeyValuePair<int, GameObject>> m_listBulletPool;
    static List<KeyValuePair<int, GameObject>> m_listPlayerParticlePool;
    static List<KeyValuePair<int, GameObject>> m_listEnemyParticlePool;
    static List<KeyValuePair<int, GameObject>> m_listEnemyPool;

    
    public static PoolManager m_instance {get; private set;}
    

    //!------------------------------------------------------------------------
    void Awake()
    {
            
        if (FindObjectsByType(this.GetType(), FindObjectsSortMode.None).Length > 1)
        {
            Debug.Log("ResetPoolManager");
            ResetPoolManager();

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
        InitPool(ref m_listBulletPool, ref m_listBullet);
        InitPool(ref m_listPlayerParticlePool, ref m_listPlayerParticle);
        InitPool(ref m_listEnemyParticlePool, ref m_listEnemyParticle);
        InitPool(ref m_listEnemyPool, ref m_listEnemy);
        Debug.Log("Done Init PoolManager!");
    }

    void InitPool(ref List<KeyValuePair<int, GameObject>> pool, ref List<GameObject> listSample)
    {
        if (listSample == null)
        {
            Debug.LogWarning("Sample List is NULL!");
            return;
        }

        if (listSample.Count == 0)
        {
            Debug.LogWarning("Sample List is empty!");
            return;
        }

        pool = new List<KeyValuePair<int, GameObject>>();

        GameObject gameObject;
        for (int i = 0; i < listSample.Count; i++)
        {
            if (listSample[i] == null) continue;

            //! Create Gameobject with component ObjectPool
            gameObject = new GameObject();
            gameObject.name = listSample[i].name;
            gameObject.AddComponent<ObjectPool>();
            gameObject.GetComponent<ObjectPool>().setPrefab(listSample[i]);

            //!  Add this gameobject to gameobject with name "PoolManager"
            pool.Add(new KeyValuePair<int, GameObject>(i, gameObject));
            gameObject.transform.SetParent(m_transform);
        }
        gameObject = null;

    }

    void ResetPoolManager()
    {
        ResetPool(ref m_listBulletPool);
        ResetPool(ref m_listPlayerParticlePool);
        ResetPool(ref m_listEnemyParticlePool);
        ResetPool(ref m_listEnemyPool);
    }

    void ResetPool(ref List<KeyValuePair<int, GameObject>> pool)
    {
        for(int i = 0; i < pool.Count; i++)
            pool[i].Value.GetComponent<ObjectPool>()?.Reset();
    }

    public static void SpawnBulletEnemy(BulletType bullet, int layerOwner, GameObject target, GameObject spanwer)
    {
        if(target == null) return;

        GameObject bulletObj = m_listBulletPool[(int)bullet].Value.GetComponent<ObjectPool>().GetPooledObject();

        bulletObj.transform.position = spanwer.transform.position;
        bulletObj.transform.rotation = spanwer.transform.rotation;

        bulletObj.transform.LookAt(target.transform);

        bulletObj.GetComponent<Projectile>().SetInput(layerOwner, bullet, Config.m_TurretDamage, Config.m_TurretBulletSpeed, Config.m_TurretBulletLifeTime);

        bulletObj = null;

    }

    public static void SpawnPlayerHitVfc(PlayerParticleType particleType, RaycastHit raycastHit)
    {
        GameObject particleSystem = m_listPlayerParticlePool[(int)particleType].Value.GetComponent<ObjectPool>().GetPooledObject();

        particleSystem.transform.position = raycastHit.point;
        particleSystem.GetComponent<VFXPlay>().Play();

        particleSystem = null;

    }


    public static void SpawnEnemyHitVfc(EnemyParticleType particleType, RaycastHit raycastHit)
    {
        if (particleType == EnemyParticleType.None) return;

        GameObject particleSystem = m_listEnemyParticlePool[(int)particleType].Value.GetComponent<ObjectPool>().GetPooledObject();

        particleSystem.transform.position = raycastHit.point;
        particleSystem.GetComponent<VFXPlay>().Play();

        particleSystem = null;

    }

    public static void ExplosionEffect(Vector3 postion)
    {
        GameObject particleSystem = m_listEnemyParticlePool[(int)EnemyParticleType.ExplosionEffect].Value.GetComponent<ObjectPool>().GetPooledObject();

        particleSystem.transform.position = postion;
        particleSystem.GetComponent<VFXPlay>()?.Play();

        particleSystem = null;
    }


    public static void SpawnEnemy(EnemyType enemyType, Vector3 pos, Gate gate)
    {
        GameObject enemyObj = m_listEnemyPool[(int)enemyType].Value.GetComponent<ObjectPool>().GetPooledObject();

        enemyObj.GetComponent<ResetScript>()?.Reset();
        enemyObj.GetComponent<EnemyHealth>()?.SetGate(gate);

        Vector3 newPos = new Vector3(pos.x + UnityEngine.Random.Range(0f, 1f),
                                    pos.y + UnityEngine.Random.Range(0f, 1f),
                                    pos.z + UnityEngine.Random.Range(0f, 1f));



        enemyObj.transform.position = newPos;
        

        enemyObj = null;
    }




    // //! Spawn bullet for boss
    // public static void SpawnTwinSlasher(Transform _transformBoss)
    // {
    //     ObjectPool pool = m_listGameObjectPool[(int)GameObjectPoolType.BulletBoss].Value.GetComponent<ObjectPool>();
    //     int numberOfBulletBossSpawn = Random.Range(2, pool.numberOfGameObject());

    //     float angle = 90 / numberOfBulletBossSpawn;

    //     GameObject gameObject;
    //     BulletBoss bulletBoss;
    //     Vector2 vectorProjectile;

    //     for(int i = 0; i < numberOfBulletBossSpawn; i++)
    //     {
    //         vectorProjectile = Vector2.zero;

    //         vectorProjectile.x  =    (Mathf.Sign(_transformBoss.localScale.x) > 0) ? Mathf.Cos(angle * i * Mathf.Deg2Rad): 
    //                                                                                 Mathf.Cos((180f - angle * i)  * Mathf.Deg2Rad);

    //         vectorProjectile.y  =    Mathf.Sin((180f - angle * i)  * Mathf.Deg2Rad);

    //         gameObject                      = pool.GetPooledObject();
    //         gameObject.transform.position   = _transformBoss.position;
    //         gameObject.transform.rotation   = _transformBoss.rotation;
            
    //         bulletBoss = gameObject.GetComponent<BulletBoss>();
    //         bulletBoss.setDirectionVector(vectorProjectile);
    //     }

    //     gameObject = null;
    //     bulletBoss = null;

    // }

    // //! Spawn bullet for player
    // public static void SpawnFireBullets(Transform _transformPlayer)
    // {

    //     GameObject _fireBullet = m_listGameObjectPool[(int)GameObjectPoolType.BulletPlayer].Value.GetComponent<ObjectPool>().GetPooledObject();

    //     Vector3 posAppear = new Vector3(_transformPlayer.position.x + _transformPlayer.localScale.x/2,
    //                                     _transformPlayer.position.y + 1 , 
    //                                     _transformPlayer.position.z);

    //     _fireBullet.transform.position      = posAppear;
    //     _fireBullet.transform.localScale    = _transformPlayer.localScale;
    //     _fireBullet = null;
        
    // }


    // public static void SpawnBulletBill(Transform _transform)
    // {
    //     GameObject BulletBill           = m_listGameObjectPool[(int)GameObjectPoolType.BulletBill].Value.GetComponent<ObjectPool>().GetPooledObject();
    //     BulletBill.GetComponent<MovingGameObject>().RefreshCollider();
    //     BulletBill.transform.position   = new Vector2(_transform.position.x + _transform.localScale.x * 1.2f, _transform.position.y + 1);
    //     BulletBill.transform.localScale = _transform.localScale;
    // }


    // public static void PlayParticleFireShooting(Transform _transform)
    // {
    //     m_instance.StartCoroutine(m_instance.IEPlayParticleFireShooting(_transform));
    // }

    // IEnumerator IEPlayParticleFireShooting(Transform _transform)
    // {

    //     GameObject gameObject           = m_listParticleObjectPool[(int)ParticleObjectPoolType.ParticleFireShooting].Value.GetComponent<ObjectPool>().GetPooledObject();
    //     gameObject.transform.position   = _transform.position;
                                                                
    //     ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
    //     ps.Play();
        
    //     // yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constant);
    //     yield return new WaitForSeconds(ps.main.duration);
    //     ps.Stop();
    //     gameObject.SetActive(false);
    // }

    // public static void PlayParticleFirework(Transform _transform)
    // {
    //     m_instance.StartCoroutine(m_instance.IEPlayParticleFirework(_transform));
    // }

    // IEnumerator IEPlayParticleFirework(Transform _transform)
    // {

    //     Debug.Log("IEPlayParticleFirework");
    //     GameObject gameObject1           = m_listParticleObjectPool[(int)ParticleObjectPoolType.ParticleFireWork1].Value.GetComponent<ObjectPool>().GetPooledObject();
    //     gameObject1.transform.position   = _transform.position;

    //     GameObject gameObject2           = m_listParticleObjectPool[(int)ParticleObjectPoolType.ParticleFireWork2].Value.GetComponent<ObjectPool>().GetPooledObject();
    //     gameObject2.transform.position   = _transform.position;

    //     GameObject gameObject3           = m_listParticleObjectPool[(int)ParticleObjectPoolType.ParticleFireWork3].Value.GetComponent<ObjectPool>().GetPooledObject();
    //     gameObject3.transform.position   = _transform.position;


    //     ParticleSystem ps1, ps2, ps3;

    //     ps1 = gameObject1.GetComponent<ParticleSystem>();
    //     ps1.Play();

    //     ps2 = gameObject2.GetComponent<ParticleSystem>();
    //     ps2.Play();

    //     ps3 = gameObject3.GetComponent<ParticleSystem>();
    //     ps3.Play();

    //     yield return new WaitForSeconds(ps3.main.duration);

    //     ps1.Stop();
    //     ps2.Stop();
    //     ps3.Stop();

    //     gameObject1.SetActive(false);
    //     gameObject2.SetActive(false);
    //     gameObject3.SetActive(false);

    // }



}