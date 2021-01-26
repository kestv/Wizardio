using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;
    [SerializeField]
    int port = 80;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists. Destroying object");
            Destroy(this);
        }
    }

    public void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(50, port);

    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer(Vector3 _pos)
    {
        return Instantiate(playerPrefab, _pos, Quaternion.identity).GetComponent<Player>();
    }
}
