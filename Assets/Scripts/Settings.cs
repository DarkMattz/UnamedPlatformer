using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject bgmSliderObject;
    [SerializeField] private GameObject sfxSliderObject;
    [SerializeField] private GameObject uiAudioObject;
    [SerializeField] private GameObject bgmObject;
    private AudioSource clickAudio;
    private AudioSource hoverAudio;
    private AudioSource bgm;
    private AudioLevel audioLevel;
    private Slider bgmSlider;
    private Slider sfxSlider;

    private void Awake()
    {
        clickAudio = uiAudioObject.transform.GetChild(0).GetComponent<AudioSource>();
        hoverAudio = uiAudioObject.transform.GetChild(1).GetComponent<AudioSource>();
        bgm = bgmObject.GetComponent<AudioSource>();
        audioLevel = AudioLevel.getInstance();
        bgmSlider = bgmSliderObject.GetComponent<Slider>();
        sfxSlider = sfxSliderObject.GetComponent<Slider>();
    }

    private void Start()
    {
        bgmSlider.value = audioLevel.getBgmLevel();
        sfxSlider.value = audioLevel.getSfxLevel();
    }

    private void Update()
    {
        audioLevel.setBgmLevel(bgmSlider.value);
        bgm.volume = bgmSlider.value;
        audioLevel.setSfxLevel(sfxSlider.value);
        clickAudio.volume = sfxSlider.value;
        hoverAudio.volume = sfxSlider.value;
    }

    public void onHover() 
    {
        hoverAudio.Play();
    }

    public void backButton() 
    {
        clickAudio.Play();
        string path = Application.persistentDataPath + "/soundSave.snort";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, audioLevel);
            stream.Close();
        }
        mainCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

}
