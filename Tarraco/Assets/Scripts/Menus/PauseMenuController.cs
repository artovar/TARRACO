using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject background;

    //GameObject Taco;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        pauseMenu.SetActive(false);
        background.SetActive(false);
        //Taco = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Start") && (Time.timeScale == 1 || pauseMenu.activeSelf))
        {
            Pause();
        }

    }


    public void Pause()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            background.SetActive(true);
            EventSystem eventSystem = EventSystem.current;
            Button[] select = pauseMenu.GetComponentsInChildren<Button>();
            eventSystem.SetSelectedGameObject(select[0].gameObject);
            Time.timeScale = 0f;

        }
        else
        {
            pauseMenu.SetActive(false);
            background.SetActive(false);
            Time.timeScale = 1f;
            //Taco.GetComponent<PlayerController>().enabled = true;
        }
    }

    public void Restart()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameController.Instance.ResetStats();
    }
    
    public void NextLevel()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameController.Instance.NextLevel());
        GameController.Instance.ResetStats();
    }

    public void BackToHub()
    {
        StopAllCoroutines();
        Pause();
        int scene = GameController.Instance.BackToHubIndex();
        if (scene < 0) 
        {
            return;
        }
        SceneManager.LoadScene(scene);
        GameController.Instance.ResetStats();
    }

    public void BackToMenu()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
        GameController.Instance.Exit();
        SceneManager.LoadScene(0);
    }
}
