using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyIA : MonoBehaviour
{
    public float speed;
    public float chaseRadius;
    public float attackRadius;
    public float hp;
    public float damage;
    public bool shouldRotate;

    //public LayerMask whatIsPlayer;

    public Transform target;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    public Vector3 dir;

    private bool isInChaseRange;
    private bool isInAttackRange;
    // Start is called before the first frame update
    void Start()
    {
        try{
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            target = GameObject.FindWithTag("Player").transform;
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try{
            CheckDistance();
  

            if(shouldRotate)
            {
                anim.SetFloat("X",dir.x);
                anim.SetFloat("Y",dir.y);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    
    public void TakeDamage()
    {
        try{
            hp = hp - Player.damage;

            if(hp<=0)
            {
                FirebaseInit.enemigos_eliminados++;
                Destroy(gameObject);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    void CheckDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= chaseRadius
         && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            anim.SetBool("isRunning", true);

        }
    }

    private void OnCollisionEnter2D(Collision2D other) {

        try{
            if(other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Player>().TakeDamage(damage);
                Debug.Log("Hit al player");
            } 
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
}
