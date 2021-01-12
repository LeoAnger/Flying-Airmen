using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Robot_Shooting_Game
{
    public class RobotTest : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Collider2D _coll;
        private Animator _anim;

        public int Hp = 10;    //血量
        public float Speed = 5, JumpForce = 18;
    
        public BulletCharacter bulletTemplate;
        public Transform firPoint;
    
        public AudioClip clip;
        public Text HpText;
    
    
        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _coll = GetComponent<Collider2D>();
            _anim = GetComponent<Animator>(); 
        
//        firPoint = this.transform.Find("ShootPos");
//        clip = Resources.Load<AudioClip>("Music/sandan");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        void FixedUpdate()
        {
            Movement();
            ShootFire();


        }

        void ShootFire()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                StopAllCoroutines();
                //ClearBulletsList();
                StartCoroutine(FirShot());
            }
        
        }
    
        void Movement()
        {
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            _rb.velocity = new Vector2(horizontalMove * Speed, _rb.velocity.y);
            _rb.velocity = new Vector2(_rb.velocity.x,vertical * Speed);

        }
    
        /// <summary>
        /// 发射散弹
        /// </summary>
        /// <returns></returns>
        IEnumerator FirShotgun()
        {
            Vector3 bulletDir = firPoint.transform.right; //由于资源的原因，我们这边的发射方向为物体的Up轴方向
            Quaternion leftRota = Quaternion.AngleAxis(-30, Vector3.forward);
            Quaternion RightRota = Quaternion.AngleAxis(30, Vector3.forward); //使用四元数制造2个旋转，分别是绕Z轴朝左右旋转30度
            for (int i=0;i<10;i++)     //散弹发射次数
            {
                for (int j=0;j<3;j++) //一次发射3颗子弹
                {
                    switch (j)
                    {
                        case 0:
                            CreatBullet(bulletDir, firPoint.transform.position);  //发射第一颗子弹，方向不需要进行旋转
                            break;
                        case 1:
                            bulletDir = RightRota * bulletDir;//第一个方向子弹发射完毕，旋转方向到下一个发射方向
                            CreatBullet(bulletDir, firPoint.transform.position);
                            break;
                        case 2:
                            bulletDir = leftRota*(leftRota * bulletDir); //右边方向发射完毕，得向左边旋转2次相同的角度才能到达下一个发射方向
                            CreatBullet(bulletDir, firPoint.transform.position);
                            bulletDir = RightRota * bulletDir; //一轮发射完毕，重新向右边旋转回去，方便下一波使用
                            break;
                    }
                }
                yield return new WaitForSeconds(0.5f); //协程延时，0.5秒进行下一波发射
            }
        }
    
        /// <summary>
        /// 发射散弹
        /// </summary>
        /// <returns></returns>
        IEnumerator FirShot()
        {
            Vector3 bulletDir = firPoint.transform.right; //由于资源的原因，我们这边的发射方向为物体的Up轴方向
            Quaternion leftRota = Quaternion.AngleAxis(-30, Vector3.forward);
            Quaternion RightRota = Quaternion.AngleAxis(30, Vector3.forward); //使用四元数制造2个旋转，分别是绕Z轴朝左右旋转30度
            for (int i=0;i<10;i++)     //散弹发射次数
            {
                for (int j=0;j<3;j++) //一次发射3颗子弹
                {
                    switch (j)
                    {
                        case 0:
                            AudioSource.PlayClipAtPoint(clip, firPoint.transform.position, 1f);
                            CreatBullet(bulletDir, firPoint.transform.position);  //发射第一颗子弹，方向不需要进行旋转
                            break;
                        case 1:
                            AudioSource.PlayClipAtPoint(clip, firPoint.transform.position, 1f);
                            CreatBullet(bulletDir, firPoint.transform.position);  //发射第一颗子弹，方向不需要进行旋转
                            break;
                        case 2:
                            AudioSource.PlayClipAtPoint(clip, firPoint.transform.position, 1f);
                            CreatBullet(bulletDir, firPoint.transform.position);  //发射第一颗子弹，方向不需要进行旋转
                            break;
                    }
                }
                yield return new WaitForSeconds(0.5f); //协程延时，0.5秒进行下一波发射
            }
        }
    
    
        public BulletCharacter CreatBullet(Vector3 dir,Vector3 creatPoint)
        {
            BulletCharacter bulletCharacter = Instantiate(bulletTemplate, creatPoint, Quaternion.identity);
            bulletCharacter.gameObject.SetActive(true);
            bulletCharacter.dir = dir;
            return bulletCharacter;
        }
    
        private void Damage(int damage)
        {
            if (Hp > 0)
            {
                Hp -= 1;
                //print("玩家血量：" + Hp);
                HpText.text = "HP : " + Hp;
                if (Hp <= 0)
                {
                    //玩家死亡
                    Hp = 0;
                    
                    //UnityEditor.EditorApplication.isPlaying = false;
                    Application.Quit();
                }
                else
                {
                    //受伤
                }
            }
        }
    }
}
