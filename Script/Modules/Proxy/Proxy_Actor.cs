using Aquila.Extension;
using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// Actor管理类，对外部调用方提供已注册的Actor
/// </summary>
public class Proxy_Actor : GameFrameworkModuleBase
{
    /// <summary>
    /// 将actor注册到代理中，成功返回true
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
    /// 注销单个实例
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
    /// 包含
    /// </summary>
    private bool Contains( int actor_id )
    {
        return _registered_id_set.Contains( actor_id );
    }

    /// <summary>
    /// 初始化Actor部分
    /// </summary>
    public void InitActor()
    {
        _proxy_actor_dic = new Dictionary<int, Proxy_Actor_Case>();
        _registered_id_set = new HashSet<int>();
    }

    /// <summary>
    /// 反初始化Actor部分
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
    /// actor索引集合，保存了战斗中所有的ActorProxy
    /// </summary>
    private Dictionary<int, Proxy_Actor_Case> _proxy_actor_dic;

    /// <summary>
    /// 注册的ID集合
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
    /// 战斗代理Actor类，表示Actor在Proxy中的表示，封装了Actor和对应的Addon
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
        /// 返回该实例持有的actor
        /// </summary>
        public TActorBase Actor
        {
            get => _actor;
        }

        /// <summary>
        /// 获取actor持有的指定类型的addon，没有返回空
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
        /// 持有的Actor
        /// </summary>
        private TActorBase _actor = null;

        /// <summary>
        /// actor持有的addon集合
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
