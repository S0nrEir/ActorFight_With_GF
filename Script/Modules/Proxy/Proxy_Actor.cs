using Aquila.Extension;
using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// Actor�����࣬���ⲿ���÷��ṩ��ע���Actor
/// </summary>
public class Proxy_Actor : GameFrameworkModuleBase
{
    /// <summary>
    /// ��actorע�ᵽ�����У��ɹ�����true
    /// </summary>
    public bool Register( TActorBase actor, AddonBase[] addons )
    {
        if ( actor is null )
        {
            Log.Warning( "<color=yellow>actor is null.</color>" );
            return false;
        }

        if ( Contains( actor.ActorID ) )
        {
            Log.Warning( $"<color=yellow>proxy has contains actor,id={actor.ActorID}</color>" );
            return false;
        }

        var actor_case = ReferencePool.Acquire<Proxy_Actor_Case>();
        actor_case.Setup( actor, addons );
        _proxy_actor_dic.Add( actor.ActorID, actor_case );

        return true;
    }

    /// <summary>
    /// ע������ʵ��
    /// </summary>
    public void UnRegister( int id )
    {
        if ( !Contains( id ) )
            Log.Warning( $"proxy doesnt have actor wich id = {id}" );

        _proxy_actor_dic.TryGetValue( id, out var actor_case );
        actor_case.Clear();
        _proxy_actor_dic.Remove( id );
    }

    /// <summary>
    /// ����
    /// </summary>
    private bool Contains( int actor_id )
    {
        return _registered_id_set.Contains( actor_id );
    }

    /// <summary>
    /// ��ʼ��Actor����
    /// </summary>
    public void InitActor()
    {
        _proxy_actor_dic = new Dictionary<int, Proxy_Actor_Case>();
        _registered_id_set = new HashSet<int>();
    }

    /// <summary>
    /// ����ʼ��Actor����
    /// </summary>
    public void DeInitActor()
    {
        var iter = _proxy_actor_dic.GetEnumerator();
        while ( iter.MoveNext() )
            iter.Current.Value.Clear();

        _proxy_actor_dic = null;

        _registered_id_set.Clear();
    }

    /// <summary>
    /// actor�������ϣ�������ս�������е�ActorProxy
    /// </summary>
    private Dictionary<int, Proxy_Actor_Case> _proxy_actor_dic;

    /// <summary>
    /// ע���ID����
    /// </summary>
    private HashSet<int> _registered_id_set = null;

    #region

    public override void Start( object param )
    {
        base.Start( param );
        InitActor();
    }

    public override void End()
    {
        DeInitActor();
        base.End();
    }

    protected override bool Contains_Sub_Module => false;

    #endregion

    /// <summary>
    /// ս������Actor�࣬��ʾActor��Proxy�еı�ʾ����װ��Actor�Ͷ�Ӧ��Addon
    /// </summary>
    private class Proxy_Actor_Case : IReference
    {
        #region pub

        public void Setup( TActorBase actor, AddonBase[] addons )
        {
            _actor = actor;
            _addon_arr = addons;
        }

        public Proxy_Actor_Case() { }

        /// <summary>
        /// ���ظ�ʵ�����е�actor
        /// </summary>
        public TActorBase Actor
        {
            get => _actor;
        }

        /// <summary>
        /// ��ȡactor���е�ָ�����͵�addon��û�з��ؿ�
        /// </summary>
        public T GetAddon<T>() where T : AddonBase
        {
            if ( _addon_arr is null || _addon_arr.Length == 0 )
                return null;

            foreach ( var addon in _addon_arr )
            {
                if ( addon is T )
                    return addon as T;
            }

            return null;
        }

        #endregion


        #region fields

        /// <summary>
        /// ���е�Actor
        /// </summary>
        private TActorBase _actor = null;

        /// <summary>
        /// actor���е�addon����
        /// </summary>
        private AddonBase[] _addon_arr = null;

        #endregion

        public void Clear()
        {
            _actor = null;
            _addon_arr = null;
        }
    }
}
