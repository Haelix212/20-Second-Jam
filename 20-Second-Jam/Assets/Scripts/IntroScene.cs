using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.ComponentModel;
using System;
using UnityEngine.AI;

public class IntroScene : MonoBehaviour
{
    [Header("Intro Variables")]
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public GameObject canvas;
    public ExitWarpEvent exitWarpEvent;

    [Header("Fail Variables")]
    public TMP_Text failText;

    [Header("Outro Variables")]
    public GameObject shipModel;
    public GameObject starFlash;
    private GameObject flash;

    [Header("General Variables")]
    public GameObject flashPanel;
    public Camera warpCamera;
    public float typeSpeed = 0.05f;
    public bool autoPlay = false;
    [Header("Dialogue Audio")]
    public AudioSource dialogueAudioSource;
    
    

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

        if (shipModel != null)
        {
            PlayOutroSequence();
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
        yield return TypeDialogue(dialogueText, "We're approaching the target system now.", "ShipPC1.1");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        yield return TypeDialogue(dialogueText, "Prepare for decceleration.", "ShipPC1.2");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);


        SetCharacterName(nameText, shipAI);
        yield return TypeDialogue(dialogueText, "WARNING, EXPERIENCING UNEXPECTED GRAVITATIONAL FORCES", "Core1");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);
        

        yield return new WaitForSeconds(0.5f);
        yield return TypeDialogue(dialogueText, "Exiting Interstellar Warp.", "ShipPC2");
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
        yield return TypeDialogue(dialogueText, "WARNING, EXTREME HEAT EXPOSURE IMMINENT", "Core2");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        SetCharacterName(nameText, shipAI);
        yield return TypeDialogue(dialogueText, "That is distinctly not our destination. We need to leave immediately.", "ShipPC3");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        // Switch to first person camera inside the cockpit
        // Use a screen flash again, maybe different colour, to mask loading of cockpit group and unloading ship mesh

        yield return TypeDialogue(dialogueText, "Prepare for an emergency jump.", "ShipPC3.1");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        SetCharacterName(nameText, control);
        yield return TypeDialogue(dialogueText, "T-MINUS 20 SECONDS UNTIL IRRECOVERABLE SYSTEM DAMAGE", "Core3");
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

    IEnumerator PlayOutroSequenceCoroutine()
    {
        // Ship movement and camera pan animations trigger on scene start
        ParticleSystem shockwave = shipModel.transform.Find("ShockwaveVFX").GetComponent<ParticleSystem>();
        GameObject engine = shipModel.transform.Find("EngineVFX").gameObject;
        yield return new WaitForSeconds(3.25f);
        if (engine != null) engine.SetActive(true);
        if (shockwave != null) shockwave.Play();

        yield return new WaitForSeconds(0.5f);
        if (starFlash != null) starFlash.SetActive(true);
        MeshRenderer[] shipPieces = shipModel.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer piece in shipPieces)
        {
            piece.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
        shipModel.SetActive(false);

        yield return new WaitForSeconds(2f);
        if (starFlash != null) starFlash.SetActive(false);
        Transform blackScreen = this.transform.Find("BlackScreen");
        if (blackScreen != null) blackScreen.gameObject.SetActive(true);
    
        yield return new WaitForSeconds(5f);
        Transform again = this.transform.Find("PlayAgainButton");
        if (again != null) 
        {
            Button againButton = again.GetComponent<Button>();
            againButton.onClick.RemoveAllListeners();
            againButton.onClick.AddListener(delegate { GameManager.Instance.RestartGame(); } );
            againButton.gameObject.SetActive(true);
        }
        Transform quit = this.transform.Find("QuitButton");
        if (quit != null) 
        {
            Button quitButton = quit.GetComponent<Button>();
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(delegate { GameManager.Instance.ReturnMainMenu(); } );
            quitButton.gameObject.SetActive(true);
        }
        
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

    IEnumerator TypeDialogue(TMP_Text textObject, string text, string audioFileName = null)
    {
        textObject.text = "";

        if (!string.IsNullOrEmpty(audioFileName))
        {
            AudioClip clip = Resources.Load<AudioClip>(
                "AudioFiles/" + audioFileName
            );

            if (clip != null && dialogueAudioSource != null)
            {
                dialogueAudioSource.Stop();
                dialogueAudioSource.clip = clip;
                dialogueAudioSource.Play();
            }
            else if (clip == null)
            {
                Debug.LogWarning("This was not found: " + audioFileName);
            }
        }

        foreach (char c in text)
        {
            textObject.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        // Wait until audio finishes
        if (dialogueAudioSource != null &&
            dialogueAudioSource.isPlaying)
        {
            while (dialogueAudioSource.isPlaying)
            {
                yield return null;
            }
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

    public void PlayOutroSequence()
    {
        StartCoroutine(PlayOutroSequenceCoroutine());
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
