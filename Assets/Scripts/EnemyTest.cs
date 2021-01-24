using NetWork.Entity;
using Newtonsoft.Json;
using Script.Enum;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
        
    public int speed;    //移动速度

    public GameObject bulletPrefab;
    private Transform firePos;

    private AudioClip BaoZha;


    void Awake()
    {
        //bulletPrefab = Resources.Load("Bullet/Lazer2") as GameObject;
        BaoZha = Resources.Load<AudioClip>("Music/baoz");
    }

    // Start is called before the first frame update
    void Start()
    {
        firePos = transform.GetChild(0);
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        InvokeRepeating(nameof(Attack), 0, Random.Range(2,5));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
        if (transform.position.x < -10)
        {
            DestroyThis();
        }
        
        //EnemyNet();
        
    }
    
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        /*Debug.Log("碰撞发生-----" + coll.name + "\n" +
                  coll.gameObject.layer + "\n" +
                  coll.gameObject.GetComponent<SpriteRenderer>().sortingLayerName);*/
        switch (coll.gameObject.GetComponent<SpriteRenderer>().sortingLayerName)
        {
            case   "BulletP1" :
                //1.显示爆炸动画
                GetComponent<Animator>().SetBool("Explosion", true);
                //2.爆炸声音
                AudioSource.PlayClipAtPoint(BaoZha, firePos.position, 1f);
                //2.销毁自己
                break;
            case   "BulletP2" :
                break;
            case   "Player1" :
                break;
            case   "Player2" :
                break;
        }
    }

    private SourceDataEntity SourceDataEntity = new SourceDataEntity();
    private GameObjEntity GameObjEntity = new GameObjEntity();
    /// <summary>
    /// 敌人发射的子弹
    /// </summary>
    private void Attack()
    {
        GameObjEntity.PrefabName = "EnemyBullet1";
        GameObject bullet = Instantiate(Resources.Load<GameObject>(
                Prefab.Prefab.Maps[GameObjEntity.PrefabName]),
            firePos.position,
            Quaternion.identity);
//        GameObject bullet = Instantiate(bulletPrefab, 
//            firePos.position, firePos.rotation);
        bullet.AddComponent<EnemyBullet>();
        bullet.GetComponent<EnemyBullet>().Speed = Random.Range(5, 8);
        bullet.name = "EnemyBullet" + GameManage.EnemyBulletInt++;
        bullet.SetActive(true);
        
        // 发送创建物体消息。。
        SendCreateGameObj(bullet);
        
        GameObject _find = GameObject.Find(bullet.name);
        if (_find)
        {
            EnemyManager.EnemyBulletNameList.Add(bullet.name);
        }
    }
        
    public void DestroyThis()
    {
        //
        EnemyManager.EnemyNameList.Remove(name);
        Destroy(gameObject);
    }
    
    /// <summary>
    /// 网络-创建子弹
    /// </summary>
    /// <param name="g"></param>
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
        string s2 = JsonConvert.SerializeObject(SourceDataEntity);

        while (!NetWork.NetWork.HasNewSendDatas)
        {
            // 通知发送
            NetWork.NetWork.SendDatasTemp = s2;
            NetWork.NetWork.HasNewSendDatas = true;
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