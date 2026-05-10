using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panel Referansları")]
    public GameObject mainMenuPanel;    // Ana butonların olduğu yer
    public GameObject optionsPanel;     // Options tuşuna basınca açılacak ana panel
    SoundManager soundManager;
    void Start()
    {
        // Oyun açıldığında her şeyin yerli yerinde olduğundan emin olalım
        ShowMainMenu();
    }

    public void PlayGame()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.button);
            
        SceneManager.LoadSceneAsync(1);
    }

    public void OpenOptions()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.button);

        mainMenuPanel.SetActive(false); // Ana menüyü gizle
        optionsPanel.SetActive(true);   // Options panelini göster
    }

    public void CloseOptions()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.button);

        ShowMainMenu(); // Ana menüye geri dön
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
         if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.button);
        Application.Quit();
    }
}