using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.ComponentModel;
using System;

public class IntroScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text dialogueText;
    public TMP_Text failText;
    public TMP_Text nameText;
    public GameObject canvas;
    public GameObject flashPanel;
    public ExitWarpEvent exitWarpEvent;
    public Camera warpCamera;
    public float typeSpeed = 0.05f;
    public bool autoPlay = false;
    

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
        // Turned this into variables so we can make bulk edits quicker
        string shipAI = "NAV PC";
        string control = "SHIP CORE";

        // Show Orbit exterior
        SetCharacterName(nameText, control);
        ClearDialogue(dialogueText);
        yield return TypeDialogue(dialogueText, "We're approaching the target system now.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        yield return TypeDialogue(dialogueText, "Prepare for decceleration.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);


        SetCharacterName(nameText, shipAI);
        yield return TypeDialogue(dialogueText, "WARNING, EXPERIENCING UNEXPECTED GRAVITATIONAL FORCES");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);
        

        yield return new WaitForSeconds(0.5f);
        yield return TypeDialogue(dialogueText, "Exiting Interstellar Warp.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);
        canvas.SetActive(false);     

        yield return new WaitForSeconds(0.5f);

        // Screen flashes to white before fading back in
        flashPanel.SetActive(true);
        yield return new WaitForSeconds(0.75f); // Delay to hide pop in of new objects
        canvas.SetActive(false);
        ExitWarp();
        warpCamera.GetComponent<Animation>().Play(); // Move camera to see star better
        yield return new WaitForSeconds(2.0f);
        // We see the sun for the first time and it is close

        canvas.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        SetCharacterName(nameText, control);
        yield return TypeDialogue(dialogueText, "WARNING, EXTREME HEAT EXPOSURE IMMINENT");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        SetCharacterName(nameText, shipAI);
        yield return TypeDialogue(dialogueText, "That is distinctly not our destination. We need to leave immediately.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        // Switch to first person camera inside the cockpit
        // Use a screen flash again, maybe different colour, to mask loading of cockpit group and unloading ship mesh

        yield return TypeDialogue(dialogueText, "Prepare for an emergency jump.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        SetCharacterName(nameText, control);
        yield return TypeDialogue(dialogueText, "T-MINUS 20 SECONDS UNTIL IRRECOVERABLE SYSTEM DAMAGE");
        yield return WaitForPlayerInput();
        yield return new WaitForSeconds(0.5f);
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

    IEnumerator WaitForPlayerInput()
    {
        while (!autoPlay)
    {
        bool spacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool mouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (spacePressed || mouseClicked)
        {
            break;
        }

        yield return null;
    }

    }

    void ClearDialogue(TMP_Text textObject)
    {
        textObject.text = "";
    }

    void SetCharacterName(TMP_Text nameText, string characterName)
    {
        nameText.text = characterName;
    }

    IEnumerator TypeDialogue(TMP_Text textObject, string text)
    {
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

    public void ExitWarp()
    {
        // Stop warp VFX
        // Stop ship animation
        // Enable star
        // Move camera so star is visible over ship's shoulder
        exitWarpEvent.TriggerEvent();
        
    }
}
