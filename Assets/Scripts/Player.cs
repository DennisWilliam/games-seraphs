using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float movementSpeed;
    public float jumpPower;

    [SerializeField] LayerMask groundLayer;

    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] Transform groundCheck_transform;
    [SerializeField] Camera cam;
    [SerializeField] GameObject wand_gameObject;
    [SerializeField] GameObject bullet_prefab;
    [SerializeField] GameObject bullet_parent;
    [SerializeField] Transform wandCristal_transform;

    bool canShoot = true;

    void Start()
    {
        movementSpeed = 5;
        jumpPower = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        WandAimAndShoot();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal") * movementSpeed;
        rigidbody.velocity = new Vector2(horizontal, rigidbody.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpPower);
        }
    }

    //Make object follow the mouse pointer direction
    void WandAimAndShoot()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(
            mousePos.y - wand_gameObject.transform.position.y, 
            mousePos.x - wand_gameObject.transform.position.x 
            ) * Mathf.Rad2Deg;

        wand_gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Make on each mouse click, dispair bullets
        if (Input.GetMouseButton(0) && canShoot)
        {
            StartCoroutine(ShootingCoolDown());
            GameObject bullet = Instantiate(
                bullet_prefab, 
                wandCristal_transform.position, 
                wand_gameObject.transform.rotation, 
                bullet_parent.transform
                );
            bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(2, 0), ForceMode2D.Impulse);
            Destroy(bullet, 10);
        }
    }

    //Check if there is any collision in objects that have 'ground' in your Layer (Need to add this layer in floor object)
    bool IsGrounded()
    {
        //Check if the object with the groundCheck have contact with objects with layer 'ground'
        return Physics2D.CircleCast(groundCheck_transform.position, 0.1f, Vector2.down, 0.1f, groundLayer);
    }

    //Coroutine that block mouse click after clicking for 1 second
    IEnumerator ShootingCoolDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(1);
        canShoot = true;
    }
}
