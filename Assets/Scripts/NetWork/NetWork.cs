using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Script.Enum;
using Script.NetWork.Entity;
using UnityEngine;

namespace Script.NetWork
{
    public class NetWork : MonoBehaviour
    {
        private TcpClient client;
        private static NetworkStream io;
        private Thread childThread;

        public static string SendDatas = "";
        public static string SendDatasTemp = "";
        public static bool HasNewSendDatas = false;


        private SourceDataEntity _sourceDataEntity = new SourceDataEntity();

        private Player2Entity P2 = new Player2Entity();

        public GameObject Player2;

        public Animator Animator2;
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
                    print(message);
                
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
    
        void Send()
        {
            /*//封装消息
            _sourceDataEntity.SourceDataType = SourceDataType.Player2;
            P2.PositionX = Player2.transform.position.x;
            P2.PositionY = Player2.transform.position.y;
            P2.PositionZ = Player2.transform.position.z;
            P2.LocalScaleX = Player2.transform.localScale.x;
            P2.LocalScaleY = Player2.transform.localScale.y;
            P2.LocalScaleZ = Player2.transform.localScale.z;
            P2.running = Animator2.GetFloat("running");
            P2.idle = Animator2.GetBool("idle");
            P2.falling = Animator2.GetBool("falling");
            P2.jumping = Animator2.GetBool("jumping");
            String s1 = JsonConvert.SerializeObject(P2);
            _sourceDataEntity.Content = s1;
            String s2 = JsonConvert.SerializeObject(_sourceDataEntity);
        
            //发送消息
            byte[] by = Encoding.UTF8.GetBytes(s2);
            io.Write(by, 0, by.Length);
            io.Flush();*/
        
            // 检测数据通知
            if (HasNewSendDatas)
            {
                SendDatas = SendDatasTemp;
                HasNewSendDatas = false;
                //发送消息
                byte[] by1 = Encoding.UTF8.GetBytes(SendDatas);
                io.Write(by1, 0, by1.Length);
                io.Flush();
            }
        }
    }
}
