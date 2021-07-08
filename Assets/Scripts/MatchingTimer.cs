using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingTimer : MonoBehaviour
{


    public GUIStyle Clockstyle;

    private float timer;
    private float minutes;
    private float seconds;

    private const float VirtualWidth = 650.0f;
    private const float VirtualHeight = 854.0f;


    private bool stopTimer;
    private Matrix4x4 matrix;
    private Matrix4x4 oldMatrix;

    // Start is called before the first frame update
    void Start()
    {
        stopTimer = false;
        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / VirtualWidth, Screen.height / VirtualHeight, 1.0f));
        oldMatrix = GUI.matrix;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stopTimer)
        {
            timer += Time.deltaTime;
        }
    }


    private void OnGUI()
    {
        GUI.matrix = matrix;
        seconds = Mathf.RoundToInt(timer);

        GUI.Label(new Rect(Camera.main.rect.x + 260, 50, 120, 50), ""  + seconds.ToString(), Clockstyle);
        GUI.matrix = oldMatrix;
    }

    public float GetCurrentTime()
    {
        return timer;
    }

    public void StopTimer()
    {
        stopTimer = true;
    }
}
