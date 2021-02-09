using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Room : MonoBehaviour
{
    public FoggedDoor[] foggedDoors;
    List<FoggedDoor> availableDoors;
    public MeshCollider meshCollider;
    public RoomEnemySystem roomEnemySystem;
    [SerializeField] bool ignoreSpawner = false;
    [SerializeField] bool isRoomClear = false;

    Keyboard keyboard;

    public Bounds RoomBounds {
        get { return meshCollider.bounds; }
    }

    private void Start() {
        keyboard = InputSystem.GetDevice<Keyboard>();
        availableDoors = new List<FoggedDoor>();
    }
    void OnTriggerEnter(Collider other) {
        Debug.Log("Entered");
        if (other.gameObject.CompareTag("Player")) {
            foreach (FoggedDoor door in foggedDoors) {
                if (!door.gameObject.activeSelf) {
                    availableDoors.Add(door);
                }
                door.gameObject.SetActive(true);
            }
            roomEnemySystem.StartRoomSpawners();
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void FixedUpdate() {
        // need to detect if player cleared the room
        // using NumPad + for replacement for now
        if (!ignoreSpawner && roomEnemySystem.IsRoomClear()) {
            //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            //foreach (GameObject enemy in enemies) Destroy(enemy);
            foreach (FoggedDoor door in availableDoors) door.gameObject.SetActive(false);
            isRoomClear = true;
        }
    }
}
