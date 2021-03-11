using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 界面管理器
/// </summary>
public class PanelMgr
{

    private void Init()
    {
        m_canvasTransform = GameObject.Find("Canvas").transform;
        m_isInit = true;
    }

    /// <summary>
    /// 显示界面
    /// </summary>
    /// <typeparam name="T">界面类</typeparam>
    public T ShowPanel<T>() where T : BasePanel
    {
        if (!m_isInit) Init();

        BasePanel panel = null;
        var panelName = typeof(T).ToString();
        if (m_panels.ContainsKey(panelName))
        {
            panel = m_panels[panelName];
        }
        else
        {
            var panelObj = new GameObject(panelName);
            panelObj.transform.SetParent(m_canvasTransform, false);
            // 铺满全屏
            var rect = panelObj.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);



            panel = panelObj.AddComponent<T>();
            m_panels[panelName] = panel;
        }
        panel.enabled = true;
        panel.Show();
        return panel as T;
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <typeparam name="T">界面类</typeparam>
    public void HidePanel<T>() where T : BasePanel
    {
        BasePanel panel = null;
        var panelName = typeof(T).ToString();
        if (m_panels.ContainsKey(panelName))
        {
            panel = m_panels[panelName];
        }
        if (null != panel)
            panel.Hide();
    }

    private Dictionary<string, BasePanel> m_panels = new Dictionary<string, BasePanel>();
    private bool m_isInit = false;
    private Transform m_canvasTransform;

    // 单例模式
    private static PanelMgr s_instance;
    public static PanelMgr instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new PanelMgr();
            return s_instance;
        }
    }
}

/// <summary>
/// 界面基类
/// </summary>
public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 界面预设资源路径
    /// </summary>
    protected string m_panelResPath;

    /// <summary>
    /// 界面GaemObject
    /// </summary>
    protected GameObject m_panelObj;
    /// <summary>
    /// 界面父节点
    /// </summary>
    protected Transform m_panelParent;

    public virtual void Awake()
    {
        m_panelParent = transform;
    }

    /// <summary>
    /// 显示界面
    /// </summary>
    public virtual void Show()
    {
        if (null == m_panelObj)
        {
            var prefab = ResourceMgr.instance.LoadRes<GameObject>(m_panelResPath);
            m_panelObj = Instantiate(prefab);
            m_panelObj.transform.SetParent(m_panelParent, false);
        }
        var slot = m_panelObj.GetComponent<PrefabSlot>();
        SetUi(slot);
        OnShow();
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public virtual void Hide()
    {
        if (null != m_panelObj)
        {
            Object.Destroy(m_panelObj);
            this.enabled = false;
        }
        OnHide();
    }

    protected virtual void OnShow()
    {

    }

    protected virtual void OnHide()
    {

    }

    /// <summary>
    /// 设置界面ui逻辑
    /// </summary>
    public virtual void SetUi(PrefabSlot slot) { }
}