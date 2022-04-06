using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoom : MonoBehaviour
{
    private const int PLAYERCHARACTOR_COUNT = 3;

    [SerializeField]
    private GameObject _riflePlayer;
    [SerializeField]
    private GameObject _bazookaPlayer;
    [SerializeField]
    private GameObject _sniperPlayer;

    private List<GameObject> _playerCharactor = new List<GameObject>();

    private int index = 0;

    private void Awake()
    {
        _playerCharactor.Add(_riflePlayer);
        _playerCharactor.Add(_bazookaPlayer);
        _playerCharactor.Add(_sniperPlayer);

        _playerCharactor[index].SetActive(true);

        for (int i = 1; i < _playerCharactor.Count; ++i)
        {
            _playerCharactor[i].SetActive(false);
        }
    }

    public void RightClick()
    {
        _playerCharactor[index].SetActive(false);
        ++index;

        if (index > PLAYERCHARACTOR_COUNT - 1)
        {
            index = 0;
        }

        _playerCharactor[index].SetActive(true);
    }

    public void LeftClick()
    {
        _playerCharactor[index].SetActive(false);
        --index;

        if (index < 0)
        {
            index = PLAYERCHARACTOR_COUNT - 1;
        }

        _playerCharactor[index].SetActive(true);
    }
}
