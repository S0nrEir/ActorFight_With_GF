using Aquila;
using Aquila.ToolKit;
using GameFramework;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    void Start()
    {
        if ( _gameEntry == null )
            throw new GameFrameworkException( "GameEntry is null!" );

        Tools.SetActive( _gameEntry, true );
    }

    [SerializeField] private GameObject _gameEntry = null;
}
