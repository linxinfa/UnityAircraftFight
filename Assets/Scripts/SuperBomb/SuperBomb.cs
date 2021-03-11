using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 核弹
/// </summary>
public class SuperBomb : MonoBehaviour
{
    public float dropSpeed = 5f;

    Transform m_selfTrans;

    private void Awake()
    {
        m_selfTrans = transform;
    }


    void Update()
    {
        if (GameState.Pause == GameMgr.instance.gameState) return;

        m_selfTrans.position -= new Vector3(0, dropSpeed * Time.deltaTime, 0);

        // 超过屏幕下面
        if (Camera.main.WorldToScreenPoint(m_selfTrans.position).y <= -100)
        {
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
