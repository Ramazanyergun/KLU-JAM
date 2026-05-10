using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        // Pause açılma sesi
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.button);
    }

    public void Home()
    {
        Time.timeScale = 1; // Sahne yüklenmeden ÖNCE zamanı başlatmak daha güvenlidir
        
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.button);

        // ÖNEMLİ: Sahne isminin "Main menu" olduğundan emin ol (Büyük/küçük harf duyarlıdır)
        SceneManager.LoadScene("Main menu");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.button);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.button);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}