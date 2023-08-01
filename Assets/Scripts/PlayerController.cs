using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public ScoreController scoreController;
    public Animator animator;
    public float speed;
    public float jumpForce;
    private bool isGrounded = true;
    private bool isCrouching = false;
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCol;
    public int playerHealth;
    [SerializeField] private Image[] hearts;
    public Transform startPosition;
    [SerializeField] private GameObject mainCamera;
    public GameOverController gameOverController;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;
    private float nextAttackTime = 0;
    public LayerMask enemyLayers;
    private AudioSource audioSource;
    private bool isMoving;

    private void Awake()
    {
        Debug.Log("Player controller awake");
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        boxCol = this.GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SoundManager.Instance.Play(Sounds.PlayerEntryVoice);
        startPosition.position = transform.position;
        UpdateHealthUI();
    }

    private void Update()
    {
        //for running animation
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("jump");

        MoveCharacter(horizontalInput, verticalInput);
        PlayMovementAnimation(horizontalInput, verticalInput);

        //Crouching
        Crouch(isCrouching);

        //for attacking animation
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        //for footsteps sound
        if(Input.GetAxisRaw("Horizontal") != 0 && isGrounded)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if(isMoving && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        if(!isMoving)
        {
            audioSource.Stop();
        }
    }

    private void MoveCharacter(float horizontalInput, float verticalInput)
    {
        //move character horizontally
        Vector3 position = transform.position;
        position.x += horizontalInput * speed * Time.deltaTime;
        transform.position = position;

        //move chararcter vertically
        if (verticalInput > 0 && isGrounded)
        {
            rigidbody2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void PlayMovementAnimation(float horizontalInput, float verticalInput)
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        Vector3 scale = transform.localScale;
        if(horizontalInput < 0)
        {
            scale.x = -1 * Mathf.Abs(scale.x);
        }
        else if(horizontalInput > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;

        //Jump
        if(verticalInput > 0)
        {
            animator.SetBool("Jump", true);
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
    }

    //Crouching
    private void Crouch(bool crouch)
    {
        crouch = isCrouching;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            animator.SetBool("Crouch", isCrouching);
        }
        else
        {
            isCrouching = false;
            animator.SetBool("Crouch", isCrouching);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
        //For Death Collider
        else if(other.gameObject.tag == "DeathCollider")
        {
            PlayDeathAnimation();
            gameOverController.PlayerDied();
            //StartCoroutine("Dead");
        }
    }

    public void PickUpKey()
    {
        Debug.Log("Player picked up the key");
        scoreController.IncreaseScore(10);
    }

    public void DecreaseHealth()
    {
        playerHealth--;
        UpdateHealthUI();
        if (playerHealth <= 0)
        {
            PlayDeathAnimation();
            PlayerDeath();
        }
        else
        {
            transform.position = startPosition.position;
        }
    }

    public void PlayerDeath()
    {
        mainCamera.transform.parent = null;
        // Setting death UI panel to active
        gameOverController.PlayerDied();
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition;
        this.enabled = false;
    }

    public void UpdateHealthUI()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < playerHealth)
            {
                hearts[i].color = Color.red;
            }
            else
            {
                hearts[i].color = Color.black;
            }
        }
    }

    public void PlayDeathAnimation()
    {
        animator.SetBool("Death", true);
    }

    void Attack()
    {
        // Play an attack animation
        animator.SetTrigger("Attack");
        SoundManager.Instance.Play(Sounds.PlayerAttack);

        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}