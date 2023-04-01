using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float delay = 2f;
    private float destroy = 4f;
    private Rigidbody2D rb;
    private new ParticleSystem particleSystem;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private bool played = false;
    private bool weGoBelow;

    private void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        particleSystem = GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        weGoBelow = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(BreakDown());
            if (!weGoBelow)
            { 
                FindObjectOfType<AudioManager>().Play("WeGoBelow");
                weGoBelow = true;
            }
        }
    }

    private IEnumerator BreakDown()
    { 
        yield return new WaitForSeconds(delay);
        if (!played)
        {
            particleSystem.Play();
            played = true;
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
        Destroy(gameObject, destroy);      
    }
}