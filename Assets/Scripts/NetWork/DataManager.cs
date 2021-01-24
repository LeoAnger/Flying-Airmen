using System;
using System.Threading;
using NetWork.Entity;
using Newtonsoft.Json;
using Script.Enum;
using Script.NetWork;
using UnityEngine;

namespace NetWork
{
    public class DataManager : MonoBehaviour
    {
        public static string sourceDatas = "";
        public static string sourceDatasTemp = "";    //缓冲数据
        public static bool isReadedData = true;
        private SourceDataEntity deserializeObject;
        
        private Thread childThread;

        // Start is called before the first frame update
        void Start()
        {
            // 开启接收服务器消息的sub线程
            ThreadStart childref = new ThreadStart(DataManage);
            childThread = new Thread(childref);
            childThread.Start();
        }

        public void DataManage()
        {
            print("DataManager线程...");
            while (true)
            {
                controll();
            }
            
        }

        void controll()
        {
            while (!isReadedData)
            {
                //读取数据
                sourceDatas = sourceDatasTemp;
                isReadedData = true;
                //是否叠包
                string[] strArr = sourceDatas.Split('☍');
                for (int i = 0; i < strArr.Length - 1; i++)
                {
                    try
                    {
                        deserializeObject = JsonConvert.DeserializeObject<SourceDataEntity>(strArr[i]);
                    }
                    catch (Exception e)
                    {
                        print("映射数据：" + strArr[i]);
                        print(e);
                        break;
                    }
                    
                    /*
                     * 1.判断数据类型
                     * 2.获取Content进行反序列化
                     * 3.逻辑处理
                     */
                    if (RoomInfo.RoomInfo.RoomPermissions != RoomPermissions.Admin)
                    {
                        switch (deserializeObject.SourceDataType)
                        {
                            
                            case SourceDataType.EnemyBulletByName:
                                EnemyManager.EnemyBulletQueue.Enqueue(deserializeObject.Content);
                                break;
                            case SourceDataType.EnemyByName:
                                EnemyManager.EnemyQueue.Enqueue(deserializeObject.Content);
                                break;
                            case SourceDataType.GameObj:
                                print("接收到服务器创建物体消息...");
                                while (NetGameObj.isReadedData)
                                {
                                    NetGameObj.sourceDatasTemp = deserializeObject.Content;
                                    NetGameObj.isReadedData = false;
                                }
                                break;
                            case SourceDataType.Player1:
                                print("Player1消息来了。。。。。");
                                break;
                        }
                    }
                    switch (deserializeObject.SourceDataType)
                    {
                        
                        case SourceDataType.Player2:
                            print("接收到Player2消息。。。");
                            //判断是否是自己
                            //2.通知Player2
                            
                            // 通知DataManager
                            while (NetPlayer2.isReadedData)
                            {
                                NetPlayer2.sourceDatasTemp = deserializeObject.Content;
                                NetPlayer2.isReadedData = false;
                            }break;
                    }
                }
                
            }  
        }

        void controllCopy()
        {
            
        }
    
    }
}
