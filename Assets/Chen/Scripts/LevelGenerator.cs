using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    public Room roomStartPrefab, roomEndPrefab;
    public List<Room> roomPrefabs = new List<Room>();
    public Vector2 roomNumberRange = new Vector2 (3,6);

    List<FoggedDoor> idleFoggedDoors = new List<FoggedDoor>();

    RoomStart roomStart;
    RoomBoss roomBoss;
    List<Room>  generatedRooms = new List<Room>();

    LayerMask roomLayerMask;

    void Start() {
        roomLayerMask = LayerMask.GetMask("Room");
        StartCoroutine("GenerateLevel");
    }

    IEnumerator GenerateLevel() {
        WaitForSeconds startup = new WaitForSeconds(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        yield return startup;

        PlaceStartRoom();
        yield return interval;

        int iterations = Random.Range((int)roomNumberRange.x, (int)roomNumberRange.y);

        for (int i = 0; i < iterations; i++) {
            PlaceRoom();
            yield return interval;
        }

        PlaceBossRoom();
        yield return interval;

        Debug.Log("Level Generated!");

        NavMeshSurface[] navSurfaces = GameObject.FindObjectsOfType<NavMeshSurface>();

        foreach (NavMeshSurface navSurface in navSurfaces) {
            Debug.Log("Baking Navmesh for " + navSurface);
            navSurface.BuildNavMesh();
        }

    }

    void AddFoggedDoorToList (Room room, ref List<FoggedDoor> list) {
        foreach (FoggedDoor foggedDoor in room.foggedDoors) {
            list.Insert (Random.Range(0, list.Count), foggedDoor);
        }
    }

    void PlaceStartRoom() {
        roomStart = Instantiate(roomStartPrefab) as RoomStart;
        roomStart.transform.parent = this.transform;

        AddFoggedDoorToList (roomStart, ref idleFoggedDoors);

        roomStart.transform.position = Vector3.zero;
        roomStart.transform.rotation = Quaternion.identity;
    }

    void PlaceRoom() {
        Room currentRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)]) as Room;
        currentRoom.transform.parent = this.transform;

        List<FoggedDoor> idleFoggedDoors_Copy = new List<FoggedDoor>(idleFoggedDoors);
        List<FoggedDoor> currentRoomDoors = new List<FoggedDoor>();
        AddFoggedDoorToList(currentRoom, ref currentRoomDoors);

        AddFoggedDoorToList(currentRoom, ref idleFoggedDoors);
        bool placed = false;

        foreach (FoggedDoor idleFoggedDoor in idleFoggedDoors_Copy) {
            foreach (FoggedDoor currentFoggedDoor in currentRoomDoors) {
                PlaceRoomAtFoggedDoor(ref currentRoom, currentFoggedDoor, idleFoggedDoor);
                if (CheckRoomOverlap(currentRoom)) continue;
                placed = true;

                generatedRooms.Add(currentRoom);
                //currentFoggedDoor.gameObject.GetComponent<MeshCollider>().enabled = false;
                currentFoggedDoor.gameObject.SetActive(false);
                idleFoggedDoors.Remove(currentFoggedDoor);
                //idleFoggedDoor.gameObject.GetComponent<MeshCollider>().enabled = false;
                idleFoggedDoor.gameObject.SetActive(false);
                idleFoggedDoors.Remove(idleFoggedDoor);

                break;
            }
            if (placed) break;
        }

        if (!placed) {
            Destroy (currentRoom.gameObject);
            ResetLevelGenerator();
        }
    }

    void PlaceRoomAtFoggedDoor(ref Room room, FoggedDoor foggedDoor, FoggedDoor otherFoggedDoor) {
        room.transform.position = Vector3.zero;
        room.transform.rotation = Quaternion.identity;

        Vector3 foggedDoorEuler = foggedDoor.transform.eulerAngles;
        Vector3 otherFoggedDoorEuler = otherFoggedDoor.transform.eulerAngles;

        room.transform.rotation = Quaternion.AngleAxis(Mathf.DeltaAngle(foggedDoorEuler.y, otherFoggedDoorEuler.y), Vector3.up) * Quaternion.Euler(0, 180f, 0);
        
        room.transform.position = otherFoggedDoor.transform.position - foggedDoor.transform.position + room.transform.position;
    }

    bool CheckRoomOverlap(Room room) {
        Bounds bounds = room.RoomBounds;
        bounds.Expand(-0.1f);

        RaycastHit[] colliders = Physics.BoxCastAll(bounds.center, bounds.size / 2, room.transform.forward, room.transform.rotation, roomLayerMask);

        if (colliders.Length > 0) {
            foreach (RaycastHit c in colliders) {
                if (c.collider.transform.parent.gameObject.Equals(room.gameObject)) continue;
                else {
                    return true;
                }
            }
        }
        return false;
    }

    void PlaceBossRoom() {
        roomBoss = Instantiate(roomEndPrefab) as RoomBoss;
        roomBoss.transform.parent = this.transform;

        List<FoggedDoor> idleFoggedDoors_Copy = new List<FoggedDoor>(idleFoggedDoors);
        FoggedDoor foggedDoor = roomBoss.foggedDoors[0];

        bool placed = false;

        foreach (FoggedDoor idleFoggedDoor in idleFoggedDoors_Copy) {
            Room room = (Room)roomBoss;
            PlaceRoomAtFoggedDoor (ref room, foggedDoor, idleFoggedDoor);
            if (CheckRoomOverlap(roomBoss)) continue;

            placed = true;

            //foggedDoor.gameObject.GetComponent<MeshCollider>().enabled = false;
            foggedDoor.gameObject.SetActive(false);
            idleFoggedDoors.Remove(foggedDoor);
            //idleFoggedDoor.gameObject.GetComponent<MeshCollider>().enabled = false;
            idleFoggedDoor.gameObject.SetActive(false);
            idleFoggedDoors.Remove(idleFoggedDoor);

            break;
        }
        if (!placed) {
            ResetLevelGenerator();
        }
    }

    void ResetLevelGenerator() {
        StopCoroutine("GenerateLevel");
        if (roomStart) Destroy(roomStart.gameObject);
        if (roomBoss) Destroy(roomBoss.gameObject);
        foreach (Room room in generatedRooms) Destroy(room.gameObject);

        generatedRooms.Clear();
        idleFoggedDoors.Clear();

        StartCoroutine ("GenerateLevel");
    }
}
