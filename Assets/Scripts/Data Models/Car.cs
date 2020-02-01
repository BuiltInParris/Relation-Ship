using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    List<GameObject> devices;
    public GameObject devicePrefab;
    int numberOfDevices = 1;
    public int location;
    // Start is called before the first frame update
    void Start()
    {
        devices = new List<GameObject>();
        for (int i = 0; i < numberOfDevices; i++){
            GameObject device = Instantiate(devicePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            devices.Add(device);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
