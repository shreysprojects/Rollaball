using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ResetLevel);
    }

    void ResetLevel()
    {
        // Reset time scale before reloading the scene
        Time.timeScale = 1;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}