using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private EventManagerData eventManagerData;
    private void OnEnable()
    {
        try
        {
            eventManagerData._EventManager.Victory += GoToMenu;
            eventManagerData._EventManager.Defeat += GoToMenu;
        }
        catch (System.NullReferenceException)
        {

        }
    }
    private void OnDisable()
    {
        try
        {
            eventManagerData._EventManager.Victory -= GoToMenu;
            eventManagerData._EventManager.Defeat -= GoToMenu;
        }
        catch (System.NullReferenceException)
        {

        }
    }
    public void GoToScene(string scene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }
    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public string GetCurrentScene()
    {
        string CurrentString = SceneManager.GetActiveScene().name;
        return CurrentString;
    }
}