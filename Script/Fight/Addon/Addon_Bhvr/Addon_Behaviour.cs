using Aquila.Module;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Fight.Addon
{
    public class Addon_Behaviour : Addon_Base
    {
        /// <summary>
        /// 获取一个behaviour，拿不到返回null
        /// </summary>
        public ActorBehaviour_Base GetBehaviour( ActorBehaviourTypeEnum type )
        {
            if ( _behaviourDic.TryGetValue( ( int ) type, out var bhvr ) )
                return bhvr;

            Log.Warning( $"Addon_Behaviour.GetBehaviour()--->_behaviourDic.TryGetValue( ( int ) type, out var bhvr ) , type{type.ToString()}" );
            return null;
        }

        /// <summary>
        /// 主动执行行为
        /// </summary>
        public void Exec( ActorBehaviourTypeEnum type, object param )
        {
            var intType = ( int ) type;
            if ( !_behaviourDic.ContainsKey( intType ) )
            {
                Log.Warning( $"behaviour【{type}】 doesnt exsit" );
                return;
            }

            _behaviourDic[( int ) type].Exec( param );
        }

        /// <summary>
        /// 移除行为
        /// </summary>
        public bool RemoveBehaviour( ActorBehaviourTypeEnum type )
        {
            Debug.Log( "remove---------------------------" );
            lock ( _behaviourDic )
                return _behaviourDic.Remove( ( int ) type );
        }

        /// <summary>
        /// 添加行为
        /// </summary
        public ActorBehaviour_Base AddBehaviour( ActorBehaviourTypeEnum type )
        {
            var intType = ( int ) type;
            if ( _behaviourDic.ContainsKey( intType ) )
            {
                Log.Warning( $"AddonBehaviour:same behaviour,type:{type}" );
                return null;
            }

            var bhvr = Gen( type, _actorInstance );
            lock(_behaviourDic )
                _behaviourDic.Add( intType, bhvr );

            return bhvr;
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.BEHAVIOUR;

        public override void OnUpdate( float elapseSeconds, float realElapseSeconds )
        {
            base.OnUpdate( elapseSeconds, realElapseSeconds );
            var iter = _behaviourDic.GetEnumerator();
            while ( iter.MoveNext() )
                iter.Current.Value.Update( elapseSeconds, realElapseSeconds );
            //try
            //{
            //    var iter = _behaviourDic.GetEnumerator();
            //    while ( iter.MoveNext() )
            //        iter.Current.Value.Update( elapseSeconds, realElapseSeconds );
            //}
            //catch( System.Exception err )
            //{
            //    Debug.LogError( err.Message );
            //    Debug.Break();
            //    Debug.DebugBreak();
            //}
        }

        public override void Cancel()
        {
        }

        public override void OnAdd()
        {
            _behaviourDic = new Dictionary<int, ActorBehaviour_Base>();
        }

        public override void Init( Module_ProxyActor.ActorInstance instance )
        {
            base.Init( instance );
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Dispose()
        {
            var iter = _behaviourDic.GetEnumerator();
            while ( iter.MoveNext() )
                iter.Current.Value.Dispose();

            _behaviourDic.Clear();
            _behaviourDic = null;
            base.Dispose();
        }

        /// <summary>
        /// 保存行为集合
        /// </summary>
        private Dictionary<int, ActorBehaviour_Base> _behaviourDic = null;

        //#todo是否不用switch/case，改成别的方式
        /// <summary>
        /// 生成
        /// </summary>
        private static ActorBehaviour_Base Gen( ActorBehaviourTypeEnum type, ActorInstance ins )
        {
            switch ( type )
            {
                case ActorBehaviourTypeEnum.ABILITY:
                    return new ActorBehaviour_Ability( ins );

                case ActorBehaviourTypeEnum.DIE:
                    return new ActorBehaviour_Die( ins );

                case ActorBehaviourTypeEnum.TRACING_TRANSFORM:
                    return new ActorBehaviour_TracingTransform( ins );

                case ActorBehaviourTypeEnum.TARGETING_POSITION:
                    return new ActorBehaviour_TargetingPosition( ins );
            }
            return null;
        }
    }
}

