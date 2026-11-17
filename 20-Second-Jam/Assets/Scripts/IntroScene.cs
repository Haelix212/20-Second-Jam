using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.ComponentModel;

public class IntroScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text dialogueText;
    public TMP_Text failText;
    public TMP_Text nameText;
    public GameObject canvas;

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
        // Show Orbit exterior
        SetCharacterName(nameText, "CONTROL");
        ClearDialogue(dialogueText);
        yield return TypeDialogue(dialogueText, "SS Frontier, this is Mission Control.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        yield return TypeDialogue(dialogueText, "You are clear to jump to FTL.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);



        SetCharacterName(nameText, "SHIP AI");

        yield return TypeDialogue(dialogueText, "Rodger that, Control. We are initiating the jump sequence.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);
        
        yield return TypeDialogue(dialogueText, "Jump drive charged. Commencing countdown.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        // Fade screen to load into player view
        yield return TypeDialogue(dialogueText, "Jump in 3... ");
        yield return new WaitForSeconds(0.5f);
        yield return TypeDialogue(dialogueText, "2... ");
        yield return new WaitForSeconds(0.5f);

        // Turn on partical effects for jump
        yield return TypeDialogue(dialogueText, "1...");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);
        yield return new WaitForSeconds(0.5f);
        canvas.SetActive(false);

        
        yield return new WaitForSeconds(5.0f);
        // Trigger flashing lights with warning and turbulance
        canvas.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return TypeDialogue(dialogueText, "Something is pulling at our wake.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        yield return TypeDialogue(dialogueText, "Control, do you read us?");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);



        SetCharacterName(nameText, "CONTROL");

        yield return TypeDialogue(dialogueText, "Captian? You are breaking up.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);



        SetCharacterName(nameText, "SHIP AI");

        yield return TypeDialogue(dialogueText, "Jump drive failed. We are being pulled out of hyperspace.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);
        yield return new WaitForSeconds(0.5f);
        canvas.SetActive(false);
        // Screen flashes to white before fading back in
        yield return new WaitForSeconds(5.0f);
        // We see the sun for the first time and it is close

        canvas.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        yield return TypeDialogue(dialogueText, "It seems we are stuck in a gravity well.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        yield return TypeDialogue(dialogueText, "You will need to jump to a safe location.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        yield return TypeDialogue(dialogueText, "I would estimate you have twenty seconds to re-instate the needed systems to power the jump drive before full meltdown.");
        yield return WaitForPlayerInput();
        ClearDialogue(dialogueText);

        yield return TypeDialogue(dialogueText, "Good luck, Captain.");
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
        while (true)
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
}
