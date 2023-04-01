using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float speed = 6.7f;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        transform.right = player.transform.position - transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
