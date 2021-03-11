using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class ConfigMgr
{
    /// <summary>
    /// 加载本地游戏配置。
    /// </summary>
    public void Load()
    {
        var asset = Resources.Load("Config/GameConfig") as TextAsset;
        if (asset == null)
        {
            Debug.LogWarning("gameconfig not exist.");
            return;
        }
        gameConfig = DeserializeObject<GameConfig>(asset.text);
    }

    /// <summary>
    /// 获取指定分数所对应的关卡配置。
    /// </summary>
    /// <param name="score">分数</param>
    /// <returns>关卡配置</returns>
    public LevelConfig GetLevelByScore(int score)
    {
        var levels = gameConfig.Levels;
        for (int i = 0, len = levels.Length; i < len; ++i)
        {
            var levelCfg = levels[i];
            if (levelCfg.Score <= score)
                return levelCfg;
        }
        return levels[levels.Length - 1];
    }

    /// <summary>
    /// 将Xml反序列化为指定类型的对象。
    /// </summary>
    /// <typeparam name="T">需要反序列化的类型</typeparam>
    /// <param name="xml">Xml文本</param>
    /// <returns>反序列化后的对象</returns>
    public static T DeserializeObject<T>(string xml)
    {
        if (xml == null)
            throw new ArgumentNullException("xml");
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
        {
            var serializer = new XmlSerializer(typeof(T));
            var obj = (T)serializer.Deserialize(stream);
            return obj;
        }
    }

    /// <summary>
    /// 将指定对象序列化为Xml。
    /// </summary>
    /// <typeparam name="T">序列化对象类型</typeparam>
    /// <param name="obj">序列化对象</param>
    /// <returns>序列化后的Xml结果</returns>
    public static string SerializeObject<T>(T obj)
    {
        if (Equals(obj, default(T)))
            return string.Empty;
        using (var stream = new MemoryStream())
        {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(stream);
            var xml = reader.ReadToEnd();
            return xml;
        }
    }

    /// <summary>
    /// 获取当前游戏配置。
    /// </summary>
    public GameConfig gameConfig { get; private set; }

    // 单例模式
    private static ConfigMgr s_instance;
    public static ConfigMgr instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new ConfigMgr();
            return s_instance;
        }
    }
}

/// <summary>
/// 游戏配置。
/// </summary>
public class GameConfig
{
    /// <summary>
    /// 获取或设置关卡设置。
    /// </summary>
    [XmlArray("Levels")]
    [XmlArrayItem("Level")]
    public LevelConfig[] Levels { get; set; }

    /// <summary>
    /// 获取指定编号的关卡配置。
    /// </summary>
    /// <param name="levelID">关卡编号</param>
    /// <returns>关卡配置</returns>
    public LevelConfig GetLevelConfig(int levelID)
    {
        for (int i = 0; i < Levels.Length; ++i)
        {
            if (levelID == Levels[i].ID)
            {
                return Levels[i];
            }
        }
        return Levels[Levels.Length - 1];
    }
}

/// <summary>
/// 关卡配置。
/// </summary>
public class LevelConfig
{
    /// <summary>
    /// 获取或设置该关卡所有敌机生成概率权重。
    /// </summary>
    [XmlArray("Enemies")]
    [XmlArrayItem("Enemy")]
    public EnemyConfig[] Enemies { get; set; }

    /// <summary>
    /// 获取或设置敌机生成的时间间隔。
    /// </summary>
    [XmlAttribute("EnemySpawnTime")]
    public float EnemySpawnTime { get; set; }

    /// <summary>
    /// 获取或设置关卡编号。
    /// </summary>
    [XmlAttribute("ID")]
    public int ID { get; set; }

    /// <summary>
    /// 获取或设置进入关卡所达到的最低分数。
    /// </summary>
    [XmlAttribute("Score")]
    public int Score { get; set; }
}

/// <summary>
/// 敌机生成设置。
/// </summary>
public class EnemyConfig
{
    /// <summary>
    /// 获取或设置敌机预设的索引值。
    /// </summary>
    [XmlAttribute("Index")]
    public int Index { get; set; }

    /// <summary>
    /// 获取或设置敌机最高速度。
    /// </summary>
    [XmlAttribute("MaxSpeed")]
    public float MaxSpeed { get; set; }

    /// <summary>
    /// 获取或设置敌机最低速度。
    /// </summary>
    [XmlAttribute("MinSpeed")]
    public float MinSpeed { get; set; }

    /// <summary>
    /// 获取或设置敌机血量。
    /// </summary>
    [XmlAttribute("Blood")]
    public int Blood { get; set; }

    /// <summary>
    /// 获取或设置随机到该配置的概率权重。
    /// </summary>
    [XmlAttribute("Weight")]
    public int Weight { get; set; }
}