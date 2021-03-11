using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// 预设插槽
/// </summary>
public class PrefabSlot : MonoBehaviour
{

    [Serializable]

    public class Item
    {
        public string name;
        public UnityEngine.Object obj;

    }

    public Item[] items = new Item[0];

    public UnityEngine.Object GetObj(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        for (int i = 0, cnt = items.Length; i < cnt; i++)
        {
            Item item = items[i];

            if (item.name.Equals(name))
            {
                return item.obj;
            }
        }
        return null;
    }

    public T GetObj<T>(string name) where T : UnityEngine.Object
    {
        try
        {
            return (T)GetObj(name);
        }
        catch (Exception e)
        {
            Debug.LogError("PrefabSlot GetObj name = " + name);
            return default(T);
        }
    }

    #region 设置ui接口
    public Text SetText(string name, string textStr)
    {
        var text = GetObj<Text>(name);
        if (null != text)
        {
            text.text = textStr;
        }
        else
        {
            Debug.LogError("PrefabSlot SetText Error, obj is null: " + name);
        }

        return text;
    }

    public Button SetButton(string name, Action<GameObject> onClick)
    {
        var btn = GetObj<Button>(name);
        if (null != btn)
        {
            btn.onClick.AddListener(() =>
            {
                onClick(btn.gameObject);
            });
        }
        else
        {
            Debug.LogError("PrefabSlot SetButton Error, obj is null: " + name);
        }
        return btn;
    }

    ///TODO: 其他接口可自行拓展

    #endregion 设置ui接口
}
