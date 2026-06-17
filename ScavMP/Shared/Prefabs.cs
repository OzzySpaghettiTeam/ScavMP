using UnityEngine;

namespace ScavMP.Shared;

public class Prefabs
{
    private GameObject _playerPrefab;
    public GameObject PlayerPrefab => _playerPrefab;

    public void RegisterPlayerPrefab()
    {
        _playerPrefab = GameObject.Find("Experiment");
    }

    public static Prefabs Instance;
}
