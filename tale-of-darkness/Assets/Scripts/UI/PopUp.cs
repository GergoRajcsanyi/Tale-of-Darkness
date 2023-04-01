using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public GameObject popUp;
    public GameObject inGame;

    public void AnimationFinished()
    {
        popUp.SetActive(false);
        inGame.SetActive(true);
        Time.timeScale = 1f;
        Input.ResetInputAxes();
        PauseMenu.GameIsPaused = false;
    }
}
