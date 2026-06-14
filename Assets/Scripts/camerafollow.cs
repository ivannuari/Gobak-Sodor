using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public Transform target; // player (drag ke sini di Inspector)
    public Vector3 offset = new Vector3(0, 5, -8); // posisi kamera di belakang atas player
    public float smoothSpeed = 0.125f;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target); // kamera selalu menghadap ke player
    }
}
