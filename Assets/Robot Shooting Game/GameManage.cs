using UnityEngine;

namespace Robot_Shooting_Game
{
    public class GameManage : MonoBehaviour
    {
    
        public GameObject[] enemys;
        // Start is called before the first frame update
        void Start()
        {
            //enemys = Resources.LoadAll<GameObject>("Enemys");
            InvokeRepeating(nameof(CreateEnemys), 0, 2);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        private void CreateEnemys()
        {
            int num = Random.Range(0, enemys.Length);
            GameObject enemy = Instantiate(enemys[num],
//            new Vector3(Random.Range(-2.2f, 2.2f), 6, 1),
                new Vector3(10, Random.Range(-4f, 4f), 1),
                Quaternion.identity);

            //enemy.AddComponent<EnemyTest>();
            int speed = Random.Range(1, 3);
            enemy.GetComponent<EnemyTest>().speed = speed;
            print("speed:" + speed);

            enemy.tag = "Enemy";
            enemy.SetActive(true);

        }
    }
}
