using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Variables for audio
    AudioSource audioSource;
    public AudioClip HitClip;
    public AudioClip Throwclip;
    
    public InputAction talkAction;
    //Variables for projectiles
    public GameObject projectilePrefab;
    public InputAction launchAction;
    //Variables for animation
    Animator animator;
    Vector2 moveDirection = new Vector2 (1,0);

    //Variables for temporary invincibiliy
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;
    
    //Variables for player character movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;

    //variables for Health system
    public int maxHealth = 5;
    int currentHealth;
    public int health {get {return currentHealth;}}
    public GameObject DamagePrefab;
    public GameObject HealthPrefab;
    public GameManagerScript gameManager;
    private bool isDead;

    //Variables for game over screen
    public int score;
    public bool gameOver=false;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        launchAction.Enable();
        launchAction.performed += Launch;
         
        talkAction.Enable();
        talkAction.performed += FindFriend;
        
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

    }
  
    // Update is called once per frame
    void Update()
    {
        
       move = MoveAction.ReadValue<Vector2>();
       if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
       {
        moveDirection.Set(move.x, move.y);
        moveDirection.Normalize();
       }
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown<0)
            isInvincible = false;
            
        }
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

         if (health <=0 && !isDead)
        {
            isDead=true;
            gameManager.gameOver();
            speed=0;
        }

        if (Input.GetKey(KeyCode.R)) // check to see if the player is pressing R

        {

            if (gameOver == true) // check to see if the game over boolean has been set to true

            {

              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene, which results in a restart of whatever scene the player is currently in

            }

        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed *Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
    public void ChangeHealth (int amount)
    { 
        if (amount <0)
        {
            if (isInvincible)
            
                return;
            
            isInvincible = true;
            damageCooldown = timeInvincible;
            GameObject DamageParticles = Instantiate(DamagePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(HitClip);
            
        }
   
        if (amount >0)
        {
            Instantiate(HealthPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }
       currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
     UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }
     

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }



   void Launch (InputAction.CallbackContext context)
   {
    GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
    Projectile projectile = projectileObject.GetComponent<Projectile>();
    projectile.Launch(moveDirection, 300);
    animator.SetTrigger("Launch");
    audioSource.PlayOneShot(Throwclip);
   }
   void FindFriend(InputAction.CallbackContext context)
{
RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));
if (hit.collider != null)
{
NPCscript NPC = hit.collider.GetComponent<NPCscript>();
if (NPC != null)
{
UIHandler.instance.DisplayDialogue();
            }
        }
    } 
    public void ChangeScore (int scoreAmount)
    { 
        score+=scoreAmount;
        
}
}