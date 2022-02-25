using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject audioObject;
    [SerializeField] private GameObject bgmObject;
    private GameObject clickAudioObject;
    private GameObject hoverAudioObject;
    private AudioSource clickAudio;
    private AudioSource hoverAudio;
    private AudioSource bgm;
    private LevelSave levelSave;
    private AudioLevel audioSave;


    private void Start()
    {

        //save level
        string path = Application.persistentDataPath + "/levelSave.snort";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            levelSave = formatter.Deserialize(stream) as LevelSave;
            stream.Close();
        }
        else 
        {
            levelSave = null;
        }

        //save volume
        path = Application.persistentDataPath + "/soundSave.snort";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            audioSave = AudioLevel.getInstance();
            audioSave = formatter.Deserialize(stream) as AudioLevel;
            AudioLevel audioLevel = AudioLevel.getInstance();
            audioLevel.setBgmLevel(audioSave.getBgmLevel());
            audioLevel.setSfxLevel(audioSave.getSfxLevel());
            stream.Close();
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            audioSave = AudioLevel.getInstance();
            audioSave.setBgmLevel(1);
            audioSave.setSfxLevel(0.2F);

            formatter.Serialize(stream, audioSave);
            stream.Close();
        }


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        audioObject.SetActive(true);
        clickAudioObject = audioObject.transform.GetChild(0).gameObject;
        hoverAudioObject = audioObject.transform.GetChild(1).gameObject;
        clickAudio = clickAudioObject.GetComponent<AudioSource>();
        hoverAudio = hoverAudioObject.GetComponent<AudioSource>();
        bgm = bgmObject.GetComponent<AudioSource>();
        bgm.volume = audioSave.getBgmLevel();
    }

    public void StartGame()
    {
        clickAudio.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame() 
    {
        if (levelSave != null)
        {
            clickAudio.Play();
            SceneManager.LoadScene(levelSave.getLevel());
        }
    }

    public void Settings()
    {
        clickAudio.Play();
        mainCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void QuitGame()
    {

        clickAudio.Play();
        Application.Quit();
    }

    public void onHover()
    {
        hoverAudio.Play();
    }

    public void onHoverContinue() 
    {
        if (levelSave != null) 
        { 
            hoverAudio.Play();
        }
    }
}

