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
        }
        catch (System.NullReferenceException)
        {

        }
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