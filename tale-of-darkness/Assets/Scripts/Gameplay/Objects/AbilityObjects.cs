using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityObjects : MonoBehaviour, DataInterface
{
    #region Ability Object

    [Header("ABILITY COMPONENTS")]
    [SerializeField] private string id;
    private bool absorbed = false;
    private SpriteRenderer sprite;

    public GameObject popUp;
    public GameObject inGame;
    public GameObject disablePopUp;

    public Image abilityImage;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D()
    {
        if (!absorbed)
        {
            CollectAbility();
        }
    }

    private void CollectAbility()
    {
        absorbed = true;
        sprite.gameObject.SetActive(false);
        DataManager.instance.SaveGame();
        FindObjectOfType<AudioManager>().Play("NewAbility");

        switch (id)
        {
            case "shadow-platform":
                PopUp();
                ShadowPlatform.shadowPlatform = true;
                break;

            case "eye-of-wisdom":
                PopUp();
                Abilities.eyeOfWisdom = true;
                break;

            case "corruption":
                PopUp();
                Abilities.corruption = true;
                if (abilityImage != null) abilityImage.fillAmount = 1f;
                break;

            case "insight":
                PopUp();
                Abilities.insight = true;
                if (abilityImage != null) abilityImage.fillAmount = 1f;
                break;

            default: break;
        }
    }

    private void PopUp()
    {
        popUp.SetActive(true);
        inGame.SetActive(false);
        disablePopUp.SetActive(false);
        Time.timeScale = 0f;
        PauseMenu.GameIsPaused = true;
        Input.ResetInputAxes();
    }

    #endregion

    #region Save System
    public void LoadData(GameData data)
    {
        data.abilityCollected.TryGetValue(id, out absorbed);
        if (absorbed)
        { 
            sprite.gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.abilityCollected.ContainsKey(id))
        {
            data.abilityCollected.Remove(id);
        }
        data.abilityCollected.Add(id, absorbed);
    }
       
    #endregion
}