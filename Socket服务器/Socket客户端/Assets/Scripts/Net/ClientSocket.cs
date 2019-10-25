using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;

/// <summary>
/// 客户端socket
/// </summary>
public class ClientSocket
{
    private Socket socket;
    private string ip;
    private int port;
    /// <summary>
    /// 构造连接对象
    /// </summary>
    /// <param name="ip">ip地址</param>
    /// <param name="port">端口号</param>
    public ClientSocket(string ip, int port)
    {
        try
        {
            this.ip = ip;
            this.port = port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }
    /// <summary>
    /// 连接
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void Connected()
    {
        try
        {
            socket.Connect(ip, port);
            Debug.Log("连接服务器成功");
            //开始处理数据
            StartReceive();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }
    #region 接收数据
    //接受的数据缓冲区
    private byte[] receiveBuffer = new byte[1024];
    /// <summary>
    /// 接收到的数据缓存数组
    /// </summary>
    private List<byte> dataCache = new List<byte>();
    /// <summary>
    /// 是否处理读取的数据
    /// </summary>
    private bool isProcessReceive = false;
    /// <summary>
    /// 解析后的消息数组
    /// </summary>
    public Queue<SocketMsg> socketMsgQueue = new Queue<SocketMsg>();
    /// <summary>
    /// 开始接收数据
    /// </summary>
    private void StartReceive()
    {
        if (socket == null && socket.Connected==false)
        {
            Debug.LogError("连接失败，无法发送数据");
            return;
        }
        socket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, socket);
    }
    /// <summary>
    /// 收到消息回调
    /// </summary>
    /// <param name="result"></param>
    private void ReceiveCallBack(IAsyncResult result)
    {
        try
        {
            //接收到的数据长度
            int length = socket.EndReceive(result);
            //临时数据流
            byte[] tmpByteArrat = new byte[length];
            //把接收到的数据拷贝到临时数据流中
            Buffer.BlockCopy(receiveBuffer, 0, tmpByteArrat, 0, length);
            //
            dataCache.AddRange(tmpByteArrat);
            if (isProcessReceive == false)
            {
                ProcessReceive();
            }
            StartReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }
    /// <summary>
    /// 处理收到的数据
    /// </summary>
    private void ProcessReceive()
    {
        isProcessReceive = true;
        //解析数据包
        byte[] data = EncodeHelper.DecodePacket(ref dataCache);
        if (data == null)
        {
            isProcessReceive = false;
            return;
        }
        SocketMsg msg = EncodeHelper.DecodeMsg(data);
        //存储消息 等待处理
        socketMsgQueue.Enqueue(msg);
        //伪递归
        ProcessReceive();
    }
    #endregion
    #region 发送数据
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    public void Send(SocketMsg msg)
    {
        byte[] data = EncodeHelper.EncodeMsg(msg);
        byte[] packet = EncodeHelper.EncodePacket(data);
        try
        {
            socket.Send(packet);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }
    #endregion
}
