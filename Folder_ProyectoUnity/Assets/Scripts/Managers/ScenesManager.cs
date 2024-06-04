using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private EventManagerData eventManagerData;
    private void OnEnable()
    {
        eventManagerData._EventManager.Victory += GoToMenu;
    }
    private void OnDisable()
    {
        eventManagerData._EventManager.Victory -= GoToMenu;
    }
    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public string GetCurrentScene()
    {
        string CurrentString = SceneManager.GetActiveScene().name;
        return CurrentString;
    }
}