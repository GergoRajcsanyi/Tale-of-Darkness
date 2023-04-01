using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadnessObjects : MonoBehaviour
{
    private float cooldown = 2f;
    public GameObject destination;
    private GameObject player;

    public GameObject passage;
    public GameObject passagePopUp;
    private bool isPassageOpen = false;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (TheMirror.madnessCount >= 5) passage.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Abilities.isCorruptionActive)
        {
            TheMirror.madnessCount++;
            if (TheMirror.madnessCount == 4 && !isPassageOpen)
            { 
                passagePopUp.SetActive(true);
                isPassageOpen = true;
                passage.SetActive(false);
            }

            FindObjectOfType<AudioManager>().Play("Madness");
            destination.GetComponentInParent<BoxCollider2D>().enabled = false;
            player.transform.position = new Vector2(destination.transform.position.x, destination.transform.position.y);
            
            StartCoroutine(SavePlayer(cooldown));
            IEnumerator SavePlayer(float cooldown)
            {
                yield return new WaitForSeconds(cooldown);
                destination.GetComponentInParent<BoxCollider2D>().enabled = true;            
            }
        }
    }
}
