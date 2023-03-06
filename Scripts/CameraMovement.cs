using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f); // needed for the game to visible
    private Vector3 velocity = Vector3.zero;
    private float cameraSmoothening = 0.25f; // how well the camera 'keeps up' with player movement

    private Transform player;

    void Start()
    {
        AttachCamera();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Global.playerMovementActive)
        {
            if (player == null)
            {
                AttachCamera();
            }
            else
            {
                if (!Global.cameraLocked)
                {
                    // can be more sophisticated ( eg: moving more back at high speeds )
                    Vector3 playerPosition = new Vector3(Mathf.Round(player.position.x + offset.x), Mathf.Round(player.position.y + offset.y), offset.z);  //round
                    transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, cameraSmoothening);
                }
            }
        }
    }

    void AttachCamera()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        DebugStats.AddLog("Bounded player to camera");
    }
}
