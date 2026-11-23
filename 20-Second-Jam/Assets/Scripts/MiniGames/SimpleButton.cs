using UnityEngine;
using UnityEngine.UI;

public class SimpleButton : PuzzleClass
{
    [Header("Set this to a unique int when hooking up to PuzzleManager.cs")]
    [SerializeField] int SimpleButtonNumber = -1;
    [SerializeField] string PuzzleType = "Button";

    private Animation animation;
    private AudioSource soundEffect;

    private Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // StartPuzzle();
    }

    public override void StartPuzzle()
    {
        button = this.GetComponent<Button>();
        animation = this.gameObject.GetComponentInChildren<Animation>();
        soundEffect = this.gameObject.GetComponentInChildren<AudioSource>();
        button.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        PuzzleManager.Instance.SetPuzzleCompleted("Simple" + PuzzleType + SimpleButtonNumber);
        if (animation != null) animation.Play();
        if(soundEffect != null) soundEffect.PlayOneShot(soundEffect.clip);
        button.interactable = false;
    }
}
