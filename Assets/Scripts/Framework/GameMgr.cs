using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 游戏管理器
/// </summary>
public class GameMgr
{
    /// <summary>
    /// 游戏主入口函数
    /// </summary>
    public void Main()
    {
        // 读取配置
        ConfigMgr.instance.Load();
 

        gameState = GameState.Ready;
        // 显示游戏开始界面
        PanelMgr.instance.ShowPanel<StartGamePanel>();
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        // 初始从第一关开始
        Level = ConfigMgr.instance.gameConfig.GetLevelConfig(1);
        // 初始化得分
        Score = 0;
        // 初始化核弹
        BombCnt = 0;


        // 关闭开始游戏界面
        PanelMgr.instance.HidePanel<StartGamePanel>();

        // 显示游戏战斗界面
        PanelMgr.instance.ShowPanel<MainGamePanel>();

        // 创建主角飞机
        player = AircraftFactory.CreateAircraft(AircraftType.Player);

        // 初始化核弹生成器
        m_superBombGenerator.Init();


        gameState = GameState.Playing;
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver()
    {
        gameState = GameState.End;
        PanelMgr.instance.ShowPanel<GameOverPanel>();
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        gameState = GameState.Pause;
        // 显示暂停界面
        PanelMgr.instance.ShowPanel<PausePanel>();
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ContinueGame()
    {
        gameState = GameState.Playing;
    }

    /// <summary>
    /// 清理飞机和子弹物体
    /// </summary>
    public void ClearObjs()
    {
        if (null != player)
            player.DestroySelf();
        // 清空所有飞机
        AircraftFactory.DestroyFactoryRoot();
        m_enemyGenerator.ClearAll();
        // 清空所有子弹
        EnemyBulletGenerator.CLear();
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void RestartGame()
    {
        ClearObjs();
        StartGame();
    }

    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void BackToHomePanel()
    {
        ClearObjs();

        // 关闭游戏战斗界面
        PanelMgr.instance.HidePanel<MainGamePanel>();
        // 显示开始游戏界面
        PanelMgr.instance.ShowPanel<StartGamePanel>();
    }

    public void Update()
    {
        if (GameState.Playing == gameState)
        {
            m_enemyGenerator.Update();
            m_superBombGenerator.Update();
        }
    }

    /// <summary>
    /// 全屏炸机
    /// </summary>
    public void KillAllEnemy()
    {
        if (BombCnt <= 0) return;

        --BombCnt;
        m_enemyGenerator.KillAllEnemy();
    }

    /// <summary>
    /// 获取主角飞机的坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerPos()
    {
        if(null != player && null != player.gameObject)
        {
            return player.transform.position;
        }
        return Vector3.zero;
    }

    private EnemyGenerator m_enemyGenerator = new EnemyGenerator();
    private SuperBombGenerator m_superBombGenerator = new SuperBombGenerator();

    public BaseAircraft player;

    /// <summary>
    /// 游戏状态
    /// </summary>
    public GameState gameState = GameState.Ready;

    /// <summary>
    /// 获取或设置当前关卡配置。
    /// </summary>
    public LevelConfig Level
    {
        get { return m_level; }
        set
        {
            m_level = value;
            m_nextLevel = ConfigMgr.instance.gameConfig.GetLevelConfig(m_level.ID + 1);
            m_enemyGenerator.UpdateRandomEnemys();
        }
    }

    /// <summary>
    /// 获取或设置游戏的当前分数。
    /// </summary>
    public int Score
    {
        get { return m_score; }
        set
        {
            m_score = value;
            if (m_nextLevel != null && m_nextLevel.Score <= value)
                Level = m_nextLevel;

            // 抛出分数更新时间
            EventDispatcher.instance.DispatchEvent(EventDef.EVENT_UPDATE_SCORE);
        }
    }

    /// <summary>
    /// 核弹数量
    /// </summary>
    public int BombCnt
    {
        get { return m_bombCnt; }
        set
        {
            m_bombCnt = value;
            EventDispatcher.instance.DispatchEvent(EventDef.EVENT_UPDATE_BOMB_CNT);
        }
    }

    private LevelConfig m_level;
    private LevelConfig m_nextLevel;
    private int m_score;
    private int m_bombCnt;

    // 单例模式
    private static GameMgr s_instance;
    public static GameMgr instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new GameMgr();
            return s_instance;
        }
    }
}

/// <summary>
/// 游戏状态
/// </summary>
public enum GameState
{
    Ready,
    Playing,
    Pause,
    End,
}