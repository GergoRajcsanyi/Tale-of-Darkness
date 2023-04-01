using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    #region Executing Settings

    [SerializeField] private MenuManager settings;
    [SerializeField] private Image joystickOutline;
    [SerializeField] private Image joystickHandle;

    private void OnEnable()
    {
        settings.OnVisibiltyChanged += Visible_Joystick;
    }

    private void OnDisable()
    {
        settings.OnVisibiltyChanged -= Visible_Joystick;
    }

    private void Visible_Joystick(object sender, System.EventArgs e)
    {
        var handleTempColor = joystickHandle.color;
        var outlineTempColor = joystickOutline.color;

        if (MenuManager.visibility)
        {
            handleTempColor.a = 0.102f;
            joystickHandle.color = handleTempColor;

            outlineTempColor.a = 0.102f;
            joystickOutline.color = outlineTempColor;
        }
        else
        {
            handleTempColor.a = 0f;
            joystickHandle.color = handleTempColor;

            outlineTempColor.a = 0f;
            joystickOutline.color = outlineTempColor;
        }
    }
    #endregion

    #region Pause

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject disablePopUp;

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        disablePopUp.SetActive(false);
        AudioListener.pause = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioListener.pause = false;
    }

    public void PlayAudio()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }
    #endregion
}