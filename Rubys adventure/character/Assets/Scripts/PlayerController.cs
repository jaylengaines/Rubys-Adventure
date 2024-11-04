using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    public int health {get {return currentHealth;}}
    int currentHealth;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        launchAction.Enable();
        launchAction.performed += Launch;
        rigidbody2d = GetComponent<Rigidbody2D>();
        
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        talkAction.performed += FindFriend;
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
            {
                isInvincible = false;
            }
        }
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);
    }
    public void ChangeHealth (int amount)
    {
        if (amount <0)
        {
            if (isInvincible)
            
                return;
            
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
     UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed *Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
   void Launch (InputAction.CallbackContext context)
   {
    GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
    Projectile projectile = projectileObject.GetComponent<Projectile>();
    projectile.Launch(moveDirection, 300);
    animator.SetTrigger("Launch");
   }
   void FindFriend(InputAction.CallbackContext context)
{
RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));


if (hit.collider != null)
{
NPCscript character = hit.collider.GetComponent<NPCscript>();
if (character != null)
{
UIHandler.instance.DisplayDialogue();
}
}
} 
}