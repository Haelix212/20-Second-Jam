using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text dialogueText;
    public TMP_Text failText;

    public float typeSpeed = 0.05f;

    private void Start()
    {
        if (dialogueText != null)
        {
            PlayIntroSequence();
        }
        
        if (failText != null)
        {
            PlayFailSequence();
        }
    }

    IEnumerator PlayIntroSequenceCoroutine()
    {
        yield return TypeDialogue(dialogueText, "Captain?");
        yield return new WaitForSeconds(1.2f);
        yield return TypeDialogue(dialogueText, "Do you read me?");
        yield return new WaitForSeconds(1.2f);
        yield return TypeDialogue(dialogueText, "We made a slight miscalculation in our jump calculations--");
        yield return new WaitForSeconds(1.2f);
        yield return TypeDialogue(dialogueText, "We need to--");
        yield return new WaitForSeconds(0.8f);
        yield return TypeDialogue(dialogueText, "Captain we aren't going to make it you need to--");
        yield return new WaitForSeconds(0.8f);
        yield return TypeDialogue(dialogueText, "JUMP ACTIVE");
        yield return new WaitForSeconds(1.2f);
        dialogueText.gameObject.SetActive(false);
        GameManager.Instance.StartGame();

    }

    IEnumerator PlayFailSequenceCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        yield return TypeDialogue(failText, "Play again...?");
        yield return new WaitForSeconds(2.0f);
        GameObject againButton = this.gameObject.transform.Find("PlayAgainButton").gameObject;
        againButton.SetActive(true);
        GameObject quitButton = this.gameObject.transform.Find("QuitButton").gameObject;
        quitButton.SetActive(true);
    }

    IEnumerator TypeDialogue(TMP_Text textObject, string text)
    {
        textObject.text = "";
        foreach (char c in text)
        {
            textObject.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public void PlayIntroSequence()
    {
        // Play the intro animation or sequence here
        // Lock player controls during the intro
        // Start space background moving the player around
        // Hyperspace jump effect
        dialogueText.text = "";
        StartCoroutine(PlayIntroSequenceCoroutine());
        // particle system for the jump + moving camera to feel like the player is moving through space
        // Abrubt halt of the music and background movement to simulate the jump
        // Close shields and turn on the alarm lights and sounds
        // Start the timer
        // Unlock player controls after the intro

    }

    public void PlayFailSequence()
    {
        failText.text = "";
        StartCoroutine(PlayFailSequenceCoroutine());
        // Wait for player to pick yes or no, either of which will change scene
    }
}
