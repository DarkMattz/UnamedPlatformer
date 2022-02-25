using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    private GameObject clickAudioObject;
    private GameObject hoverAudioObject;
    private GameObject AudioObject;
    private AudioSource clickAudio;
    private AudioSource hoverAudio;

    private void Awake()
    {
        AudioObject = GameObject.FindGameObjectsWithTag("Audio")[0];
        clickAudioObject = AudioObject.transform.GetChild(0).gameObject;
        hoverAudioObject = AudioObject.transform.GetChild(1).gameObject;
        clickAudio = clickAudioObject.GetComponent<AudioSource>();
        hoverAudio = hoverAudioObject.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void Restart() 
    {
        clickAudio.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
