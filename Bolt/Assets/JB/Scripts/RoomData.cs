using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public List<Vector3> roomExits;

    public Vector3 GetExit() {// maybe a struct with a boolean for exit found would be better

        if(roomExits.Count>0)
            return roomExits[Random.Range(0, roomExits.Count)];

        return new Vector3(0, 0, 0);
    }


}
