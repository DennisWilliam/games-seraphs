using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour

{
    public EnemySO enemySO;

    public float health;
    public float height;

    GameObject player;
    GameObject buller_parent;
    SpriteRenderer spriteRenderer;

    float distancePlayer;
    bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        buller_parent = GameObject.Find("BulletParent");
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (enemySO.sprite != null)
        {
            spriteRenderer.sprite = enemySO.sprite;
        }

        health = enemySO.healthMax;
        height = Random.Range(enemySO.height -0.5f, enemySO.height + 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = new Color(
            spriteRenderer.color.r, 
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            Mathf.Pow(health / enemySO.healthMax, 0.7f)
            );

        if (health < 0)
        {
            Destroy(gameObject);
        }

        if(distancePlayer <= enemySO.distance && canShoot)
        {
            StartCoroutine(ShootingCoolDown());
            GameObject bullet = Instantiate(
                enemySO.bullet_prefab,
                transform.position,
                Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z - 90))
                );

            bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(2, 0), ForceMode2D.Impulse);
            Destroy(bullet, 10);
        }

        RotateFowardsPlayer();
        Move();
    }


    //Rotate enemy face to player position
    public void RotateFowardsPlayer()
    {
        float angle = Mathf.Atan2(
            player.transform.position.y - transform.position.y,
            player.transform.position.x - transform.position.x
            ) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
    }

    //Check the distance between the player and try to aproximate slowly to him.
    public void Move()
    {
        distancePlayer = Vector2.Distance(transform.position, player.transform.position);

        if(distancePlayer > enemySO.distance || transform.position.y > height)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - Mathf.Pow((distancePlayer - height + 2) * 0.017f, 3),
                0
                ) ;
        }else if (transform.position.y < height - 1)
        {
            transform.position = new Vector3(
               transform.position.x,
               transform.position.y + Mathf.Pow((distancePlayer - height + 2) * 0.017f, 3),
               0
               );
        }

        if (player.transform.position.x > transform.position.x)
        {
            transform.position = new Vector3(
               transform.position.x,
               transform.position.y - Mathf.Pow((distancePlayer - height + 2) * 0.017f, 3),
               0
               );
        }else
        {
            transform.position = new Vector3(
               transform.position.x,
               transform.position.y + Mathf.Pow((distancePlayer - height + 2) * 0.017f, 3),
               0
               );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health -= 20;
        }
    }


    //Coroutine that block mouse click after clicking for 1 second
    IEnumerator ShootingCoolDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(enemySO.cooldown);
        canShoot = true;
    }
}
