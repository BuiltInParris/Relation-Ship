using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraScript : MonoBehaviour
{
    Transform t1;
    Transform t2;
    GameObject[] players;

    void Start(){
        players = GameObject.FindGameObjectsWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        FixedCameraFollowSmooth(gameObject.GetComponent<Camera>());
    }

    // Follow Two Transforms with a Fixed-Orientation Camera
    public void FixedCameraFollowSmooth(Camera cam)
    {
        // How many units should we keep from the players
        float zoomFactor = 1.5f;
        float followTimeDelta = 0.8f;

        Vector3 midpoint = new Vector3(0, 0, 0);
        float distance = Constants.PLAYER_CAMERA_MINIMUM_DISTANCE;
        for (int i = 0; i < players.Length; i++)
        {
            // Midpoint we're after
            midpoint = midpoint + players[i].transform.position;

            // Distance between objects
            for (int j = 0; j < players.Length; j++)
            {
                float newDistance = (players[j].transform.position - players[i].transform.position).magnitude;
                if (distance < newDistance){
                    distance = newDistance;
                }
            }
        }

        midpoint = midpoint / players.Length;

        // Move camera a certain distance
        Vector3 cameraDestination = midpoint - cam.transform.forward * distance * zoomFactor;

        // Adjust ortho size if we're using one of those
        if (cam.orthographic)
        {
            // The camera's forward vector is irrelevant, only this size will matter
            cam.orthographicSize = distance / 2.5f;
        }
        // You specified to use MoveTowards instead of Slerp
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, followTimeDelta);

        // Snap when close enough to prevent annoying slerp behavior
        if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
            cam.transform.position = cameraDestination;

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 0, 10);
    }
}
