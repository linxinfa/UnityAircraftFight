using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏结束界面
/// </summary>
public class GameOverPanel : BasePanel
{
    public override void Awake()
    {
        base.Awake();
        m_panelResPath = "Panels/GameOverPanel";
    }

    public override void SetUi(PrefabSlot slot)
    {
        base.SetUi(slot);
        slot.SetText("ScoreText", GameMgr.instance.Score.ToString());
        slot.SetButton("RestartBtn", (btn) =>
        {
            // 继续游戏
            Hide();
            GameMgr.instance.RestartGame();
        });
    }
}
