using UnityEngine;

public class AircraftFactory 
{
    /// <summary>
    /// 创建飞机
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static BaseAircraft CreateAircraft(AircraftType t)
    {
        if(null == s_aircraftFactoryRoot)
        {
            var rootObj = new GameObject("AircraftFactoryRoot");
            s_aircraftFactoryRoot = rootObj.transform;
        }
        BaseAircraft aircraft = null;
        switch (t)
        {
            case AircraftType.Player:
                {
                    var prefab = ResourceMgr.instance.LoadRes<GameObject>("Player/Player");
                    var obj = Object.Instantiate(prefab);
                    aircraft = obj.AddComponent<PlayerAircraft>();
                    break;
                }
            case AircraftType.Enemy1:
            case AircraftType.Enemy2:
            case AircraftType.Enemy3:
                {
                    var prefab = ResourceMgr.instance.LoadRes<GameObject>("Enemy/Enemy" + (int)t);
                    var obj = Object.Instantiate(prefab);
                    aircraft = obj.AddComponent<EnemyAircraft>();
                    break;
                }
        }
        aircraft.transform.SetParent(s_aircraftFactoryRoot, false);
        aircraft.aircraftType = t;
        return aircraft;
    }

    public static void DestroyFactoryRoot()
    {
        if(null != s_aircraftFactoryRoot)
        {
            Object.Destroy(s_aircraftFactoryRoot.gameObject);
            s_aircraftFactoryRoot = null;
        }
    }

    private static Transform s_aircraftFactoryRoot;
}

/// <summary>
/// 飞机类型
/// </summary>
public enum AircraftType
{
    Player,
    Enemy1,
    Enemy2,
    Enemy3,
}
