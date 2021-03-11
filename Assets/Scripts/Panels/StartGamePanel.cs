
/// <summary>
/// 开始游戏界面
/// </summary>
public class StartGamePanel : BasePanel
{
    public override void Awake()
    {
        base.Awake();
        m_panelResPath = "Panels/StartGamePanel";
    }

    public override void SetUi(PrefabSlot slot)
    {
        base.SetUi(slot);
        slot.SetButton("StartGameBtn", (btn) => 
        {
            // 开始游戏
            GameMgr.instance.StartGame();
        });
    }
}
