using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjects : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    public float distance;
    bool isFalling = false;
    private Vector3 originalPos;
    [Range(-20, 20)] public int tilt = 0;
    private ParticleSystem dust;

    private void Awake()
    {
        originalPos = transform.position;
        dust = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Physics2D.queriesStartInColliders = false;
        if (isFalling == false)
        {
            Vector3 tilted = Quaternion.Euler(0, 0, tilt) * Vector3.down;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, tilted,distance);

            if (hit.transform != null)
            {
                dust.Stop();
                if (hit.transform.CompareTag("Player"))
                {
                    
                    rb.gravityScale = 6;
                    isFalling = true;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.gravityScale = 0;
            boxCollider2D.enabled = false;
            Audio();
        }

        if (other.gameObject.CompareTag("Line"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FallDetector"))
        {
            Destroy(gameObject);
        }       
    }

    public void StalactiteRespawner()
    {
        transform.position = originalPos;
        gameObject.SetActive(true);
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        isFalling = false;
        dust.Play();
    }

    private void Audio()
    {
        int random = Random.Range(0, 2);
        if (random == 0) FindObjectOfType<AudioManager>().Play("Stalactite1");
        else FindObjectOfType<AudioManager>().Play("Stalactite2");
    }
}
