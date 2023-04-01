using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    [Header("BOSS ARENA SETUP")]
    [SerializeField] private BossTrigger bossTrigger;
    [SerializeField] private GameObject middlePlatform;
    [SerializeField] private GameObject boxCollider2D;
    [SerializeField] private ParticleSystem particles;
    private GameObject player;
    private bool weGoBelow;

    [Header("BOSS SETUP")]
    [SerializeField] private Animator animator;
    BossHealth bossHealth = new BossHealth(4);
    public static bool inTheFight;
    public static bool isTheBattleOver;

    [Header("METEOR SHOWER")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Collider2D spawnBox;
    private WaitForSeconds meteorDelay = new WaitForSeconds(0.3f);
    private WaitForSeconds Recharge = new WaitForSeconds(4f);
    private WaitForSeconds attackDelay = new WaitForSeconds(5f);

    [Header("PROJECTILE")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] projectileSpawns;
    List<GameObject> projectileList = new List<GameObject>();
    public GameObject projectile;

    public enum Stage {
        MoonPresence,
        Stage_1,
        Stage_2,
        Stage_3,
    }
    private Stage stage;

    private void OnEnable()
    {
        DeathRespawn.PlayerDied += ArenaResetOnDeath;
        DeathRespawn.PlayerRespawned += ArenaResetOnRespawn;
    }

    private void OnDisable()
    {
        DeathRespawn.PlayerDied -= ArenaResetOnDeath;
        DeathRespawn.PlayerRespawned -= ArenaResetOnRespawn;
    }

    private void Awake()
    {
        stage = Stage.MoonPresence;
        player = GameObject.Find("Player");
        weGoBelow = false;
    }

    private void Start()
    {
        bossTrigger.OnPlayerEnterTrigger += Boss_PlayerEnter;
        bossHealth.OnDamaged += Boss_OnDamaged;
        bossHealth.OnDefeated += Darkness_Banished;

        boxCollider2D.SetActive(false);
        inTheFight = false;
        isTheBattleOver = false;
    }

    #region Boss Fight Logic

    private void Boss_PlayerEnter(object sender, System.EventArgs e)
    {
        boxCollider2D.SetActive(true);
        animator.SetTrigger("Start");
        FindObjectOfType<AudioManager>().Play("BossStart");
        FindObjectOfType<AudioManager>().Play("BossTheme");
        FindObjectOfType<AudioManager>().FadeOut("TheNightmare");
        inTheFight = true;

        FinalBattle();
        bossTrigger.OnPlayerEnterTrigger -= Boss_PlayerEnter;
    }

    private void Boss_OnDamaged(object sender, System.EventArgs e)
    {
        switch (stage) {
            case Stage.Stage_1:
                if (bossHealth.GetHealth() == 3)
                {
                    FindObjectOfType<AudioManager>().Play("BossPhase");
                }
                if (bossHealth.GetHealth() == 2)
                { 
                    NextStage();
                }
                break;
            case Stage.Stage_2:
                if (bossHealth.GetHealth() == 1)
                {
                    NextStage();
                }
                break;
        }
    }

    private void Darkness_Banished(object sender, System.EventArgs e)
    {
        animator.SetTrigger("Defeated");
        FindObjectOfType<AudioManager>().Play("BossDeath");
        FindObjectOfType<AudioManager>().FadeOut("BossTheme");
        player.GetComponent<PlayerController>().enabled = false;
        DestroyAllShadows();
        StopAllCoroutines();
        bossHealth.OnDamaged -= Boss_OnDamaged;
        bossHealth.OnDefeated -= Darkness_Banished;
    }

    public void FinalBattle()
    {
        NextStage();
    }

    private void NextStage()
    {
        switch (stage) {
            case Stage.Stage_1:
                stage = Stage.Stage_2;
                FindObjectOfType<AudioManager>().Play("BossPhase");
                StopCoroutine("AttackPattern1");
                DestroyAllShadows();
                StartCoroutine("AttackPattern2");
                break;
            case Stage.Stage_2:
                stage = Stage.Stage_3;
                FindObjectOfType<AudioManager>().Play("BossPhase");
                StopCoroutine("AttackPattern2");
                DestroyAllShadows();
                StartCoroutine("AttackPattern3");
                PlatformBreakdown();
                break;
        }
    }
    #endregion

    #region Attack Patterns & IEnumerators

    void PlatformBreakdown()
    {
        middlePlatform.GetComponent<ParticleSystem>().Play();
        middlePlatform.GetComponent<SpriteRenderer>().enabled = false;
        middlePlatform.GetComponent<BoxCollider2D>().enabled = false;
        if (!weGoBelow)
        {
            FindObjectOfType<AudioManager>().Play("WeGoBelow");
            weGoBelow = true;
        }
        
    }

    void PlatformSetBack()
    {
        middlePlatform.GetComponent<SpriteRenderer>().enabled = true;
        middlePlatform.GetComponent<BoxCollider2D>().enabled = true;
    }

    IEnumerator AttackPattern1() 
    {
        while (true)
        {
            RandomMeteorShower();
            yield return meteorDelay;
            RandomMeteorShower();
            yield return meteorDelay;
            RandomMeteorShower();
            yield return meteorDelay;
            RandomMeteorShower();
            yield return meteorDelay;
            RandomMeteorShower();
            yield return meteorDelay;
            RandomMeteorShower();
            yield return Recharge;
        }
    }

    IEnumerator AttackPattern2()
    {
        while (true)
        {
            Projectile();
            yield return attackDelay;
            for (int i = 0; i <= 6; i++)
            {
                WallMeteorShower();
            }
            yield return Recharge;
        }
    }

    IEnumerator AttackPattern3()
    {
        while (true)
        {
            Projectile();
            Projectile();
            yield return attackDelay;
            RandomMeteorShower();
            yield return meteorDelay;
            RandomMeteorShower();
            yield return meteorDelay;
            RandomMeteorShower();
            yield return meteorDelay;
            RandomMeteorShower();
            yield return Recharge;
        }
    }
    #endregion

    #region Basic Attacks

    void RandomMeteorShower()
    {
        var randomX = Random.Range(spawnBox.bounds.min.x, spawnBox.bounds.max.x);
        GameObject meteor = ObjectPooling.SharedInstance.GetPooledObject();
        if (meteor != null)
        {
            meteor.transform.position = new Vector3(randomX, spawnBox.bounds.min.y);
            meteor.transform.rotation = Quaternion.identity;
            meteor.SetActive(true);
        }
    }

    private void WallMeteorShower()
    {
        int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        GameObject meteor = ObjectPooling.SharedInstance.GetPooledObject();
        if (meteor != null)
        {
            meteor.transform.position = spawnPoints[randomSpawnPoint].position;
            meteor.transform.rotation = Quaternion.identity;
            meteor.SetActive(true);
        }
    }

    private void Projectile()
    {
        int randomSpawn = Random.Range(0, projectileSpawns.Length);
        projectile = Instantiate(projectilePrefab, projectileSpawns[randomSpawn].position, Quaternion.identity);
        projectileList.Add(projectile);
        Destroy(projectile, 5f);
    }
    #endregion

    #region Player Manager
    private void DestroyAllShadows()
    {
        ObjectPooling.SharedInstance.SetAllInactive();
        foreach (GameObject g in projectileList)
        { 
            if (g != null) Destroy(g);
        }
    }

    public void DamageDarkness()
    {
        bossHealth.Damage(1);
    }

    public void OnDyingAnimationFinished()
    {
        isTheBattleOver = true;
        FindObjectOfType<ChapterChanger>().FadeToNextChapter();
    }

    public void OnIntroAnimationFinished()
    {
        animator.SetTrigger("RealForm");
        stage = Stage.Stage_1;
        StartCoroutine("AttackPattern1");
        particles.Play();
    }

    void ArenaResetOnDeath()
    { 
        boxCollider2D.SetActive(false);    
        bossHealth.ResetHealth();
    }

    void ArenaResetOnRespawn()
    {
        if (inTheFight)
        {
            particles.Stop();
            stage = Stage.MoonPresence;
            inTheFight = false;
            animator.SetTrigger("PlayerDied");
            StopAllCoroutines();
            DestroyAllShadows();
            bossTrigger.OnPlayerEnterTrigger += Boss_PlayerEnter;
            FindObjectOfType<AudioManager>().Stop("BossTheme");
            FindObjectOfType<AudioManager>().FadeIn("TheNightmare");
            PlatformSetBack();
            weGoBelow = false;
        }        
    }
    #endregion
}