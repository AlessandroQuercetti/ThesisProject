using CrazyMinnow.SALSA;
using UnityEngine;

public class EmoteRManager : MonoBehaviour
{

    public Emoter emoter;


    void Start()
    {
        if (emoter == null)
        {
            Debug.LogError("EmoteRManager - Emoter reference is not set.");
            return;
        }
        //StartCoroutine(RandomExpressions());
    }

    public void TriggerEmote(string emoteName)
    {
        Debug.Log("EmoteRManager - Triggering: " + emoteName);
        emoter.ManualEmote(emoteName, ExpressionComponent.ExpressionHandler.RoundTrip);
    }

    /*
    private System.Collections.IEnumerator RandomExpressions()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(10f, 20f));

            float random = Random.value;

            if (random < 0.7f)
            {
                //TriggerEmote("smile");
            }
            else if (random < 0.9f)
            {
                //TriggerEmote();
            }
            else
            {
                //TriggerEmote();
            }
        }
    }*/
}
