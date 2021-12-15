using UnityEngine;
public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject background;

    GameObject Taco;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        background.SetActive(false);
        Taco = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetButtonDown("Start"))
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
            Time.timeScale = 0f;
            Taco.GetComponent<PlayerController>().enabled = false;

        }
        else
        {
            pauseMenu.SetActive(false);
            background.SetActive(false);
            Time.timeScale = 1f;
            Taco.GetComponent<PlayerController>().enabled = true;
        }
    }

}
