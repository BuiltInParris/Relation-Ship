using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public int numberOfCars;
    int speed = 1;
    List<GameObject> cars;
    public GameObject carPrefab;

    // Start is called before the first frame update
    void Start()
    {
        cars = new List<GameObject>();
        for (int i = 0; i < numberOfCars; i++){
            GameObject car = Instantiate(carPrefab, new Vector3((i / 2) - 5, 0, 0), Quaternion.identity);
            car.GetComponent<Car>().location = i;
            cars.Add(car);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
