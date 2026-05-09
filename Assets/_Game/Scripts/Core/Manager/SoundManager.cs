using UnityEngine;

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


    private void Start()
    {
        musicSource.clip=backgroundmusic;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
