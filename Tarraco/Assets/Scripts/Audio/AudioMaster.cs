using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMaster : MonoBehaviour
{
    public AudioMixer mixer;

    private bool ready;


    float musicVolume;
    float effectsVolume;
    float peopleVolume;
    float masterVolume;

    public Slider musicSlider;
    public Slider effectsSlider;
    public Slider peopleSlider;
    public Slider masterSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        //SoundValuesSingleton.musicValue
        musicVolume = AudioValuesSingleton.Instance.musicVolume;
        mixer.SetFloat("MusicVol", Mathf.Log10(musicVolume) * 20);
        musicSlider.value = musicVolume;

        //SoundValuesSingleton.effectsValue
        effectsVolume = AudioValuesSingleton.Instance.effectsVolume;
        mixer.SetFloat("EffectsVol", Mathf.Log10(effectsVolume) * 20);
        effectsSlider.value = effectsVolume;

        //SoundValuesSingleton.peopleValue
        peopleVolume = AudioValuesSingleton.Instance.peopleVolume;
        mixer.SetFloat("PeopleVol", Mathf.Log10(peopleVolume) * 20);
        peopleSlider.value = peopleVolume;

        //SoundValuesSingleton.masterValue
        masterVolume = AudioValuesSingleton.Instance.masterVolume;
        mixer.SetFloat("MasterVol", Mathf.Log10(masterVolume) * 20);
        masterSlider.value = masterVolume;

        ready = true;
    }
    public void SetMusicVol()
    {
        if (!ready) return;
        mixer.SetFloat("MusicVol", Mathf.Log10(musicSlider.value) * 20);
        musicVolume = musicSlider.value;
        AudioValuesSingleton.Instance.musicVolume = musicVolume;
        //SoundValuesSingleton
    }
    public void SetEffectsVol()
    {
        if (!ready) return;
        mixer.SetFloat("EffectsVol", Mathf.Log10(effectsSlider.value) * 20);
        effectsVolume = effectsSlider.value;
        AudioValuesSingleton.Instance.effectsVolume = effectsVolume;
        //SoundValuesSingleton
    }
    public void SetPeopleVol()
    {
        if (!ready) return;
        mixer.SetFloat("PeopleVol", Mathf.Log10(peopleSlider.value) * 20);
        peopleVolume = peopleSlider.value;
        AudioValuesSingleton.Instance.peopleVolume = peopleVolume;
        //SoundValuesSingleton
    }
    public void SetMasterVol()
    {
        if (!ready) return;
        mixer.SetFloat("MasterVol", Mathf.Log10(masterSlider.value) * 20);
        masterVolume = masterSlider.value;
        AudioValuesSingleton.Instance.masterVolume = masterVolume;
        //SoundValuesSingleton
    }
}
