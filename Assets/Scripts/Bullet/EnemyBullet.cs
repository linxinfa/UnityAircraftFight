using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 1f;
    /// <summary>
    /// 自我旋转
    /// </summary>
    public bool rotateSelf = false;

    /// <summary>
    /// 回收委托
    /// </summary>
    public Action backToPoolAction;

    private Transform m_selfTrans;
    private GameObject m_selfGo;
    private Vector3 m_targetDir;

    private void Awake()
    {
        m_selfGo = gameObject;
        m_selfTrans = transform;
    }


    public void SetStartPos(Vector3 pos)
    {
        m_selfTrans.position = pos;
    }

    public void SetTargetDir(Vector3 dir)
    {
        m_targetDir = dir.normalized;
    }

    public void SetAngles(Vector3 rot)
    {
        m_selfTrans.eulerAngles = rot;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if("Player" == collision.tag)
        {
            BackToPool();
        }
    }

    private void Update()
    {
        if (GameState.Pause == GameMgr.instance.gameState) return;

        // 子弹向上飞行
        m_selfTrans.position += m_targetDir * speed * Time.deltaTime;

        // 超过屏幕外面
        var screenPos = Camera.main.WorldToScreenPoint(m_selfTrans.position);
        if (screenPos.y <= -50 || screenPos.x < -50 || screenPos.x > Screen.width + 50)
        {
            BackToPool();
        }

        if(rotateSelf)
        {
            m_selfTrans.Rotate(0, 0, 30);
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

    /// <summary>
    /// 激活
    /// </summary>
    public void ActiveSelf(bool active)
    {
        m_selfGo.SetActive(active);
    }
}
