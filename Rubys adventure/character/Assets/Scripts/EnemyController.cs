using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool aggressive = true;
    Animator animator;
    // Public Variables
    public bool vertical;
    public float speed;
    public float changeTime = 3.0f;
    
    //Pirvate Variables
    Rigidbody2D rigidbody2d;
    float timer;
    int direction= 1;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <0)
        {
            direction = -direction;
            timer = changeTime;
        }
        
    }
    void FixedUpdate()
    {
        if (!aggressive)
        {
            return;
        }
        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + speed * direction* Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction* Time.deltaTime;
            animator.SetFloat("Move X",direction);
            animator.SetFloat("Move Y",0);

        }
        rigidbody2d.MovePosition(position);
        
    }
    void OnCollisionEnter2d(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }

    }
    public void Fix()
    {
        aggressive = false;
        rigidbody2d.simulated=false;
        animator.SetTrigger("Fixed");
    }

}
