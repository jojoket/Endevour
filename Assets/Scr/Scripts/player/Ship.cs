using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Ship : MonoBehaviour
{
    /// <summary>
    /// The first Room of the ship.
    /// </summary>
    public Room firstRoom;
    public GameObject freeNeighPrefab;
    public Room[,] rooms = new Room[50,50];
    public GameObject[] roomsPrefabs = new GameObject[0];
    public List<GameObject> freeNeighboorsFx = new List<GameObject>();
    public LayerMask freeNeighLayer;


    /// <summary>
    /// The size of one room of the ship. Every Room must have the same size.
    /// </summary>
    public float roomSize;
    
    public bool Building;
    public GameObject selectedRoom;
    public GameObject buildBar;


    void Start() {
        this.rooms = new Room[50, 50];
        this.rooms[25, 25] = this.firstRoom;

    }

    void Update()
    {



        #region BuildMode
        if (this.Building == true) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Collider2D[] results = new Collider2D[1];
                ContactFilter2D contactFilter = new ContactFilter2D();
                contactFilter.layerMask = this.freeNeighLayer;
                contactFilter.useLayerMask = true;
                if (Physics2D.OverlapPoint(Camera.main.ScreenPointToRay(Input.mousePosition).origin, contactFilter, results) > 0 ){
                    Vector2 vector2 = this.GetCellIndexFromWorldPos(results[0].transform.position);
                    this.rooms[Mathf.RoundToInt(vector2.x + 50 / 2), Mathf.RoundToInt(vector2.y + 50 / 2)] = Instantiate(this.selectedRoom, results[0].transform.position, Quaternion.identity, this.transform).GetComponent<Room>();
                    this.UpdateFreeNeighboors();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                this.selectedRoom = this.roomsPrefabs[0];
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                this.selectedRoom = this.roomsPrefabs[1];
            }

        }
        #endregion







    }



    [ContextMenu("update Neighboors")]
    public void UpdateFreeNeighboors() {
        foreach (GameObject g in this.freeNeighboorsFx) {
            Destroy(g);
        }
        this.freeNeighboorsFx.Clear();
        List<Vector2> freeNeighboors = this.GetDisponiblePlaces();
        foreach (Vector2 neighboor in freeNeighboors) {
            this.freeNeighboorsFx.Add(Instantiate(this.freeNeighPrefab, this.GetWorldPosFromCell(Mathf.RoundToInt(neighboor.x), Mathf.RoundToInt(neighboor.y)), Quaternion.identity, this.transform));
        }
    }

    public List<Vector2> GetDisponiblePlaces() {

        List<Vector2> freeNeighboors = new List<Vector2>();

        for (int x = 0; x < 50; x++)  {
            for (int y = 0; y < 50; y++)  {

                if (this.rooms[x,y] != null) {

                    if (this.CheckNeighboor(x, y, Vector2.right) == false) {
                        freeNeighboors.Add(new Vector2(x,y) + Vector2.right);
                    }
                    if (this.CheckNeighboor(x, y, Vector2.left) == false) {
                        freeNeighboors.Add(new Vector2(x, y) + Vector2.left);
                    }
                    if (this.CheckNeighboor(x, y, Vector2.up) == false) {
                        freeNeighboors.Add(new Vector2(x, y) + Vector2.up);
                    }
                    if (this.CheckNeighboor(x, y, Vector2.down) == false) {
                        freeNeighboors.Add(new Vector2(x, y) + Vector2.down);
                    }
                }

            }
        }

        return freeNeighboors;

    }

    private bool CheckNeighboor(int x, int y, Vector2 whichNeighboor){
        if (this.rooms[x + Mathf.RoundToInt(whichNeighboor.x), y + Mathf.RoundToInt(whichNeighboor.y)] != null) {
            return true;
        }
        return false;
    }


    public Vector3 GetWorldPosFromCell(int x, int y) {
        return new Vector3((x - 25) * this.roomSize, (y - 25) * this.roomSize, 0);
    }

    public Vector2 GetCellIndexFromWorldPos(Vector3 worldPos) {
        return new Vector2(Mathf.RoundToInt(worldPos.x/this.roomSize), Mathf.RoundToInt(worldPos.y/this.roomSize));
    }


    #region State
    public void ChangeToBuildMode() {
        if (this.Building == false) {
            this.Building = true;
            this.UpdateFreeNeighboors();
            this.buildBar.SetActive(true);
        } else {
            this.Building = false;
            this.buildBar.SetActive(false);
            foreach (GameObject g in this.freeNeighboorsFx) {
                Destroy(g);
            }
            this.freeNeighboorsFx.Clear();
        }
    }
    #endregion



}
