using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 入口脚本
/// </summary>
public class Main : MonoBehaviour
{
    /// <summary>
    /// 游戏主入口
    /// </summary>
    void Start()
    {
        GameMgr.instance.Main();
    }

    void Update()
    {
        GameMgr.instance.Update();
    }
}
