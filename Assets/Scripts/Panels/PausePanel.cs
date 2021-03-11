using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 暂停界面
/// </summary>
public class PausePanel : BasePanel
{
    public override void Awake()
    {
        base.Awake();
        m_panelResPath = "Panels/PausePanel";
    }

    public override void SetUi(PrefabSlot slot)
    {
        base.SetUi(slot);
        slot.SetButton("ContinueBtn", (btn) =>
        {
            // 继续游戏
            Hide();
            GameMgr.instance.ContinueGame();
        });
        slot.SetButton("RestartBtn", (btn) =>
        {
            // 重新开始游戏
            Hide();
            GameMgr.instance.RestartGame();
        });
        slot.SetButton("HomeBtn", (btn) =>
        {
            // 返回主菜单
            Hide();
            GameMgr.instance.BackToHomePanel();
        });
    }
}
