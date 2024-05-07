using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipSceneManager : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private int SceneIndex = 0;
    
    private void OnEnable()
    {
        restartButton.onClick.AddListener((delegate{ ChangeScene(SceneIndex);} ));
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveAllListeners();
    }

    private void ChangeScene(int _scene)
    {
        SceneManager.LoadScene(_scene);
    }
}
