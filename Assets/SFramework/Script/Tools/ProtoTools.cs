using UnityEngine;

public class ProtoTools
{
    /// <summary>
    /// 序列化操作
    /// </summary>
    /// <param name="proto"></param>
    /// <returns></returns>
    public static byte[] Decode(object proto)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        ProtoBuf.Serializer.Serialize(ms, proto);
        byte[] data = ms.ToArray();
        return data;
    }

    /// <summary>
    /// 反序列化操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T Encode<T>(byte[] data)
    {
        if (data == null)
        {
            throw new Exception("Protobuf Covert Error =============找程序。");
        }

        System.IO.MemoryStream ms1 = new System.IO.MemoryStream(data);
        T t = ProtoBuf.Serializer.Deserialize<T>(ms1);
        return t;
    }
}
