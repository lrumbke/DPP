using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI BestScore;
    public Button StartButton;
    public Button ExitButton;
    public TMP_InputField NameInput;

    [SerializeField]

    void Start()
    {
        DataStorage.getInstance().Load();
        BestScore.text = DataStorage.getInstance().GetBestScoreText();

        NameInput.onValueChanged.AddListener(OnNameInputChanged);
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        DataStorage.getInstance().Save();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }

    private void OnNameInputChanged(string newText)
    {
        DataStorage.getInstance().Username = newText;
        BestScore.text = DataStorage.getInstance().GetBestScoreText();
    }
}
