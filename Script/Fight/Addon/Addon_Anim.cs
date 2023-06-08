using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Toolkit;
using UnityEngine;
using UnityEngine.Playables;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 动画addon by yhc
    /// </summary>
    public class Addon_Anim : Addon_Base
    {
        public override AddonTypeEnum AddonType => AddonTypeEnum.ANIM;

        public override void OnAdd ()
        {
            CurrClipName = string.Empty;
        }

        public override void Init ( TActorBase actor, GameObject target_go, Transform target_transform )
        {
            base.Init( actor, target_go, target_transform );
            //动画机是挂在GameObject上的,制作GameObject的时候手动加上去，这里只尝试获取
            _animator = Tools.GetComponent<Animator>( Actor.gameObject );

            if ( _animator == null )
                Debug.LogError( "<color=red>faild to get animator</color>" );
        }

        //
        //#todo动态设置playableAseet？
        public override void Init( Module_Proxy_Actor.ActorInstance instance )
        {
            base.Init( instance );
        }

        public override void Dispose ()
        {
            base.Dispose();
            _animator = null;
        }

        public override void Reset ()
        {
            base.Reset();
            // PlayStandAnim();
        }

        /// <summary>
        /// play指定动画
        /// </summary>
        public bool Play (string clipName)
        {
            //Debug.Log( $"<color=white>Actor{Actor.ActorID}.Play()---->clipName:{clipName}</color>" );
            if (string.IsNullOrEmpty( clipName ))
            {
                Debug.LogError( "string.IsNullOrEmpty( clipName )" );
                return false;
            }

            if (_animator == null)
                return false;
            
            _animator.SetBool(CurrClipName,false);
            _animator.SetBool( clipName ,true);
            CurrClipName = clipName;
            return true;
            // var len = AnimClipNameArr.Length;
            // var nameToHash = Animator.StringToHash( clipName );
            // var succFlag = false;
            // bool matchFlag;
            // for (int i = 0; i < len; i++)
            // {
            //     matchFlag = AnimClipNameArr[i] == nameToHash;
            //     //这里有个坑，为什么要用SetBool的方法？
            //     _animator.SetBool( AnimClipNameArr[i], matchFlag );
            //     if(matchFlag)
            //         succFlag = matchFlag;
            // }
            //
            // //给的clipName不匹配任何已指定的clipName
            // if (!succFlag)
            //     Debug.LogError( $"clip {clipName} dosent match any exist clipArr" );
            // else
            //     CurrClipName = clipName;
            //
            // return succFlag;
        }

        /// <summary>
        /// 当前的动画名称
        /// </summary>
        public string CurrClipName { get; private set; } = string.Empty;

        /// <summary>
        /// 动画机
        /// </summary>
        private Animator _animator;

        /// <summary>
        /// 动画片段名称，要求名称统一
        /// </summary>
        private static readonly int[] AnimClipNameArr = new int[]
        {
            Animator.StringToHash("Idle"),//待机
            Animator.StringToHash("Ability"),//技能
            Animator.StringToHash("Walk"),//行走
        };
    }
}


