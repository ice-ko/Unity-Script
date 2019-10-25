using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
/// 编码助手
/// </summary>
public class EncodeHelper
{
    #region 粘包拆包
    /// <summary>
    /// 构造数据包：包头+包尾
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] EncodePacket(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                //先写入长度
                bw.Write(data.Length);
                //在写入数据
                bw.Write(data);
                //
                byte[] byteArray = new byte[(int)ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, byteArray, 0, (int)ms.Length);
                return byteArray;
            }
        }
    }
    /// <summary>
    /// 解析数据包 
    /// </summary>
    /// <returns></returns>
    public static byte[] DecodePacket(ref List<byte> dataCache)
    {
        if (dataCache.Count < 4)
        {
            return null;
            //throw new Exception("数据缓存长度不足4,不能构成完整的数据包");
        }
        using (MemoryStream ms = new MemoryStream(dataCache.ToArray()))
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                int length = br.ReadInt32();
                int dataRemainLegth = (int)(ms.Length - ms.Position);
                if (length > dataRemainLegth)
                {
                    return null;
                    //throw new Exception("数据长度不够包头约定的长度,不能构成完整的信息");
                }
                byte[] data = br.ReadBytes(length);
                //更新数据缓存
                dataCache.Clear();
                dataCache.AddRange(br.ReadBytes(dataRemainLegth));
                return data;
            }
        }
    }
    #endregion
    #region 构造发送的socketMsg类
    /// <summary>
    /// 把socketMsg类转换成字节数组
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] EncodeMsg(SocketMsg msg)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(msg.OpCode);
                bw.Write(msg.SubCode);
                //
                if (msg.value != null)
                {
                    byte[] valueBytes = EncodeObj(msg.value);
                    bw.Write(valueBytes);
                }

                byte[] data = new byte[(int)ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, (int)ms.Length);
                return data;
            }
        }
    }
    /// <summary>
    /// 将收到的字节数据数组转换成socketMsg对象
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static SocketMsg DecodeMsg(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                SocketMsg msg = new SocketMsg();
                msg.OpCode = br.ReadInt32();
                msg.SubCode = br.ReadInt32();
                //
                if (ms.Length > ms.Position)
                {
                    byte[] valueBytes = br.ReadBytes((int)(ms.Length - ms.Position));
                    msg.value = DecodeObj(valueBytes);
                }
                return msg;
            }
        }
    }
    #endregion
    #region 把object类型转换成byte[] 
    /// <summary>
    /// 系列化对象
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] EncodeObj(object value)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, value);
            //从内存中拷贝数据
            byte[] valueBytes = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, valueBytes, 0, (int)ms.Length);
            return valueBytes;
        }
    }
    /// <summary>
    /// 反序列化对象
    /// </summary>
    /// <param name="valueBytes"></param>
    /// <returns></returns>
    public static object DecodeObj(byte[] valueBytes)
    {
        using (MemoryStream ms = new MemoryStream(valueBytes))
        {
            BinaryFormatter bf = new BinaryFormatter();

            return bf.Deserialize(ms);
        }
    }
    #endregion
}
