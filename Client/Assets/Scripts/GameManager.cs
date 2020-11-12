﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public Map[] maps;

    GameObject loadedMapGO;
    int loadedMapId = -1;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation) {
        GameObject _player;
        if (_id == Client.instance.myId) {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
            CameraController.instance.target = _player.transform;
        } else {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        PlayerManager playerManager = _player.GetComponent<PlayerManager>();
        playerManager.id = _id;
        playerManager.username = _username;
        if (playerManager.usernameFeild != null) {
            playerManager.usernameFeild.text = _username;
        }
        players.Add(_id, playerManager);
    }

    public void DestroyPlayer(int id) {
        GameObject player = players[id].gameObject;
        players.Remove(id);

        Destroy(player);
    }

    public void LoadMap(int mapId) {
        if (mapId == loadedMapId) {
            return;
        }

        if (mapId >= maps.Length || mapId < 0) {
            Debug.LogError("Load Map: Index outside bouds of array!");
            return;
        }

        if (loadedMapGO != null) {
            Destroy(loadedMapGO);
        }

        loadedMapGO = Instantiate(maps[mapId].gameObject);
        loadedMapId = mapId;

        loadedMapGO.name = "Map " + mapId.ToString();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            ClientSend.LoadMap(0);
        }

        if (Input.GetKeyDown(KeyCode.O)) {
            ClientSend.LoadMap(1);
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            ClientSend.LoadMap(2);
        }
    }
}