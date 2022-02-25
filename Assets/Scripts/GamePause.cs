using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject clickAudioObject;
    [SerializeField] GameObject hoverAudioObject;
    [SerializeField] GameObject AudioObject;
    private AudioSource clickAudio;
    private AudioSource hoverAudio;
    private bool isPaused;

    private void Awake()
    {
        isPaused = false;
        AudioObject = GameObject.FindGameObjectsWithTag("Audio")[0];
        clickAudioObject = AudioObject.transform.GetChild(0).gameObject;
        hoverAudioObject = AudioObject.transform.GetChild(1).gameObject;
        clickAudio = clickAudioObject.GetComponent<AudioSource>();
        hoverAudio = hoverAudioObject.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(AudioObject);
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            clickAudio.Play();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused) 
        {
            Resume();
        }
    }

    public void Resume()
    {
        clickAudio.Play();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void MainMenu()
    {
        clickAudio.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void onHover() 
    {
        hoverAudio.Play();
    }
}
