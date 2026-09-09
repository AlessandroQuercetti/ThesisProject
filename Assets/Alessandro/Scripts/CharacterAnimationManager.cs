using Meta.WitAi.Json;
using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour
{
    public Animator characterAnimator;

    [Header("Idle Animation Settings")]
    public float idleMinTime = 8f;
    public float idleMaxTime = 18f;

    [Header("Talking Animation Settings")]
    public float talkingAnimationChance = 0.8f;

    [Header("Expressions")]
    public EmoteRManager emoterManager;

    private bool isTalking;

    private Coroutine talkingRoutine;


    // Expressions to look for in the Wit.ai response to trigger confirming or rejecting animations
    private string[] confirmExpressions =
    {
        "yes",
        "yeah",
        "yep",
        "correct",
        "exact",
        "good",
        "nice",
        "great",
        "perfect",
        "excellent", 
        "okay"
    };

    // Expressions to look for in the Wit.ai response to trigger rejecting animations
    private string[] rejectExpressions =
    {
        "no",
        "nope",
        "wrong",
        "incorrect",
        "again",
        "retry",
        "different",
        "not"
    };

    private string[] postureShiftExpressions = //a lot of them will be not used 
    {
        "anyway",
        "anyways",
        "so",
        "well",
        "moving on",
        "by the way",
        "actually",
        "basically",
        "you know",
        "to be honest",
        "honestly",
        "listen",
        "for example",
        "for instance",
        "i mean",
        "what i mean is"
    };

    void Start()
    {
        isTalking = false;
        StartCoroutine(IdleVariationLoop());
    }

    // =====================================================
    // WIT RESPONSE ANIMATIONS
    // =====================================================

    //only because intents are not being recognized , using this function to trigger animations based on the raw response from Wit.ai
    //get a response from Wit.ai and trigger the appropriate animation based on the direction entity in the response
    public void OnResponse(WitResponseNode response)
    {
        var entities = response["entities"];

        if (entities["direction:direction"] != null)
        {
            string dir = entities["direction:direction"][0]["value"];

            if (dir == "left")
            {
                emoterManager.TriggerEmote("Confirm");
                Debug.Log("CharacterAnimationManager - TRIGGERING ANIMATION: pointingLeft");
                characterAnimator.SetTrigger("pointingLeft");


            } 
            else if (dir == "right")
            {
                emoterManager.TriggerEmote("Confirm");
                Debug.Log("CharacterAnimationManager - TRIGGERING ANIMATION: pointingRight");
                characterAnimator.SetTrigger("pointingRight");
            }
        }

        //since we dont have other intents, we have to check if the response contains some words and trigger animations based on that
        string text = response["text"].ToString().ToLower().Trim();
        text = text.Replace("'", "");
        text = text.Trim('"');
        string[] words = text.Split(' ');

        Debug.Log("TEXT = " + text);

        foreach (var word in words)
        {
            Debug.Log("WORD = [" + word + "]");
        }

        //confirm expressions
        foreach (var expression in confirmExpressions)
        {
            //if (text.Contains(expression))
            if (words.Contains(expression))
            {
                emoterManager.TriggerEmote("Confirm");
                Debug.Log("CharacterAnimationManager - TRIGGERING ANIMATION: confirming");
                characterAnimator.SetTrigger("confirming");
                break;
            }
        }

        //reject expressions
        foreach (var expression in rejectExpressions)
        {
            //if (text.Contains(expression))
            if (words.Contains(expression))
            {
                emoterManager.TriggerEmote("Reject");
                Debug.Log("CharacterAnimationManager - TRIGGERING ANIMATION: rejecting");
                characterAnimator.SetTrigger("rejecting");
                break;
            }
        }

        //posture shifts expresions 
        foreach (var expression in postureShiftExpressions)
        {
            //if (text.Contains(expression))
            if (words.Contains(expression))
            {
                PostureShift();
                break;
            }
        }
    }

    // =====================================================
    // IDLE VARIATIONS
    // =====================================================

    //Routine for idle animation
    private IEnumerator IdleVariationLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(idleMinTime, idleMaxTime);

            yield return new WaitForSeconds(waitTime);

            if (isTalking) // if the avatar is talking, we dont want to trigger idle animations
                continue;

            int random = Random.Range(0, 5); //min inclusive, max exclusive

            switch (random)
            {
                // 0 and 1 trigger the first animation
                case 0:
                case 1:
                    Debug.Log("CharacterAnimationManager - TRIGGERING ANIMATION: movingALittleBit");
                    characterAnimator.SetTrigger("movingALittleBit");
                    break;

                // 2 the second one
                case 2:
                    Debug.Log("CharacterAnimationManager - TRIGGERING ANIMATION: nodding");
                    characterAnimator.SetTrigger("nodding");
                    emoterManager.TriggerEmote("Wide Eyes");
                    break;

                //the last two numbers lead to no animation
                default:
                    Debug.Log("CharacterAnimationManager - nothing to trigger");
                    break;
            }

        }
    }

    // =====================================================
    // TALKING ANIMATIONS
    // =====================================================

    private IEnumerator TalkingAnimations()
    {
        while (isTalking)
        {
            yield return new WaitForSeconds(
                Random.Range(3f, 6f));

            float random = Random.value;

            if (random < 0.6f)
            {
                Debug.Log("CharacterAnimationManager - TRIGGERING ANIMATION: SittingTalking (gesturing)");
                characterAnimator.SetTrigger("gesturing");
            }
        }
    }

    public void StartTalking()
    {
        if(isTalking)
            return;

        isTalking = true;
        Debug.Log("CharacterAnimationManager - SETTING ANIMATION: Talking Hand Pose -> true");
        characterAnimator.SetBool("talking", true);

        talkingRoutine = StartCoroutine(TalkingAnimations());
    }

    public void StopTalking()
    {
        isTalking = false;
        Debug.Log("CharacterAnimationManager - SETTING ANIMATION: Talking Hand Pose -> false");
        characterAnimator.SetBool("talking", false); 

        if (talkingRoutine != null)
            StopCoroutine(talkingRoutine);
    }

    // =====================================================
    // POSTURE SHIFT ANIMATIONS
    // =====================================================

    //Routine for crossing legs animation, we can trigger it from the idle variation loop
    private System.Collections.IEnumerator CrossLegsRoutine(string animationToTrigger)
    {
        Debug.Log("CharacterAnimationManager - TRIGGERING ANIMATION: " + animationToTrigger + " for 4 seconds");

        characterAnimator.SetBool(animationToTrigger, true);
        yield return new WaitForSeconds(4f);
        characterAnimator.SetBool(animationToTrigger, false);
    }

    public void PostureShift()
    {
        int random = Random.Range(0, 3);

        switch(random)
        {
            case 0:
                Debug.Log("CharacterAnimationManager - TRIGGERING ANIMATION: leaningForward");
                characterAnimator.SetTrigger("leaningForward");
                break;
            case 1:
                StartCoroutine(CrossLegsRoutine("crossingLegs"));
                break;
            case 2:
                StartCoroutine(CrossLegsRoutine("stretchingLegs"));
                break;
        }        
    }

    /*
    
    // This function is not being used because the intents are not being recognized, so we are using the raw response from Wit.ai to trigger animations instead

    public void triggerAnimation(string[] input)
    {
        string intent = input[0].ToLower();

        if (intent == "point_direction")
        {
            string direction = input[1].ToLower(); 

            if (direction == "left")
                characterAnimator.SetTrigger("pointingLeft");

            if (direction == "right")
                characterAnimator.SetTrigger("pointingRight");
        }

        if (intent == "confirm")
            characterAnimator.SetTrigger("confirming");

        if (intent == "reject")
            characterAnimator.SetTrigger("rejecting");
    }
    */

}