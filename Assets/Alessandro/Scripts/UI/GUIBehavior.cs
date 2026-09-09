using UnityEngine;

public class GUIBehavior : MonoBehaviour
{

    /*public GameObject menu; 

    public void ToggleMenu()
    {
        if (menu != null)
        {
            menu.SetActive(!menu.activeSelf);
        }
    }   */

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else          
        Application.Quit();
#endif
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
