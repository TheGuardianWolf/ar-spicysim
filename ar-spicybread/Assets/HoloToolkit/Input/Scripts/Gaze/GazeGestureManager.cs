using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.WSA.Input;

public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }

    public delegate void OnTappedEventHandler(GameObject focusedObject);

    public static event OnTappedEventHandler OnTappedEvent;

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;

    // Use this for initialization
    void Awake()
    {
        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.Tapped += (args) =>
        {
            // Send an OnSelect message to the focused object and its children.
            if (FocusedObject != null)
            {
                if (OnTappedEvent != null)
                {
                    OnTappedEvent(FocusedObject);
                }
                    
                if (FocusedObject.name.Contains("ResistorComponent") || FocusedObject.name.Contains("BatteryComponent") || FocusedObject.name.Contains("Node"))
                {
                    FocusedObject.SendMessage("OnSelect", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    FocusedObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                BroadcastMessage("OnTapped", null);
            }
        };
        recognizer.StartCapturingGestures();
    }

    // Update is called once per frame
    void Update()
    {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }

        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if (FocusedObject != oldFocusObject)
        {
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
    }
}