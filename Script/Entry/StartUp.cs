using Aquila;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    void Start()
    {
        if ( _game_entry == null )
            throw new GameFrameworkException( "GameEntry is null!" );

        Tools.SetActive( _game_entry, true );
    }

    [SerializeField] private GameObject _game_entry = null;
}
