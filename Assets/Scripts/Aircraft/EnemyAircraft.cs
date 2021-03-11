using UnityEngine;
using System;

/// <summary>
/// 敌机类
/// </summary>
public class EnemyAircraft : BaseAircraft
{
    public float moveSpeed = 0;
    public Action backToPoolAction;
    private bool m_underAttack = false;
    private float m_timeToFire = -1;


    protected override void Awake()
    {
        base.Awake();
        m_aniEvent.aniEventCb = (msg) =>
        {
            if ("explode_finish" == msg)
            {
                BackToPool();
            }
        };
    }

    /// <summary>
    /// 随机一个起始位置
    /// </summary>
    public void RandomStartPos()
    {
        var randomX = UnityEngine.Random.Range(0, Screen.width);
        m_selfTrans.position = Camera.main.ScreenToWorldPoint(new Vector3(randomX, Screen.height, 5));
    }

    private void Update()
    {
        if (GameState.Pause == GameMgr.instance.gameState) return;

        // 敌机下落
        m_selfTrans.position += new Vector3(0, -moveSpeed * Time.deltaTime, 0);

        // 超过屏幕下面
        if (Camera.main.WorldToScreenPoint(m_selfTrans.position).y <= -50)
        {
            BackToPool();
        }

        if (m_timeToFire > 0)
        {
            m_timeToFire -= Time.deltaTime;
            if (m_timeToFire <= 0)
            {
                EnemyBulletGenerator.GenerateBulletByAircraftType(aircraftType, m_selfTrans.position);
            }
        }
    }

    /// <summary>
    /// 碰撞处理
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if ("PlayerBullet" == other.tag)
        {
            --blood;
            if (blood > 0)
            {
                // 受击
                UnderAttack();
            }
            else
            {
                // 爆炸
                Explode();
            }
        }
    }

    private void LateUpdate()
    {
        if (m_underAttack)
        {
            m_underAttack = false;
            ani.SetBool("underattack", m_underAttack);
        }
    }

    public void ResetTimeToFire(float t)
    {
        m_timeToFire = t;
    }

    /// <summary>
    /// 对象回收
    /// </summary>
    private void BackToPool()
    {
        ActiveSelf(false);
        if (null != backToPoolAction)
            backToPoolAction();
    }

    /// <summary>
    /// 激活
    /// </summary>
    public void ActiveSelf(bool active)
    {
        m_collider.enabled = active;
        m_selfGo.SetActive(active);
    }

    /// <summary>
    /// 受击
    /// </summary>
    public override void UnderAttack()
    {
        m_underAttack = true;
        ani.SetBool("underattack", m_underAttack);
    }

    /// <summary>
    /// 爆炸
    /// </summary>
    public override void Explode()
    {
        // 爆炸时，把碰撞体禁用
        m_collider.enabled = false;

        ani.SetBool("explode", true);

        // 加分
        GameMgr.instance.Score += (int)aircraftType * 1000;
    }
}
