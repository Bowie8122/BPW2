using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CameraController : MonoBehaviour
{
    public static scr_CameraController instance;
    public scr_Room currRoom;
    public float moveSpeedRoomChange;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();   
    }

    void UpdatePosition()
    {
        //Als er geen current room is, Exit out
        if (currRoom == null)
        {
            return;
        }

        Vector3 targetPos = GetCameraTargetPosition();

        //Move the Camera
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedRoomChange);
    }

    Vector3 GetCameraTargetPosition()
    {
        //Als er geen current room is, Exit out
        if (currRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPos = currRoom.GetRoomCenter();
        targetPos.z = transform.position.z;

        return targetPos;
    }

    public bool IsSwitchingScene()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
