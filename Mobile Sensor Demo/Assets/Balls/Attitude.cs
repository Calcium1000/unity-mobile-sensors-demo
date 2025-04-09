using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;


public class Attitude : MonoBehaviour
{

    public GameObject button;
    public static DateTime dateTimeOnLoad;
    [SerializeField] string dataPath;
    float timeElapsed;
    private void Start()
    {
        Input.gyro.enabled = true;  
        dateTimeOnLoad = DateTime.Now;
        dataPath = Path.Combine(Application.persistentDataPath, $"{Input.gyro.attitude}sensorData.CSV");
    }
    private void Update()
    {
        if (SystemInfo.supportsGyroscope)
        {
            //Getting the gyro data
            Quaternion gyro = Input.gyro.attitude;

            //Adjusting 
            Quaternion adjustedGyro = new Quaternion(gyro.x,-gyro.y,gyro.z, gyro.w);
            Vector3 angels = adjustedGyro.eulerAngles;
            gameObject.transform.rotation = Quaternion.Euler(-angels.x, 0, angels.y);

            timeElapsed += Time.deltaTime;
            if (timeElapsed >= 0.1)
            {
                Debug.Log(angels);
                timeElapsed = 0;
            }
        }
    }

    public void PrintAttitude()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Quaternion gyro = Input.gyro.attitude;
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"{gyro}";
            File.AppendAllText(dataPath, gyro.ToString() + "\n");
            Debug.Log(dataPath);
            return;
        }
        button.GetComponentInChildren<TextMeshProUGUI>().text = "No gyro sensor data";
    }
}