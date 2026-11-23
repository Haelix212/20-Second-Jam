using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Dials : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject dial;
    public string dialAnswer;
    public TMP_Text dialText;
    public List<string> directions = new List<string> { "Left", "Right", "Down" };
    [SerializeField] private Dictionary<string, GameObject> lights;




    void SetAnswers()
    {
        dialAnswer = GetRandomString();
    }
    public string GetRandomString()
    {
        if (directions.Count == 0) return string.Empty;

        int randomIndex = Random.Range(0, directions.Count);
        return directions[randomIndex];
    }


    void Start()
    {
        SetAnswers();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
