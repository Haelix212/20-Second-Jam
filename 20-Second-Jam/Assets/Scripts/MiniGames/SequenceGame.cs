using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SequenceGame : PuzzleClass
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Sequence Variables")]
    public int sequenceLength;
    public GameObject buttonPrefab;
    public float lockOutTimer;
    public AudioSource keyClick;
    public AudioSource failSound;

    private GameObject[] buttons;
    private int sequenceIndex;
    private Vector3 buttonSpawnPosition;
    private Vector3 spawnOffset;
    private Vector3 secondRowOffset;

    void Start()
    {
        // StartPuzzle();
    }

    public override void StartPuzzle()
    {
        spawnOffset = new Vector3(buttonPrefab.GetComponent<RectTransform>().rect.width, 0, 0);
        secondRowOffset = new Vector3(0, -buttonPrefab.GetComponent<RectTransform>().rect.height, 0);
        buttons = new GameObject[sequenceLength];
        GenerateSequence();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateSequence()
    {
        sequenceIndex = 0;
        AdjustPanelWidth();
        SetSpawnLocations();  
        SpawnButtons();
        AssignNumbers();
    }

    private void SpawnButtons()
    {
        int secondRowIndex = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = Instantiate(buttonPrefab, this.transform);
            buttons[i].GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
            buttons[i].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CheckSequence);

            RectTransform rectTransform = buttons[i].GetComponent<RectTransform>();
            //buttonSpawnPosition = rectTransform.anchoredPosition;

            if (i < buttons.Length / 2)
            {
                rectTransform.anchoredPosition = buttonSpawnPosition + spawnOffset * i;
            }
            else
            {
                rectTransform.anchoredPosition = buttonSpawnPosition + (spawnOffset * secondRowIndex) + secondRowOffset;
                secondRowIndex++;
            }
        }
    }

    private void AssignNumbers()
    { 
        List<int> sequenceNumbers = new List<int>(sequenceLength);

        for (int i = 0; i < sequenceLength; i++)
        {
            sequenceNumbers.Add(i);
        }

        for (int i = 0; i < sequenceLength; i++) 
        {
            int randomIndex = Random.Range(0, sequenceNumbers.Count);
            int number = sequenceNumbers[randomIndex];

            Text buttonText = buttons[i].transform.GetChild(0).GetComponent<Text>();
            buttonText.text = sequenceNumbers[randomIndex].ToString();
            sequenceNumbers.RemoveAt(randomIndex);
        }
    }

    public void CheckSequence()
    {
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Text buttonText = clickedButton.transform.GetChild(0).GetComponent<Text>();

        if (buttonText != null)
        {
            if (buttonText.text == sequenceIndex.ToString())
            {
                if (keyClick != null) keyClick.PlayOneShot(keyClick.clip);
                clickedButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
                clickedButton.GetComponentInChildren<Text>().color = Color.green;
                sequenceIndex++;
                CheckForWin();
            }
            else
            {
                if (failSound != null) failSound.Play();
                clickedButton.GetComponentInChildren<Text>().color = Color.red;
                foreach(GameObject button in buttons)
                {
                    button.GetComponent<UnityEngine.UI.Button>().interactable = false;
                }

                StartCoroutine(WaitAndResetPuzzle());
            }
        }
    }

    private void ResetPuzzle()
    {
        sequenceIndex = 0;
        foreach (GameObject button in buttons)
        {
            button.GetComponentInChildren<Text>().color = Color.black;
            button.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    private IEnumerator WaitAndResetPuzzle()
    {
        yield return new WaitForSeconds(lockOutTimer);
        ResetPuzzle();
    }

    private void CheckForWin()
    {
        if (sequenceIndex == sequenceLength)
        {
            Debug.Log("Sequence Completed!");
            PuzzleManager.Instance.SetPuzzleCompleted("Sequence");
        }
    }

    private void AdjustPanelWidth()
    {
        float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonWidth * (sequenceLength/2 + 1));
    }

    private void SetSpawnLocations()
    {
        float height = GetComponent<RectTransform>().rect.height;
        float width = GetComponent<RectTransform>().rect.width;
        buttonSpawnPosition = new Vector3(-width / 3, (height-10f)/4, 0);
    }

}
