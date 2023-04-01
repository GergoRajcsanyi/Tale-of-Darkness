using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour, DataInterface
{
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (corruptionImage != null && corruption) corruptionImage.fillAmount = 1f;
        else corruptionImage.fillAmount = 0f;

        if (insightImage != null && insight) insightImage.fillAmount = 1f;
        else insightImage.fillAmount = 0f;
    }

    void Update()
    {
        if (corruptionCooldown)
        {
            corruptionImage.fillAmount -= 1f / darkDuration * Time.deltaTime;
            if (corruptionImage.fillAmount <= 0f) corruptionImage.fillAmount = 0f;
        }
    }

    #region Ability: Corruption

    [Header("CORRUPTION SETTINGS")]
    [Range(0f, 8f)] public float darkDuration = 5f;
    public static bool corruption = false;
    public static bool isCorruptionActive;
    private bool corruptionCooldown = false;
    public Image corruptionImage;
    private Rigidbody2D rb;
    private Animator animator;

    public void Corruption()
    {
        if (corruption && !corruptionCooldown && !DeathRespawn.isDead)
        {
            StartCoroutine(Ability3(darkDuration));
        }

        IEnumerator Ability3(float duration)
        {
            corruptionCooldown = true;
            isCorruptionActive = true;
            animator.SetBool("Corruption", true);
            rb.gravityScale = 0f;

            yield return new WaitForSeconds(duration);
            isCorruptionActive = false;
            rb.gravityScale = 3.3f; 
            animator.SetBool("Corruption", false);
            Invoke("ResetCorruption", 8f);
        } 
    }
    void ResetCorruption()
    {
        corruptionCooldown = false;
        corruptionImage.fillAmount = 1f;
    }

    #endregion

    #region Ability: Granted Eyes

    [Header("EYE OF WISDOM SETTINGS")]
    public GameObject flashlight;
    public static bool eyeOfWisdom = false;
    public bool isEyeOfWisdomActive;
    [Range(0f, 60f)] public float lightDuration = 30f;

    [Header("INSIGHT SETTINGS")]
    public ParticleSystem vision;
    public static bool insight = false;
    private bool insightCooldown = false;
    public Image insightImage;
    [SerializeField] private BossFightManager theDarkness;
   
    public void ElderSigns()
    {
        //Eye of Wisdom
        if (eyeOfWisdom && !insight && !isEyeOfWisdomActive && !DeathRespawn.isDead)
        {      
            StartCoroutine(Ability2(lightDuration));
        }

        //Insight
        if (eyeOfWisdom && insight && BossFightManager.inTheFight && !DeathRespawn.isDead)
        {            
            if (!insightCooldown)
            {
                vision.Play();
                insightImage.fillAmount = 0f;
                if (flashlight != null) Destroy(flashlight);
                if (theDarkness != null) theDarkness.DamageDarkness();
                Invoke("ResetInsight", 30f);
                insightCooldown = true;
            }
        }

        IEnumerator Ability2(float duration)
        {
            isEyeOfWisdomActive = true;
            flashlight.SetActive(true);

            yield return new WaitForSeconds(duration);
            flashlight.SetActive(false);
            isEyeOfWisdomActive = false;
        }  
    }
    void ResetInsight()
    {
        insightCooldown = false;
        insightImage.fillAmount = 1f;
    }

    #endregion

    #region Save System

    public void LoadData(GameData data)
    {
        foreach(KeyValuePair<string, bool> pair in data.abilityCollected)
        {
            if (pair.Key == "eye-of-wisdom")
            { 
                eyeOfWisdom = pair.Value;
            }

            if (pair.Key == "corruption")
            {
                corruption = pair.Value;
            }

            if (pair.Key == "insight")
            {
                insight = pair.Value;
            }
        }
    }
    public void SaveData(GameData data)
    {
        //Nothing to save here...
    }
    #endregion
}
