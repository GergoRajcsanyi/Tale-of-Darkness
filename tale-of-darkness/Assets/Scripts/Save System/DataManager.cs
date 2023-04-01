using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    [Header("CONFIGURATION")]
    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    [SerializeField] private bool debugger = false;

    private GameData gameData;
    private List<DataInterface> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
    }

    #region Scene Config

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        if (SceneManager.GetActiveScene().buildIndex <= 4) SaveGame();
        Resources.UnloadUnusedAssets();     
    }

    #endregion

    #region Game Data Config
    public void NewGame()
    { 
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null && debugger)
        {
            NewGame();
        }

        if (this.gameData == null)
        {
            return;
        }

        foreach (DataInterface dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (this.gameData == null)
        {
            return;
        }

        foreach (DataInterface dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        dataHandler.Save(gameData);
        System.GC.Collect();
    }
    #endregion

    #region Managing Data

    private void OnApplicationQuit()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    private List<DataInterface> FindAllDataPersistenceObjects()
    {
        IEnumerable<DataInterface> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<DataInterface>();

        return new List<DataInterface>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }
    #endregion
}
