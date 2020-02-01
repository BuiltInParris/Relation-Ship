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
        int playerCount = 2;//GameObject.Find("GameSettings").playerCount;
        devices = new List<GameObject>();
        for (int i = 0; i < numberOfDevices; i++){
            int xLoc = Constants.DISTANCE_BETWEEN_CARS * (i - playerCount / 2);
            GameObject device = Instantiate(devicePrefab, new Vector3(xLoc, 0, 0), Quaternion.identity);
            devices.Add(device);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
