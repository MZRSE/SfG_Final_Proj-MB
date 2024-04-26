using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private TextMeshProUGUI _highScoreTextView;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreTextView.text = highScore.ToString();

        if(_menuMusic != null)
        {
            AudioManager.Instance.PlayMusic(_menuMusic);
        }
    }

    public void ResetScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreTextView.text = highScore.ToString();
    }
}
