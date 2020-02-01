using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    List<Device> devices;
    int numberOfDevices = 1;
    public int location;
    // Start is called before the first frame update
    void Start()
    {
        devices = new List<Device>();
        for (int i = 0; i < numberOfDevices; i++){
            Device device = new Device();
            devices.Add(device);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
