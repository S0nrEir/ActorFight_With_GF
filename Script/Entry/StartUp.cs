using Aquila;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    void Start()
    {
        if ( _entryObject == null )
            throw new GameFrameworkException( "GameEntry is null!" );

        Tools.SetActive( _entryObject, true );
        DontDestroyOnLoad( _eventSystem );
    }

    [SerializeField] private GameObject _entryObject = null;
    [SerializeField] private GameObject _eventSystem = null;
}
