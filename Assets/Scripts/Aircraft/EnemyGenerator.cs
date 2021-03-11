using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 敌机生成器
/// </summary>
public class EnemyGenerator
{
    /// <summary>
    /// 更新随机的敌机
    /// </summary>
    public void UpdateRandomEnemys()
    {
        m_enemyRandom.Clear();
        foreach (var enemy in GameMgr.instance.Level.Enemies)
            m_enemyRandom.Add(enemy, enemy.Weight);
    }

    public void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= GameMgr.instance.Level.EnemySpawnTime)
        {
            RandomGenerateEnemy();
            m_timer = 0;
        }
    }

    /// <summary>
    /// 清空对象池
    /// </summary>
    public void ClearAll()
    {
        m_reusePool.Clear();
        m_aliveEnemy.Clear();
    }

    /// <summary>
    /// 随机生成一个敌机
    /// </summary>
    private void RandomGenerateEnemy()
    {
        EnemyAircraft enemy = null;
        var config = m_enemyRandom.Next();
        var aircraftType = (AircraftType)config.Index;
        if (m_reusePool.ContainsKey(aircraftType) && m_reusePool[aircraftType].Count > 0)
        {
            enemy = m_reusePool[aircraftType].Dequeue();
            enemy.ActiveSelf(true);
        }
        else
        {
            enemy = (EnemyAircraft)AircraftFactory.CreateAircraft((AircraftType)config.Index);
            enemy.backToPoolAction = () =>
            {
                // 对象回收
                if (!m_reusePool.ContainsKey(aircraftType))
                {
                    m_reusePool[aircraftType] = new Queue<EnemyAircraft>();
                }
                m_reusePool[aircraftType].Enqueue(enemy);

                if (m_aliveEnemy.Contains(enemy))
                    m_aliveEnemy.Remove(enemy);
            };
        }
        enemy.blood = config.Blood;
        enemy.moveSpeed = Random.Range(config.MinSpeed, config.MaxSpeed);
        enemy.ResetTimeToFire(1);
        enemy.RandomStartPos();
        if (!m_aliveEnemy.Contains(enemy))
            m_aliveEnemy.Add(enemy);
    }

    /// <summary>
    /// 全屏炸机
    /// </summary>
    public void KillAllEnemy()
    {
        for (int i = 0; i < m_aliveEnemy.Count; ++i)
        {
            m_aliveEnemy[i].Explode();
        }
        m_aliveEnemy.Clear();
    }

    private float m_timer;
    /// <summary>
    /// 加权随机配置
    /// </summary>
    private readonly WeightedRandom<EnemyConfig> m_enemyRandom = new WeightedRandom<EnemyConfig>();

    /// <summary>
    /// 对象池
    /// </summary>
    private Dictionary<AircraftType, Queue<EnemyAircraft>> m_reusePool = new Dictionary<AircraftType, Queue<EnemyAircraft>>();

    /// <summary>
    /// 屏幕中的敌机
    /// </summary>
    private List<EnemyAircraft> m_aliveEnemy = new List<EnemyAircraft>();
}
