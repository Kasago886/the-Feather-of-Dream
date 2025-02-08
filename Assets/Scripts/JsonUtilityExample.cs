using UnityEngine;
using Newtonsoft.Json;
using System;

[Serializable]
public class NestedClass
{
    public int value;
    public NestedClass nested; // 类可以包含自身类型
}

[Serializable]
public class DataContainer
{
    public NestedClass root;
}

public class JsonUtilityExample : MonoBehaviour
{
    void Start()
    {
        // 创建一个包含嵌套结构的数据
        var nested = new NestedClass { value = 2 };
        var root = new NestedClass { value = 1, nested = nested };
        var container = new DataContainer { root = root };

        // 使用Newtonsoft.Json序列化为JSON字符串
        string jsonString = JsonConvert.SerializeObject(container);
        Debug.Log("Serialized JSON: " + jsonString);

        // 使用Newtonsoft.Json反序列化回对象
        DataContainer deserializedContainer = JsonConvert.DeserializeObject<DataContainer>(jsonString);
        Debug.Log("Deserialized value: " + deserializedContainer.root.value);
        if (deserializedContainer.root.nested != null)
        {
            Debug.Log("Nested value: " + deserializedContainer.root.nested.value);
        }
    }
}