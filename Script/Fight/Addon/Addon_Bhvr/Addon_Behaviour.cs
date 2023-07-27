using Aquila.Module;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Fight.Addon
{
    public class Addon_Behaviour : Addon_Base
    {
        /// <summary>
        /// 执行行为
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
        /// 添加行为
        /// </summary
        public void AddBehaviour( ActorBehaviourTypeEnum type )
        {
            var intType = ( int ) type;
            if ( _behaviourDic.ContainsKey( intType ) )
            {
                Log.Warning( $"AddonBehaviour:same behaviour,type:{type}" );
                return;
            }

            _behaviourDic.Add( intType, Gen( type, _actorInstance ) );
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.BEHAVIOUR;

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
            }
            return null;
        }
    }
}

