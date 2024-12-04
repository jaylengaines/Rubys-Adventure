using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    AudioSource audioSource;
    bool aggressive = true;
    Animator animator;
    // Public Variables
    public bool vertical;
    public float speed;
    public float changeTime = 3.0f;
    public ParticleSystem smokeEffect;
    
    //Pirvate Variables
    Rigidbody2D rigidbody2d;
    float timer;
    int direction= 1;
    
    public PlayerController playerController;
    // Start is called before the first frame update

    void Start()
    {
        
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby
        audioSource = GetComponent<AudioSource>();
        if (rubyControllerObject != null)

        {

            playerController = rubyControllerObject.GetComponent<PlayerController>(); //and this line of code finds the rubyController and then stores it in a variable

            print ("Found the RubyConroller Script!");

        }

        if (playerController == null)

        {

            print ("Cannot find GameController Script!");

        }
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

       
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
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
    void OnTriggerEnter2d(Collision2D other)
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
        GetComponent<Rigidbody2D>().simulated=false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();

        if (playerController != null)
        {
                playerController.ChangeScore(1); //this line of code is increasing Ruby's health by 1!
                audioSource.Stop();
    
        }
    }
     

}
