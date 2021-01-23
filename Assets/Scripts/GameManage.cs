using System;
using System.Collections;
using System.Threading;
using NetWork.Entity;
using NetWork.RoomInfo;
using NetWork.UserInfo;
using Newtonsoft.Json;
using Script.Enum;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManage : MonoBehaviour
{
    private Thread childThread;
    private int enemyInt = 10000;
    public static int EnemyBulletInt = 1000;
    
    
    
//    public GameObject[] enemys;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        //enemys = Resources.LoadAll<GameObject>("Enemys");

        if (RoomInfo.RoomPermissions == RoomPermissions.Admin)
        {
            InvokeRepeating(nameof(CreateEnemys), 0, 2);
        }  
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateEnemys()
    {

        GameObjEntity.PrefabName = "Enemy1";
        GameObject enemy = Instantiate(Resources.Load<GameObject>(
                Prefab.Prefab.Maps[GameObjEntity.PrefabName]),
            new Vector3(10, Random.Range(-4f, 4f), 1),
            Quaternion.identity);

        print("创建物体前的Int:" + enemyInt);
        string name1 = "Enemy" + Random.Range(-4f, 400f);
        enemy.name = name1;
        enemy.AddComponent<EnemyTest>();
        int speed = Random.Range(1, 3);
        enemy.GetComponent<EnemyTest>().speed = speed;
        print("speed:" + speed);

        enemy.tag = "Enemy";
        enemy.SetActive(true);
        
        // 发送创建物体消息。。
        SendCreateGameObj(enemy);

        GameObject _find = GameObject.Find(enemy.name);
        if (_find)
        {
            EnemyManager.EnemyNameList.Add(enemy.name);
        }
    }

    private SourceDataEntity SourceDataEntity = new SourceDataEntity();
    private GameObjEntity GameObjEntity = new GameObjEntity();
    void SendCreateGameObj(GameObject g)
    {
        GameObjEntity.CreateOrDestroy = true;    // 创建
        GameObjEntity.ObjName = g.name;
        
        GameObjEntity.PositionX = g.transform.position.x;
        GameObjEntity.PositionY = g.transform.position.y;
        GameObjEntity.PositionZ = g.transform.position.z;
        GameObjEntity.LocalScaleX = 1;
        GameObjEntity.LocalScaleY = 1;
        GameObjEntity.LocalScaleZ = 1;
        
        string s1 = JsonConvert.SerializeObject(GameObjEntity);
        SourceDataEntity.Content = s1;
        SourceDataEntity.SourceDataType = SourceDataType.GameObj;
        string s2 = JsonConvert.SerializeObject(SourceDataEntity);

        while (!NetWork.NetWork.HasNewSendDatas)
        {
            // 通知发送
            NetWork.NetWork.SendDatasTemp = s2;
            NetWork.NetWork.HasNewSendDatas = true;
            print("通知发送-"+ s2);
            break;
        }  
        /*
         * 0:{"SourceDataType":0,
         *     "Content":"{\"CreateOrDestroy\":true,\"ObjName\":\"Enemy10003\",
         *     \"PrefabName\":\"Enemy1\",\"PositionX\":10.0,\"PositionY\":-2.63091278,
         *     \"PositionZ\":1.0,\"LocalScaleX\":1.0,\"LocalScaleY\":1.0,\"LocalScaleZ\":1.0}","DataSerialNumber":0}
         */
    }
}