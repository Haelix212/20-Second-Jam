using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SequenceGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Sequence Variables")]
    public int sequenceLength;
    public GameObject sequencePrefab;
    public Vector3 spawnOffset;
    public Vector3 secondRowOffset;
    public float lockOutTimer;

    private GameObject[] buttons;
    private int sequenceIndex;

    void Start()
    {
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
        SpawnButtons();
        AssignNumbers();
    }

    private void SpawnButtons()
    {
        int secondRowIndex = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = Instantiate(sequencePrefab, this.transform);

            RectTransform rectTransform = buttons[i].GetComponent<RectTransform>();
            Vector3 spawnPosition = rectTransform.anchoredPosition;

            if (i < buttons.Length / 2)
            {
                rectTransform.anchoredPosition = spawnPosition + spawnOffset * i;
            }
            else
            {
                rectTransform.anchoredPosition = spawnPosition + (spawnOffset * secondRowIndex) + secondRowOffset;
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

    public void CheckSequence(GameObject clickedButton)
    {
        Text buttonText = clickedButton.transform.GetChild(0).GetComponent<Text>();

        print (buttonText.text);
        print(sequenceIndex);

        if(buttonText.text == sequenceIndex.ToString())
        {
            clickedButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
            clickedButton.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            sequenceIndex++;
        }
        else
        {
            clickedButton.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            /*
            foreach(GameObject button in buttons)
            {
                button.GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
            */
            ResetPuzzle();
        }
    }

    private void ResetPuzzle()
    {
        sequenceIndex = 0;
        foreach (GameObject button in buttons)
        {
            button.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            button.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }
}
