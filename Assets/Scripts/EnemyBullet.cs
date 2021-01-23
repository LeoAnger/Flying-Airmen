using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    private bool b = true;
    public int Speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        Speed = Random.Range(4, 7);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * Speed);
        if (transform.position.x < -10)
        {
            //print("自行销毁子弹。。。");
            Destroy(gameObject);
            EnemyManager.EnemyBulletNameList.Remove(name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player" :
                if (this.CompareTag("EnemyBullet"))
                {
                    other.SendMessage("Damage", 1, SendMessageOptions.DontRequireReceiver);
                    EnemyManager.EnemyBulletNameList.Remove(name);
                    Destroy(gameObject);
                }
                break;
        }
    }
}