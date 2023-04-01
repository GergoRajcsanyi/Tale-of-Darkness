using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] float degree = 30f;
    [SerializeField] Vector3 axis = Vector3.forward;

    void Update()
    {
        transform.Rotate(axis.normalized * degree * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FallDetector"))
        {
            gameObject.SetActive(false);
        }
    }
}
