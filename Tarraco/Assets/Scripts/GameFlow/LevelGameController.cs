using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGameController : GameController
{
    [SerializeField]
    private OvationBar mainBar;

    [SerializeField]
    private int baseHubIndex;
    [SerializeField]
    private int midHubIndex;
    [SerializeField]
    private int level1Index;
    [SerializeField]
    private int totalLevels;
    [SerializeField]
    private int creditsIndex;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip musicHub;
    [SerializeField]
    private AudioClip[] musicLevels;
    [SerializeField]
    private AudioClip musicBoss;
    [SerializeField]
    public SpawnPoint spawn;

    private HubScreen screen;

    [HideInInspector]
    public int currentLevel = 0;
    protected override void AdditionalStarto()
    {
        mainBar.transform.parent.gameObject.SetActive(true);
        mainBar.gameObject.SetActive(true);
        mainBar.EnableBar();
        if(!inGame)
        {
            screen = GameObject.FindGameObjectWithTag("Heal").GetComponent<HubScreen>();
            screen.SetUP();
            screen.DisplayLevel(currentLevel + 1);
        }
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.B))
        {
            SpawnBoss();
        }
    }

    public override int BackToHubIndex()
    {
        StopAllCoroutines();
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index >= level1Index && index <= level1Index + totalLevels - 1)
        {
            currentLevel--;
            PlayMusic(midHubIndex);
            return midHubIndex;
        }
        return -1;
    }

    public override int NextLevel()
    {
        StopAllCoroutines();
        SaveWeapons();
        int index = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = 0;
        if (index == baseHubIndex)
        {
            currentLevel = 1;
            nextLevel = level1Index;
            //return level1Index;
        }
        else if (index == midHubIndex)
        {
            currentLevel++;
            nextLevel = level1Index + currentLevel - 1;
            //return level1Index + currentLevel - 1;
        }
        else if (index >= level1Index && index < level1Index + totalLevels - 1)
        {
            nextLevel = midHubIndex;
            //return midHubIndex;
        }
        else if (index == level1Index + totalLevels - 1)
        {
            nextLevel = -creditsIndex;
        }
        PlayMusic(nextLevel);
        return nextLevel;
    }

    private void PlayMusic(int nextLevel)
    {
        if (nextLevel == midHubIndex)
        {
            audioSource.clip = musicHub;
        }
        else if (nextLevel >= level1Index && nextLevel < level1Index + totalLevels - 1)
        {
            audioSource.clip = musicLevels[currentLevel - 1];
        }
        audioSource.Play();
        audioSource.loop = true;
    }

    public override void ResetMusic()
    {
        StartCoroutine(Music());
        IEnumerator Music()
        {
            yield return new WaitForSeconds(.1f);
            PlayMusic(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void SpawnBoss()
    {
        audioSource.clip = musicBoss;
        audioSource.Play();
        audioSource.loop = true;
        cam.GetComponent<CameraControl>().ChangeToBoss(spawn.SpawnBoss(currentLevel));
    }

    public int GetTotalLevels()
    {
        return totalLevels;
    }
}
