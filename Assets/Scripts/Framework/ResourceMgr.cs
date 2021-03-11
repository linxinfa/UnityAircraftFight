using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源管理器
/// </summary>
public class ResourceMgr
{
    /// <summary>
    /// 资源缓存
    /// </summary>
    Dictionary<string, Object> m_res = new Dictionary<string, Object>();

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="resPath">资源路径</param>
    /// <returns></returns>
    public T LoadRes<T>(string resPath) where T : Object
    {
        if(m_res.ContainsKey(resPath))
        {
            return m_res[resPath] as T;
        }
        T t = Resources.Load<T>(resPath);
        m_res[resPath] = t;
        return t;
    }

    // 单例模式
    private static ResourceMgr s_instance;
    public static ResourceMgr instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new ResourceMgr();
            return s_instance;
        }
    }
}
