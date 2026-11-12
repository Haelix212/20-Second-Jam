using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text dialogueText;

    public float typeSpeed = 0.05f;

    private void Start()
    {
        PlayIntroSequence();
    }

    IEnumerator PlayIntroSequenceCoroutine()
    {
        yield return TypeDialogue("Captain?");
        yield return new WaitForSeconds(1.2f);
        yield return TypeDialogue("Do you read me?");
        yield return new WaitForSeconds(1.2f);
        yield return TypeDialogue("We made a slight miscalculation in our jump calculations--");
        yield return new WaitForSeconds(1.2f);
        yield return TypeDialogue("We need to--");
        yield return new WaitForSeconds(0.8f);
        yield return TypeDialogue("Captain we aren't going to make it you need to--");
        yield return new WaitForSeconds(0.8f);
        yield return TypeDialogue("JUMP ACTIVE");
        yield return new WaitForSeconds(1.2f);
        dialogueText.gameObject.SetActive(false);
        GameManager.Instance.StartGame();

    }

    IEnumerator TypeDialogue(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
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
}
