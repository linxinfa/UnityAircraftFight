using UnityEngine;

/// <summary>
/// 飞机基类
/// </summary>
public class BaseAircraft : MonoBehaviour
{
    public AircraftType aircraftType;
    public int blood;
    
    public Animator ani;
    public AnimationEvent m_aniEvent;
    protected Transform m_selfTrans;
    protected GameObject m_selfGo;
    protected Collider2D m_collider;


    protected virtual void Awake()
    {
        m_selfGo = gameObject;
        m_selfTrans = m_selfGo.transform;
        ani = GetComponent<Animator>();
        m_aniEvent = GetComponent<AnimationEvent>();
        m_collider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        // TODO 
    }


    /// <summary>
    /// 受击
    /// </summary>
    public virtual void UnderAttack()
    {

    }

    /// <summary>
    /// 爆炸
    /// </summary>
    public virtual void Explode()
    {
        
    }

    public virtual void DestroySelf()
    {

    }
}