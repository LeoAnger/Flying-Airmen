using System;
using NetWork.Entity;
using Newtonsoft.Json;
using UnityEngine;

namespace Script.NetWork
{
    public class NetPlayer2 : MonoBehaviour
    {
        public static string sourceDatas = "";
        public static string sourceDatasTemp = "";    //缓冲数据
        public static bool isReadedData = true;
        private Player2Entity deserializeObject = new Player2Entity();

        private int isReadedNum = 0;

        private Animator _animator;
//    public static int DataSerialNumber;
        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            while (!isReadedData)
            {
                //读取数据
                sourceDatas = sourceDatasTemp;
                isReadedData = true;
                try
                {
                    deserializeObject = JsonConvert.DeserializeObject<Player2Entity>(sourceDatas);
                }
                catch (Exception e)
                {
                    print(e);
                    break;
                }
                
                /*
             * 1.判断数据是否合理（掉帧）
             * 2.获取Content进行反序列化
             * 3.逻辑处理
             */
                transform.position = new Vector3(deserializeObject.PositionX, deserializeObject.PositionY, deserializeObject.PositionZ);
                transform.localScale = new Vector3(deserializeObject.LocalScaleX, deserializeObject.LocalScaleY, deserializeObject.LocalScaleZ);
                _animator.SetFloat("running", deserializeObject.running);
                _animator.SetBool("jumping", deserializeObject.jumping);
                _animator.SetBool("idle", deserializeObject.idle);
                _animator.SetBool("falling", deserializeObject.falling);
            }       
        }
    
        void FixedUpdate()
        {
         
        }
    
    }
}
