using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DebugServer : MonoBehaviour
{
    public readonly string DEBUG_SERVER_ADDRESS = "192.168.2.57";

    public readonly bool USE_DEBUG_SERVER = false;

    public void SendDebugFile(string filename, byte[] bytes)
    {
        UnityEngine.WSA.Application.InvokeOnAppThread(() =>
        {
            var www = UnityWebRequest.Put($"http://{DEBUG_SERVER_ADDRESS}:8000/{filename}", bytes);
            StartCoroutine(SendWebRequest(www));
        }, false);
    }

    private static IEnumerator SendWebRequest(UnityWebRequest www)
    {
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Sent web request");
        }
    }
}
