using UnityEngine;
using System;

/// <summary>
/// 主角子弹
/// </summary>
public class PlayerBullet : MonoBehaviour
{
    /// <summary>
    /// 威力
    /// </summary>
    public int power = 1;
    /// <summary>
    /// 飞行速度
    /// </summary>
    public float speed = 1f;

    private Transform m_selfTrans;
    private GameObject m_selfGo;

    /// <summary>
    /// 回收委托
    /// </summary>
    public Action backToPoolAction;

    void Awake()
    {
        m_selfGo = gameObject;
        m_selfTrans = transform;
    }

    void Update()
    {
        if (GameState.Pause == GameMgr.instance.gameState) return;

        // 子弹向上飞行
        m_selfTrans.position += Vector3.up * speed * Time.deltaTime;

        // 超过屏幕上面
        if (Camera.main.WorldToScreenPoint(m_selfTrans.position).y >= Screen.height + 50)
        {
            BackToPool();
        }
    }

    /// <summary>
    /// 激活
    /// </summary>
    public void ActiveSelf(bool active)
    {
        m_selfGo.SetActive(active);
    }

    /// <summary>
    /// 设置坐标
    /// </summary>
    /// <param name="pos"></param>
    public void SetStartPos(Vector3 pos)
    {
        m_selfTrans.position = pos;
    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ("Enemy" == collision.tag)
        {
            BackToPool();
        }
    }


    /// <summary>
    /// 回收
    /// </summary>
    private void BackToPool()
    {
        ActiveSelf(false);
        if (null != backToPoolAction)
        {
            backToPoolAction();
        }
    }
}
