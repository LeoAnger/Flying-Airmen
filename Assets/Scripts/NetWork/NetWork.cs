using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace NetWork
{
    public class NetWork : MonoBehaviour
    {
        private TcpClient client;
        private static NetworkStream io;
        private Thread childThread;

        public static string SendDatas = "";
        public static string SendDatasTemp = "";
        public static bool HasNewSendDatas = false;
        public static Queue<string> EnemyQueue = new Queue<string>();
        public static Queue<string> EnemyBulletQueue = new Queue<string>();

        /*
     * 1.连接网络
     * 2.接受网络数据
     * 3.处理数据
     */
        void Awake()
        {
            NetWorkConn();
        }
    
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate=30;  
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            Send();
        }

        void NetWorkConn()
        {
            //1.获取IP
//        string ip = MenuBtnEvent.IP;
//        int port = MenuBtnEvent.PORT;
            //相当于java的 Socket so=new Socket("127.0.0.1",61666);
            client = new TcpClient("127.0.0.1",61666); 
            io = client.GetStream();
            // 开启接收服务器消息的sub线程
            ThreadStart childref = new ThreadStart(Receive);
            childThread = new Thread(childref);
            childThread.Start();
        }
    
        /// <summary>
        /// 客户端接收服务器发送过来的消息
        /// </summary>
        public void Receive()
        {
            byte[] buffer = new byte[1024];
            int buflen = -1;
            while (true)
            {
                try
                {
                    buflen = io.Read(buffer, 0, 1024);
                    string message = Encoding.UTF8.GetString(buffer, 0, buflen);
                    //print("Server: " + buffer.Length + " --> " + message);
                    //print(message);
                
                    // 通知DataManager
                    while (DataManager.isReadedData)
                    {
                        DataManager.sourceDatasTemp = message;
                        DataManager.isReadedData = false;
                    }

                }
                catch
                {
                    print("Server: close");
                }
            }
        }

        private byte[] by1 = new byte[1024];
        private byte[] by2 = new byte[1024];
        void Send()
        {
            // 检测数据通知
            if (HasNewSendDatas)
            {
                SendDatas = SendDatasTemp + "☍";
                HasNewSendDatas = false;
                //发送消息
                by1 = Encoding.UTF8.GetBytes(SendDatas);
                io.Write(by1, 0, by1.Length);
                io.Flush();
            }

            if (EnemyQueue.Count > 0)
            {
                //发送消息
                by1 = Encoding.UTF8.GetBytes(EnemyQueue.Dequeue() + "☍");
                io.Write(by1, 0, by1.Length);
                io.Flush();
            }
            
            if (EnemyBulletQueue.Count > 0)
            {
                //发送消息
                by2 = Encoding.UTF8.GetBytes(EnemyBulletQueue.Dequeue() + "☍");
                io.Write(by2, 0, by2.Length);
                io.Flush();
            }
        }
    }
}
