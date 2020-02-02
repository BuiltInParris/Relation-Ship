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
        Vector3 position = this.transform.position;
        position -= new Vector3(-0.3f, 0.38f, 1f);
        for (int i = 0; i < numberOfDevices; i++){
            GameObject device = Instantiate(devicePrefab, position, Quaternion.identity);
            devices.Add(device);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
