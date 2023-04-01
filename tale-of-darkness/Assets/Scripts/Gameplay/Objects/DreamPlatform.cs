using System.Collections;
using UnityEngine;

public class DreamPlatform : MonoBehaviour
{
    public GameObject nextPlatform;
    public GameObject originalPlatform;
    private Animator animator;
    private float jumpTime = 2.5f;

    private void OnEnable()
    {
        DeathRespawn.PlayerDied += RespawnPlatform;
    }

    private void OnDisable()
    {
        DeathRespawn.PlayerDied -= RespawnPlatform;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (nextPlatform != null) nextPlatform.SetActive(true);
            StartCoroutine(Disappear());
        }
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(jumpTime);
        animator.SetTrigger("Disappear");       
    }

    void RespawnPlatform()
    {
        this.gameObject.SetActive(false);
        originalPlatform.SetActive(true);
        if (nextPlatform != null) nextPlatform.SetActive(false);
    }
}