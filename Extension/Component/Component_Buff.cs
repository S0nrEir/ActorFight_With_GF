using Aquila.Fight.Actor;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Buff
{
    /// <summary>
    /// buff组件
    /// </summary>
    public class Component_Buff : GameFrameworkComponent
    {
        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 收到buff信息
        /// </summary>
        public void OnRecvImpactInfo()
        {
        }

        /// <summary>
        /// 移除buff
        /// </summary>
        public void Remove( int impactID, int objID )
        {
            if ( !_fightFlag )
                return;

            BuffBase buff = null;
            if ( !_buffDic.ContainsKey( impactID ) )
                return;

            buff = _buffDic[impactID];
            if ( !buff.Contains( objID ) )
                return;

            if ( !buff.Remove( objID, out var _ ) )
                return;
        }

        /// <summary>
        /// 添加buff
        /// </summary>
        private void Add( int impactID, int objID )
        {

        }

        /// <summary>
        /// 开始战斗buff管理
        /// </summary>
        public void StarFight()
        {
            _fightFlag = true;

            if ( _buffDic is null )
                _buffDic = new Dictionary<int, BuffBase>();
        }

        /// <summary>
        /// 关闭战斗buff管理
        /// </summary>
        public void EndFight()
        {
            _fightFlag = false;
            _buffDic?.Clear();
        }

        /// <summary>
        /// 初始化战场计时器
        /// </summary>
        private bool EnsureInitFightTimer()
        {
            if ( !_fightFlag )
                return false;

            return true;
        }

        /// <summary>
        /// 应用一个缓存buff
        /// </summary>
        public void ApplyCachedBuff( Actor_Hero actor )
        {
            if ( actor is null )
                return;

            var objID = actor.ActorID;
            //#类型转换的坑ilrt
            //找所有buff缓存中符合actorID的，然后将他添加上
            var iter = _buffDic.GetEnumerator();
            while ( iter.MoveNext() )
                iter.Current.Value.ApplyCache( actor );
        }

        /// <summary>
        /// 战场buff计时轮询
        /// </summary>
        //private void OnFightTimerTick ( float elapsed )
        //{
        //    //Log.Info($"OnFightTimerTick" );
        //}

        ///// <summary>
        ///// 初始化普通计时器
        ///// </summary>
        //private void EnsureInitNormalTimer ()
        //{

        //}

        private bool _fightFlag = false;

        /// <summary>
        /// buff集合,K = buffID,v = buffBase
        /// </summary>
        private Dictionary<int, BuffBase> _buffDic = null;
    }

}
