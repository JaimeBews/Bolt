using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the goal of this class is to create the rooms and hallways connecting them
public class RoomCreation : MonoBehaviour
{
    
    public List<GameObject> m_Rooms;//0 is spawn 
    public int numFloors = 4;
    private int m_CurrRoom = 0;
    private int m_reservedRooms = 1;
    public List<GameObject> m_ActiveRooms;
    // Start is called before the first frame update
    void Start()
    {
        GenerateFloor();
    }
    void GenerateFloor() {
        //spawn player at start pos
        //spawn room should be consitent
        //1 exit doesnt matter what side 
        // Vector2 startPosition = new Vector2(Random.Range(0, floorSize), Random.Range(0, floorSize));// what if this doesn't matter just keep the other rooms close by 
        m_ActiveRooms.Add(Instantiate(m_Rooms[0]));
        for(int i = 0; i<numFloors; i++) {
            if (m_ActiveRooms.Count > numFloors)
                break;
            Vector3 nextExit = m_ActiveRooms[m_CurrRoom].GetComponent<RoomData>().GetExit();
            if(nextExit.magnitude == 0) {
                int side = Random.Range(0, 4);//direction for room to come off of
                Vector3 offset = new Vector3(0, 0, 0);
                switch (side) {
                    case 0:
                        offset.x = -1;
                        break;
                    case 1:
                        offset.x = 1;
                        break;
                    case 2:
                        offset.z = -1;
                        break;
                    case 3:
                        offset.z = 1;
                        break;
                    default:
                        break;
                }
                offset *= 10;//put it out by 10 units
                int dir = Random.Range(-1, 1);//offset for height or width
                int roomIndex = Random.Range(m_reservedRooms, m_Rooms.Count);
                if (side < 2) {
                    offset.z += dir * m_ActiveRooms[i].GetComponent<Renderer>().bounds.size.z;//add for the final room position
                    if(side ==0)
                        offset.x -= m_Rooms[roomIndex].GetComponent<Renderer>().bounds.size.x;
                    else
                        offset.x += m_Rooms[roomIndex].GetComponent<Renderer>().bounds.size.x;
                } else {
                    offset.x += dir * m_ActiveRooms[i].GetComponent<Renderer>().bounds.size.x;
                    if (side ==2)
                        offset.z -= m_Rooms[roomIndex].GetComponent<Renderer>().bounds.size.z;
                    else
                        offset.z += m_Rooms[roomIndex].GetComponent<Renderer>().bounds.size.z;
                }
                offset += m_ActiveRooms[i].transform.position;
                m_ActiveRooms.Add(Instantiate(m_Rooms[roomIndex], offset, Quaternion.identity));
                for (int j = 0; j < m_ActiveRooms.Count; j++) {
                    if(j == i + 1) {
                        continue;
                    }
                    if (m_ActiveRooms[i + 1].GetComponent<Collider>().bounds.Intersects(m_ActiveRooms[j].GetComponent<Collider>().bounds)) {
                        Destroy(m_ActiveRooms[i + 1]);
                        m_ActiveRooms.RemoveAt(i + 1);
                        i--;
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
