using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartManager : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    private void OnEnable()
    {
        restartButton.onClick.AddListener((delegate{ ChangeScene(0);} ));
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
