using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    private DictationRecognizer dictationRecognizer;

    // Use this for initialization
    void Start()
    {

        startRecognizing();
    }

    private void Awake()
    {
        dictationRecognizer = new DictationRecognizer();
    }

    private void Update()
    {
        if (!keywordRecognizer.IsRunning && PhraseRecognitionSystem.Status == SpeechSystemStatus.Running)
        {
            keywordRecognizer.Start();
        }
    }

    public void startRecognizing()
    {
        if (keywords.Count == 0)
        {
            keywords.Add("Ground", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("onGround");
            });

            keywords.Add("Simulate", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("onSimulate");
            });

            keywords.Add("Resistor", () =>
            {
            // Call the OnReset method on every descendant object.
            this.BroadcastMessage("onResistor");
            });

            keywords.Add("Voltage Source", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("onVoltageSource");
            });

            keywords.Add("Complete", () =>
            {
            // Call the OnReset method on every descendant object.
            this.BroadcastMessage("onComplete");
            });

            keywords.Add("Stop", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("onStop");
            });

            keywords.Add("Reset", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("onReset");
            });

            keywords.Add("Undo", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("onUndo");
            });

            keywords.Add("Rotate", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("onRotate");
            });

            keywords.Add("Zero", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("button0Listener");
            });

            keywords.Add("One", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("button1Listener");
            });

            keywords.Add("Two", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("button2Listener");
            });

            keywords.Add("Three", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("button3Listener");
            });

            keywords.Add("Four", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("button4Listener");
            });

            keywords.Add("Five", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("button5Listener");
            });

            keywords.Add("Six", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("button6Listener");
            });

            keywords.Add("Seven", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("button7Listener");
            });

            keywords.Add("Eight", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("button8Listener");
            });

            keywords.Add("Nine", () =>
            {
                // Call the OnReset method on every descendant object.
                this.BroadcastMessage("button9Listener");
            });


            // Tell the KeywordRecognizer about our keywords.
            keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());


            // Register a callback for the KeywordRecognizer and start recognizing!
            keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        }
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}