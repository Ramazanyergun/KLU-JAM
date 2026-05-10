using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio; // En üste ekle
using UnityEngine.UI;    // Sliderlar için ekle 
public class SoundManager : MonoBehaviour
{
[SerializeField] AudioSource musicSource;
[SerializeField] AudioSource SFXSource;
[SerializeField] private AudioMixer mainMixer;
public AudioClip button;
public AudioClip backgroundmusic;
public AudioClip attack;
public AudioClip bam;
public AudioClip blockedbyjames;
public AudioClip bocukattack;
public AudioClip bocukdeath;
public AudioClip fuf;
public AudioClip gamemenumusic;
public AudioClip goblindeath;
public AudioClip shield;
public AudioClip takedamage;
public AudioClip warriordeath;
public AudioClip witchdeath;
   public static SoundManager Instance;

  private void Awake()
{
    // Singleton yapısı
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // --- BAĞLANTIYI BURADA KODLA YAPIYORUZ ---
        if (mainMixer != null)
        {
            // 1. Mixer'ın içindeki "Music" isimli grubu bul
            var musicGroups = mainMixer.FindMatchingGroups("Music");
            if (musicGroups.Length > 0) 
            {
                // 2. Bulduğun grubu musicSource'un OUTPUT kısmına ata
                musicSource.outputAudioMixerGroup = musicGroups[0];
            }

            // 3. Mixer'ın içindeki "SFX" isimli grubu bul
            var sfxGroups = mainMixer.FindMatchingGroups("SFX");
            if (sfxGroups.Length > 0) 
            {
                // 4. Bulduğun grubu SFXSource'un OUTPUT kısmına ata
                SFXSource.outputAudioMixerGroup = sfxGroups[0];
            }
        }
    }
    else
    {
        Destroy(gameObject);
    }
}
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Her sahne yüklendiğinde çalışır
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) // Ana Menü (Build Settings'deki index)
        {
            PlayMusic(gamemenumusic);
        }
        else // Oyun Sahnesi
        {
            PlayMusic(backgroundmusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return; // Zaten bu müzik çalıyorsa baştan başlatma

        musicSource.clip = clip;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void PlayDeathSound(string tag)
    {
        switch (tag)
        {
            case "Goblin": PlaySFX(goblindeath); break;
            case "Fare": PlaySFX(bocukdeath); break;
            case "Cadi": PlaySFX(witchdeath); break;
        }
    }
  

    // Music Slider için fonksiyon
    public void SetMusicVolume(float value)
    {
        Debug.Log("Müzik Slider Değeri: " + value); // Console'da bunu görüyor musun?
    
    if (mainMixer != null)
    {
        bool result = mainMixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
        Debug.Log("Mixer Parametresi Değişti mi?: " + result); 
    }
    }

    // SFX Slider için fonksiyon
    public void SetSFXVolume(float value)
    {
        mainMixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
    }
}
