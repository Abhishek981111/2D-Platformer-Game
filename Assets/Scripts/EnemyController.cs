using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float enemySpeed;
    public int movingRight = 1;
    public GameObject groundDetector;
    public float rayDistance;
    public Animator enemyAnimator;
    public int maxHealth = 100;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        enemyAnimator.SetBool("IsPatrol", true);
    }

    private void Update()
    {
        PatrolEnemy();
    }

    private void PatrolEnemy()
    {
        transform.Translate(movingRight * Vector2.right * enemySpeed * Time.deltaTime);

        RaycastHit2D hit = Physics2D.Raycast(groundDetector.transform.position, Vector2.down, rayDistance);

        if(!hit)
        {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            movingRight = movingRight * -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            SoundManager.Instance.PlayMusic(Sounds.EnemyAttack);
            controller.DecreaseHealth();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //Play hurt animation
        enemyAnimator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Died!!");
        
        //Die animation
        enemyAnimator.SetBool("IsDead", true);
        SoundManager.Instance.Play(Sounds.EnemyDeath);

        //Disable enemy
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
