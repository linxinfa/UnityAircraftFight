using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletGenerator
{
    /// <summary>
    /// 根据敌机类型创建子弹
    /// </summary>
    /// <param name="aircraftType">敌机类型</param>
    /// <param name="aircraftPos">敌机坐标</param>
    public static void GenerateBulletByAircraftType(AircraftType aircraftType, Vector3 aircraftPos)
    {
        if (null == s_enemyBulletRoot)
        {
            var rootObj = new GameObject("EnemyBulletRoot");
            s_enemyBulletRoot = rootObj.transform;
        }
        switch (aircraftType)
        {
            case AircraftType.Enemy2:
                {
                    GenerateEnemy2Bullet(aircraftPos);
                }
                break;
            case AircraftType.Enemy3:
                {
                    GenerateEnemy3Bullet(aircraftPos);
                }
                break;
        }
    }

    /// <summary>
    /// 创建敌机2的子弹
    /// </summary>
    /// <param name="aircraftPos"></param>
    private static void GenerateEnemy2Bullet(Vector3 aircraftPos)
    {

        var bullet = CreateBulletObj(aircraftPos);
        bullet.rotateSelf = true;
        var playerPos = GameMgr.instance.GetPlayerPos();
        if(Vector3.zero != playerPos)
            bullet.SetTargetDir(playerPos - aircraftPos);
        else
            bullet.SetTargetDir(-Vector3.up);
    }

    /// <summary>
    /// 创建敌机3的子弹
    /// </summary>
    /// <param name="aircraftPos"></param>
    private static void GenerateEnemy3Bullet(Vector3 aircraftPos)
    {
        for (int i = 0; i < 8; ++i)
        {
            var bullet = CreateBulletObj(aircraftPos);
            bullet.transform.Rotate(0, 0, 180 - 90f / 2 + 90f / 7 * i);
            bullet.SetTargetDir(bullet.transform.up);
            bullet.rotateSelf = false;
        }

    }

    /// <summary>
    /// 创建子弹物体
    /// </summary>
    /// <param name="startPos"></param>
    /// <returns></returns>
    private static EnemyBullet CreateBulletObj(Vector3 startPos)
    {
        EnemyBullet bullet = null;
        if (s_reusePool.Count > 0)
        {
            bullet = s_reusePool.Dequeue();
        }
        else
        {
            var prefab = ResourceMgr.instance.LoadRes<GameObject>("Bullet/enemy_bullet");
            var obj = Object.Instantiate(prefab);
            obj.transform.SetParent(s_enemyBulletRoot, false);
            bullet = obj.GetComponent<EnemyBullet>();
            bullet.backToPoolAction = () =>
            {
                s_reusePool.Enqueue(bullet);
            };
        }
        bullet.speed = 5f;
        bullet.SetStartPos(startPos);
        bullet.SetAngles(Vector3.zero);
        bullet.ActiveSelf(true);
        return bullet;
    }

    /// <summary>
    /// 清理
    /// </summary>
    public static void CLear()
    {
        if(null != s_enemyBulletRoot)
        {
            Object.Destroy(s_enemyBulletRoot.gameObject);
            s_enemyBulletRoot = null;
        }
        s_reusePool.Clear();
    }

    private static Transform s_enemyBulletRoot;
    private static Queue<EnemyBullet> s_reusePool = new Queue<EnemyBullet>();
}
