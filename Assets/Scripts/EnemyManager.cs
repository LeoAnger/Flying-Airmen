using System;
using System.Collections;
using System.Collections.Generic;
using NetWork.Entity;
using NetWork.RoomInfo;
using Newtonsoft.Json;
using Script.Enum;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 敌人数据
    public static Queue<string> EnemyQueue = new Queue<string>();
    
    // 敌人子弹数据
    public static Queue<string> EnemyBulletQueue = new Queue<string>();
    
    public static List<string> EnemyNameList = new List<string>();
    public static List<string> EnemyBulletNameList = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (RoomInfo.RoomPermissions == RoomPermissions.Admin)
        {
            EnemyHandler();
            EnemyBulletHandler();
        }
    }
    
    void FixedUpdate()
    {
        EnemyHand();
        EnemyBulletHand();
    }
    
    /// <summary>
    /// 敌人子弹发送数据同步
    /// </summary>
    void EnemyBulletHandler()
    {
        for (int i = 0; i < EnemyBulletNameList.Count; i++)
        {  
            EnemyBulletNet((string) EnemyBulletNameList[i]);
        }
    }
    
    /// <summary>
    /// 敌人发送数据同步
    /// </summary>
    void EnemyHandler()
    {
        //print("ForEach之前的集合数：" + EnemyNameList.Count);
        for (int i = 0; i < EnemyNameList.Count; i++)
        {  
            EnemyNet((string) EnemyNameList[i]);
        }
        
    }
    
    EnemyEntity _enemyEntity = new EnemyEntity();
    private SourceDataEntity SourceDataEntity1 = new SourceDataEntity();
    private string s1;
    private string s2;
    /// <summary>
    /// 网络同步-敌机位置
    /// </summary>
    void EnemyNet(string _name)
    {
        GameObject g = GameObject.Find(_name);
        if (!g) return;
        _enemyEntity.ObjName = g.name;
        _enemyEntity.PositionX = g.transform.position.x;
        _enemyEntity.PositionY = g.transform.position.y;
        _enemyEntity.PositionZ = g.transform.position.z;
        _enemyEntity.LocalScaleX = g.transform.localScale.x;
        _enemyEntity.LocalScaleY = g.transform.localScale.y;
        _enemyEntity.LocalScaleZ = g.transform.localScale.z;
        
        s1 = JsonConvert.SerializeObject(_enemyEntity);
        SourceDataEntity1.Content = s1;
        SourceDataEntity1.SourceDataType = SourceDataType.EnemyByName;
        s2 = JsonConvert.SerializeObject(SourceDataEntity1);

        NetWork.NetWork.EnemyQueue.Enqueue(s2);  
    }

    EnemyBulletEntity _bulletEntity = new EnemyBulletEntity();
    private string s3;
    private string s4;
    /// <summary>
    /// 网络同步-敌机子弹位置
    /// </summary>
    void EnemyBulletNet(string _name)
    {
        GameObject g = GameObject.Find(_name);
        if (!g) return;
        _bulletEntity.ObjName = g.name;
        _bulletEntity.PositionX = g.transform.position.x;
        _bulletEntity.PositionY = g.transform.position.y;
        _bulletEntity.PositionZ = g.transform.position.z;
        _bulletEntity.LocalScaleX = g.transform.localScale.x;
        _bulletEntity.LocalScaleY = g.transform.localScale.y;
        _bulletEntity.LocalScaleZ = g.transform.localScale.z;
        
        s3 = JsonConvert.SerializeObject(_bulletEntity);
        SourceDataEntity1.Content = s3;
        SourceDataEntity1.SourceDataType = SourceDataType.EnemyByName;
        s4 = JsonConvert.SerializeObject(SourceDataEntity1);

        NetWork.NetWork.EnemyBulletQueue.Enqueue(s4);
    }

    EnemyEntity _enemyEntity1 = new EnemyEntity();
    /// <summary>
    /// 敌人接收数据同步
    /// </summary>
    void EnemyHand()
    {
        if (EnemyQueue.Count > 0)
        {
            //读取数据
            try
            {
                _enemyEntity1 = JsonConvert.DeserializeObject<EnemyEntity>(EnemyQueue.Dequeue());
            }
            catch (Exception e)
            {
                print(e);
                return;
            }
            // 处理数据
            GameObject _find = GameObject.Find(_enemyEntity1.ObjName);
            if (_find)
            {
                _find.transform.position = new Vector3(_enemyEntity1.PositionX,
                    _enemyEntity1.PositionY, _enemyEntity1.PositionZ);
                _find.transform.localScale = new Vector3(_enemyEntity1.LocalScaleX,
                    _enemyEntity1.LocalScaleY, _enemyEntity1.LocalScaleZ);
            }
        }
    }
    
    EnemyBulletEntity _enemyBulletEntity = new EnemyBulletEntity();
    /// <summary>
    /// 敌人子弹接收数据同步
    /// </summary>
    void EnemyBulletHand()
    {
        if (EnemyBulletQueue.Count > 0)
        {
            //读取数据
            try
            {
                _enemyBulletEntity = JsonConvert.DeserializeObject<EnemyBulletEntity>(EnemyBulletQueue.Dequeue());
            }
            catch (Exception e)
            {
                print(e);
                return;
            }
            // 处理数据
            GameObject _find = GameObject.Find(_enemyBulletEntity.ObjName);
            if (_find)
            {
                _find.transform.position = new Vector3(_enemyBulletEntity.PositionX,
                    _enemyBulletEntity.PositionY, _enemyBulletEntity.PositionZ);
                _find.transform.localScale = new Vector3(_enemyBulletEntity.LocalScaleX,
                    _enemyBulletEntity.LocalScaleY, _enemyBulletEntity.LocalScaleZ);
            }
        }
    }
}
