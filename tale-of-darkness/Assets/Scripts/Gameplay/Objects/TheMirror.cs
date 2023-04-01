using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheMirror : MonoBehaviour, DataInterface
{
    public GameObject hideHUD;
    public GameObject showCanvas;
    public Text counter;
    public static int madnessCount = 0;
    private int deathCounter = 0;
    private float usageCooldown = 10f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        MirrorOfMadness();
    }

    private void MirrorOfMadness()
    {
        Time.timeScale = 0f;
        PauseMenu.GameIsPaused = true;
        Input.ResetInputAxes();

        FindObjectOfType<AudioManager>().Play("Mirror");
        hideHUD.SetActive(false);
        showCanvas.SetActive(true);
        deathCounter = FindObjectOfType<DeathRespawn>().deathCounter;
        counter.text = "Your failure caused the Dreamlands to be perished " + deathCounter + " times.";

        this.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Exit()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;

        hideHUD.SetActive(true);
        showCanvas.SetActive(false);

        StartCoroutine(SavePlayer(usageCooldown));
        IEnumerator SavePlayer(float cooldown)
        {
            yield return new WaitForSeconds(cooldown);
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    #region Save System
    public void LoadData(GameData data)
    {
        madnessCount = data.madnessCount;
    }

    public void SaveData(GameData data)
    {
        data.madnessCount = madnessCount;
    }
    #endregion
}