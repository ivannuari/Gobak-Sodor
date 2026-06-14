using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    [Header("Buttons")]
    public Button optionButton;   // tombol OPTION di menu utama
    public Button backButton;     // tombol BACK di OptionPanel

    [Header("Panels")]
    public GameObject optionPanel;
    public GameObject mainMenuPanel;   // MENU UTAMA
    public GameObject levelPanel;      // DIFFICULTY

    [Header("Audio UI")]
    public Slider volumeSlider;
    public Toggle muteToggle;

    private bool isChanging = false;

    private void Awake()
    {
        // Matikan option saat awal
        if (optionPanel != null)
            optionPanel.SetActive(false);

        // Load pengaturan audio
        float savedVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        bool savedMuted = PlayerPrefs.GetInt("isMuted", 0) == 1;

        isChanging = true;
        if (volumeSlider != null)
            volumeSlider.value = savedVolume;

        if (muteToggle != null)
            muteToggle.isOn = savedMuted;
        isChanging = false;

        ApplyAudio(savedVolume, savedMuted);

        // ===== TOMBOL OPTION =====
        optionButton.onClick.RemoveAllListeners();
        optionButton.onClick.AddListener(() =>
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(false);

            if (levelPanel != null)
                levelPanel.SetActive(false);

            if (optionPanel != null)
                optionPanel.SetActive(true);
        });

        // ===== TOMBOL BACK =====
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                if (optionPanel != null)
                    optionPanel.SetActive(false);

                if (mainMenuPanel != null)
                    mainMenuPanel.SetActive(true);
            });
        }

        // ===== SLIDER VOLUME =====
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        // ===== TOGGLE MUTE =====
        if (muteToggle != null)
        {
            muteToggle.onValueChanged.RemoveAllListeners();
            muteToggle.onValueChanged.AddListener(OnMuteChanged);
        }
    }

    private void OnVolumeChanged(float value)
    {
        if (isChanging) return;

        bool muted = muteToggle != null && muteToggle.isOn;
        ApplyAudio(value, muted);

        PlayerPrefs.SetFloat("masterVolume", value);
    }

    private void OnMuteChanged(bool muted)
    {
        if (isChanging) return;

        float volume = volumeSlider != null ? volumeSlider.value : 1f;
        ApplyAudio(volume, muted);

        PlayerPrefs.SetInt("isMuted", muted ? 1 : 0);
    }

    private void ApplyAudio(float volume, bool muted)
    {
        AudioListener.volume = muted ? 0f : volume;
    }
}
