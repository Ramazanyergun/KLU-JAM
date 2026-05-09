using UnityEngine;
using UnityEngine.SceneManagement;
public class SoundManager : MonoBehaviour
{
[SerializeField] AudioSource musicSource;
[SerializeField] AudioSource SFXSource;

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
        // Singleton Yapısı: Sahneler arası geçişte tek bir SoundManager kalmasını sağlar
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
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
}
