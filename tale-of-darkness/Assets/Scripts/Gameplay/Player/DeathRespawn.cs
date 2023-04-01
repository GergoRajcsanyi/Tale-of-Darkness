using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathRespawn : MonoBehaviour, DataInterface
{
    private Vector3 respawnPoint;
    private Animator animator;
    private Rigidbody2D rb;

    private bool hit;
    private GameObject stalactite;

    public delegate void Dead();
    public static event Dead PlayerDied;

    public delegate void Arise();
    public static event Arise PlayerRespawned;

    public int deathCounter = 0;
    public static bool isDead = false;

    private bool chapter2 = false;
    private bool chapter3 = false;
    private bool chapter4 = false;
    private bool eclipseSuite;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        respawnPoint = transform.position;
        eclipseSuite = false;
    }

    private void Respawn()
    {
        animator.SetBool("Death", false);
        transform.position = respawnPoint;
        rb.bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<PlayerController>().enabled = true;
        gameObject.GetComponentInChildren<ShadowPlatform>().enabled = true;
        animator.Play("Player_Idle");
        deathCounter++;
        isDead = false;
        if (hit)
        {
            stalactite.GetComponent<FallingObjects>().StalactiteRespawner();
            hit = false;
        }
        if (Abilities.isCorruptionActive)
        {
            animator.SetBool("Corruption", false);
        }
        if (PlayerRespawned != null) PlayerRespawned();
    }

    private void Death()
    {
        FindObjectOfType<AudioManager>().Play("Death");
        animator.SetBool("Death", true);
        gameObject.GetComponent<PlayerController>().enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        gameObject.GetComponentInChildren<ShadowPlatform> ().enabled = false;
        isDead = true;
        if (PlayerDied != null) PlayerDied();
    }

    #region Triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Respawn"))
        {
            respawnPoint = transform.position;
            DataManager.instance.SaveGame();
        }

        if (other.CompareTag("FallDetector") || other.CompareTag("Thorns"))
        {
            Death();
        }

        if (other.CompareTag("Teleporter") && !Abilities.isCorruptionActive)
        {
            Death();
        }

        if (other.CompareTag("EclipseSuite"))
        {
            if (!eclipseSuite)
            {
                FindObjectOfType<AudioManager>().Play("TheEclipse");
                eclipseSuite = true;
            }      
        }

        //Managing the spawn point for the next Chapter
        if (other.gameObject.CompareTag("NextScene"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                chapter2 = true;
                FindObjectOfType<ChapterChanger>().FadeToNextChapter();
            }
            
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                chapter3 = true;
                FindObjectOfType<ChapterChanger>().FadeToNextChapter();
            }

            if (SceneManager.GetActiveScene().buildIndex == 4)
            {
                chapter4 = true;
                FindObjectOfType<ChapterChanger>().FadeToNextChapter();
            }
            
        }
    }
    #endregion

    #region Collisions

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("FallingObject"))
        {
            Death();
            hit = true;
            stalactite = other.gameObject;
        }
        if (other.gameObject.CompareTag("BossAttack"))
        {
            Death();
        }
    }
    #endregion

    #region Save System
    public void LoadData(GameData data)
    {

        this.deathCounter = data.deathCount;
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        if (chapter2 && !chapter3 && !chapter4)
        {
            data.playerPosition = new Vector3((float)-26.8, (float)-0.21, (float)0);
            chapter2 = false;
        }

        else if (!chapter2 && chapter3 && !chapter4)
        {
            data.playerPosition = new Vector3((float)0.23, (float)-4.18, (float)0);
            chapter3 = false;
        }

        else if (!chapter2 && !chapter3 && chapter4)
        {
            data.playerPosition = new Vector3((float)0.23, (float)-4.18, (float)0);
            chapter4 = false;
        }

        else 
        {
            data.deathCount = this.deathCounter;
            data.playerPosition = this.respawnPoint;
        }
    }
    #endregion
}
