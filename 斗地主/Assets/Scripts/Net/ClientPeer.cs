using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;

/// <summary>
/// 客户端socket
/// </summary>
public class ClientPeer
{
    private Socket socket;

    private string ip;
    private int port;

    /// <summary>
    /// 构造连接对象
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <param name="port">端口号</param>
    public ClientPeer(string ip, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ip = ip;
            this.port = port;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void Connect()
    {
        try
        {
            socket.Connect(ip, port);
            Debug.Log("连接服务器成功！");

            StartReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }


    #region 接受数据

    //接受的数据缓冲区
    private byte[] receiveBuffer = new byte[1024];

    /// <summary>
    /// 一旦接收到数据 就存到缓存区里面
    /// </summary>
    private List<byte> dataCache = new List<byte>();

    private bool isProcessReceive = false;

    public Queue<SocketMsg> socketMsgQueue = new Queue<SocketMsg>();

    /// <summary>
    /// 开始异步接受数据
    /// </summary>
    private void StartReceive()
    {
        if (socket == null && socket.Connected == false)
        {
            Debug.LogError("没有连接成功，无法发送数据");
            return;
        }

        socket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, socket);
    }

    /// <summary>
    /// 收到消息的回调
    /// </summary>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            int length = socket.EndReceive(ar);
            byte[] tmpByteArray = new byte[length];
            Buffer.BlockCopy(receiveBuffer, 0, tmpByteArray, 0, length);

            //处理收到的数据
            dataCache.AddRange(tmpByteArray);
            if (isProcessReceive == false)
                ProcessReceive();

            StartReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
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

        //尾递归
        ProcessReceive();
    }

    #endregion

    #region 发送数据
    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="msg"></param>
    public void Send(SocketMsg msg)
    {
        //if (!socket.Connected)
        //{
        //    Connect();
        //}
        byte[] data = EncodeHelper.EncodeMsg(msg);
        byte[] packet = EncodeHelper.EncodePacket(data);

        try
        {
            socket.Send(packet);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    #endregion
}

