using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterChanger : MonoBehaviour, DataInterface
{
    #region Config

    [Header("COMPONENTS")]
    public Animator animator;
    private int chapterToLoad;
    [SerializeField] private Button continueGame;
    
    private bool exit = false;
    private bool mainMenu = false;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        exit = false;
        mainMenu = false;

        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!DataManager.instance.HasGameData())
            {
                continueGame.interactable = false;
            }
        }
    }
    #endregion

    #region Fade & Scene Setup
    public void FadeToNextChapter()
    {
        FadeToChapter(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FadeToChapter(int chapterIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex == 5 && BossFightManager.isTheBattleOver)
        {
            chapterToLoad = chapterIndex;
            animator.SetTrigger("FadeInWhite");
            BossFightManager.isTheBattleOver = false;
        }
        else 
        {
            chapterToLoad = chapterIndex;
            animator.SetTrigger("Fade");
        }
    }

    public void OnFadeComplete()
    {
        if (mainMenu)
        {
            Time.timeScale = 1f;
            PauseMenu.GameIsPaused = false;
            AudioListener.pause = false;
        }
        else if (exit)
        {
            Application.Quit();
        }
        SceneManager.LoadScene(chapterToLoad);
    }
    #endregion

    #region Menus

    public GameObject overWriteUI;

    public void StartNewGame()
    {
        DataManager.instance.NewGame();
        FadeToChapter(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreateGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!DataManager.instance.HasGameData())
            {
                StartNewGame();
            }
            else
            {
                overWriteUI.SetActive(true);
            }
        }
    }

    public void ContinueGame()
    {
        FadeToChapter(chapterToLoad);
    }

    public void QuitGame()
    {
        animator.SetTrigger("Fade");
        exit = true;
    }

    public void MainMenu()
    {
        FadeToChapter(0);
        mainMenu = true;
        DataManager.instance.SaveGame();
    }
    #endregion

    #region Save System
    public void LoadData(GameData data)
    {
        this.chapterToLoad = data.sceneIndex;
    }

    public void SaveData(GameData data)
    {
        if (this.chapterToLoad >= 2)
        {
            data.sceneIndex = this.chapterToLoad;
        }
    }
    #endregion
}
