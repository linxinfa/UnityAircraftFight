using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主角飞机
/// </summary>
public class PlayerAircraft : BaseAircraft
{
    private bool m_isPress = false;

    private const float SCREEN_INNER_OFFSET = 20;

    // 子弹生成器
    private PlayerBulletGenerator m_bulletGenerator = new PlayerBulletGenerator();

    protected override void Awake()
    {
        base.Awake();
        // 监听动画帧事件
        m_aniEvent.aniEventCb = (msg) =>
        {
            // 爆炸动画播放结束
            if ("explode_finish" == msg)
            {
                DestroySelf();
                // 游戏结束
                GameMgr.instance.GameOver();
            }
        };

        m_bulletGenerator.Init(m_selfTrans);
    }

    /// <summary>
    /// 鼠标按中主角飞机
    /// </summary>
    protected void OnMouseDown()
    {
        m_isPress = true;
    }

    /// <summary>
    /// 鼠标抬起
    /// </summary>
    protected void OnMouseUp()
    {
        m_isPress = false;
    }

    protected void Update()
    {
        if (GameState.Pause == GameMgr.instance.gameState) return;

        if (Input.GetMouseButton(0) && m_isPress)
        {
            // 根据鼠标位置设置飞机坐标
            var mousePos = Input.mousePosition;
            // 限制坐标在屏幕内
            if (mousePos.x < SCREEN_INNER_OFFSET)
            {
                mousePos.x = SCREEN_INNER_OFFSET;
            }
            else if(mousePos.x > Screen.width - SCREEN_INNER_OFFSET)
            {
                mousePos.x = Screen.width - SCREEN_INNER_OFFSET;
            }
            if (mousePos.y < SCREEN_INNER_OFFSET)
            {
                mousePos.y = SCREEN_INNER_OFFSET;
            }
            else if (mousePos.y > Screen.height- SCREEN_INNER_OFFSET)
            {
                mousePos.y = Screen.height - SCREEN_INNER_OFFSET;
            }

            var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5));
            m_selfTrans.position = worldPos;
        }

        // 子弹生成器
        m_bulletGenerator.Update();
    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="other"></param>
    public override void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                {
                    // 爆炸
                    Explode();
                }
                break;
            case "SuperBomb":
                {
                    Destroy(other.gameObject);
                    ++GameMgr.instance.BombCnt;
                }
                break;
        }
    }

    /// <summary>
    /// 爆炸
    /// </summary>
    public override void Explode()
    {
        ani.SetBool("explode", true);
    }

    public override void DestroySelf()
    {
        Destroy(m_selfGo);
        m_bulletGenerator.ClearBullets();
    }
}
