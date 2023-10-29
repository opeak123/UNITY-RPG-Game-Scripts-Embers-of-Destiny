using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    

    //SaveManager _save;
    //LoadManager _load;
    PlayerStateManager _playerState;
    public GameObject[] characterPrefabs;

    private Transform _spawnPosition;


    private void Awake()
    {
        //_save = FindObjectOfType<SaveManager>();
        //_load = FindObjectOfType<LoadManager>();
        //_load.Load();
        _playerState = FindObjectOfType<PlayerStateManager>();

        

        Transform _spawnPosition = GameObject.Find("SpawnPoint").transform;

        int jobIndex = PlayerPrefs.GetInt("SelectedJobIndex");
        _playerState.SetCLASS(jobIndex);
        //int jobIndex = _playerState.GetCLASS();
        Instantiate(characterPrefabs[jobIndex], _spawnPosition.transform.position, _spawnPosition.transform.rotation);


    }
    public void RecallTown()
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        saveManager.Save();

        SceneManager.LoadScene("Map");
    }
}