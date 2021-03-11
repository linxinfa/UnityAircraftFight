using System;
using System.Collections.Generic;


/// <summary>
/// 加权随机生成器。
/// </summary>
public class WeightedRandom<T>
{
    /// <summary>
    /// 构造一个加权随机生成器。
    /// </summary>
    /// <param name="capcity">初始容量</param>
    public WeightedRandom(int capcity)
    {
        if (capcity < 0)
            throw new ArgumentOutOfRangeException("capcity");

        m_items = new Dictionary<T, int>(capcity);
    }

    /// <summary>
    /// 构造一个加权随机生成器。
    /// </summary>
    public WeightedRandom()
    {
        m_items = new Dictionary<T, int>();
    }

    /// <summary>
    /// 增加一个加权随机项。
    /// </summary>
    /// <param name="item">加权随机项</param>
    /// <param name="weight">权重</param>
    public void Add(T item, int weight)
    {
        if (weight <= 0)
            return;

        m_totalWeight += weight;
        if (m_items.ContainsKey(item))
            m_items[item] += weight;
        else
            m_items.Add(item, weight);
    }

    /// <summary>
    /// 清除所有的加权随机项。
    /// </summary>
    public void Clear()
    {
        m_totalWeight = 0;
        m_items.Clear();
    }

    /// <summary>
    /// 获取下一个加权随机项。
    /// </summary>
    /// <returns>加权随机项</returns>
    public T Next()
    {
        if (m_totalWeight == 0)
            throw new InvalidOperationException("Items is empty.");

        var weight = m_random.Next(1, m_totalWeight + 1);
        var counter = 0;
        foreach (var item in m_items)
        {
            counter += item.Value;
            if (counter >= weight)
                return item.Key;
        }
        throw new InvalidOperationException();
    }

    /// <summary>
    /// 获取指定个数的加权随机项。
    /// </summary>
    /// <param name="count">个数</param>
    /// <returns>加权随机项集合</returns>
    public IEnumerable<T> Next(int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException("count");

        for (var i = 0; i < count; i++)
            yield return Next();
    }

    /// <summary>
    /// 移除一个加权随机项。
    /// </summary>
    /// <param name="item">加权随机项</param>
    public void Remove(T item)
    {
        if (!m_items.ContainsKey(item))
            return;

        m_totalWeight -= m_items[item];
        m_items.Remove(item);
    }

    private readonly Dictionary<T, int> m_items;
    private readonly Random m_random = new Random();
    private int m_totalWeight;
}

