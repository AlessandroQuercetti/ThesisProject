using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log(Display.displays.Length);
        // activate additional displays
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();

            Debug.Log("Activated display: " + i);
        }
    }
}