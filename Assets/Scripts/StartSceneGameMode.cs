using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneGameMode : MonoBehaviour
{
    [SerializeField] private Button playButton;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene(1));
    }
}
