using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{

    public int numberOfCars;
    int speed = 1;
    List<Car> cars;

    // Start is called before the first frame update
    void Start()
    {
        cars = new List<Car>();
        for (int i = 0; i < numberOfCars; i++){
            Car car = new Car();
            car.location = i;
            cars.Add(car);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
