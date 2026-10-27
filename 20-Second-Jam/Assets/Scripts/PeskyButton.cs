using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PeskyButton : MonoBehaviour
{
    [Header("Timer Variables")]
    public float timeIntervalMin;
    public float timeIntervalMax;

    private float timeInterval = 0.0f;
    private bool timerStarted = false;
    private float timeRemaining = 0.0f;

    [Header("Button Variables")]
    public int peskyButtonWinCondition;
    private int buttonClickCount = 0;
    public Button thePeskyButton;
    public TextMeshProUGUI thePeskyButtonText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Disables button on intialization. Sets the time interval and begins the timer.
        thePeskyButton.enabled = false;
        SetColor();
        SetTimeInterval();
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        // checks if timer is started, if so timeRemaining will be decremented until it reaches 0. At 0, the button will be enabled and the cycle starts again.
        if(timerStarted)
        {
            timeRemaining -= Time.deltaTime;

            // Currently use being used for debugging.
            thePeskyButtonText.text = "Time Remaining: " + Mathf.Round(timeRemaining).ToString();   
            //
            if (timeRemaining <= 0)
            {
                thePeskyButton.enabled = true;
                SetColor();
                timerStarted = false;
                thePeskyButtonText.text = "FEED ME!";
            }
        }
    }

    // Function called when button is clicked.
    public void OnClick()
    {
        //Increments total towards WinCon.
        buttonClickCount++;

        // Checks if button is satisfied. If not, the timer will begin again with a new interval. If true, mini game is over!
        if (!isButtonSatisfied())
        {
            thePeskyButton.enabled = false;
            SetColor();
            SetTimeInterval();
            StartTimer();
        }
        else
        {
            thePeskyButton.enabled = false;
            SetColor();
        }
    }
    
    // Function uses the set min and max values to generate a random interval for what parameters we set. Rounds to nearest whole number to keep it pretty.
    private void SetTimeInterval()
    {
        timeInterval = Random.Range(timeIntervalMin, timeIntervalMax);
        timeInterval = Mathf.Round(timeInterval);
        print("Time Interval: " + timeInterval);
    }

    //Starts timer and sets timerRemaing to the interval generated.
    private void StartTimer()
    {
        timerStarted = true;
        timeRemaining = timeInterval;
    }

    // Sets the button colors based on its current state.
    private void SetColor()
    {
        if (thePeskyButton.enabled)
        {
            thePeskyButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            thePeskyButton.GetComponent<Image>().color = Color.green;
        }
    }

    //Return bool based on if the win condition is met.
    private bool isButtonSatisfied()
    {
        if (buttonClickCount >= peskyButtonWinCondition)
        {
            thePeskyButton.enabled = false;
            SetColor();
            thePeskyButtonText.text = "Ugh so many clicks...";
            return true;
        }
        return false;
    }

}