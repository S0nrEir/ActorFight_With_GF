using Aquila.Fight.Actor;
using Aquila.Toolkit;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 动画addon by yhc
    /// </summary>
    public class Addon_Anim : Addon_Base
    {
        #region play interface
        /// <summary>
        /// 播放死亡动画
        /// </summary>
        public bool PlayDieAnim ()
        {
            return Play( "Die" );
        }

        /// <summary>
        /// 播放站立动画
        /// </summary>
        public bool PlayStandAnim ()
        {
            return Play( "Stand" );
        }

        /// <summary>
        /// 播放移动动画
        /// </summary>
        public bool PlayRunAnim ()
        {
            return Play( "Run" );
        }

        /// <summary>
        /// 播放攻击动画
        /// </summary>
        public bool PlayAttackAnim()
        {
            return Play( "Attack1" );
        }

        /// <summary>
        /// 播放攻击1动画
        /// </summary>
        public bool PlayAttack1Anim ()
        {
            return Play( "Attack1" );
        }

        /// <summary>
        /// 播放攻击2动画
        /// </summary>
        public bool PlayAttack2Anim ()
        {
            return Play( "Attack2" );
        }

        /// <summary>
        /// 播放攻击3动画
        /// </summary>
        public bool PlayAttack3Anim ()
        {
            return Play( "Attack3" );
        }

        /// <summary>
        /// 播放技能1动画
        /// </summary>
        public bool PlaySkill1Anim ()
        {
            return Play( "Skill11" );
        }

        /// <summary>
        /// 播放技能2动画
        /// </summary>
        public bool PlaySkill2Anim ()
        {
            return Play( "Skill2" );
        }

        /// <summary>
        /// 播放技能3动画
        /// </summary>
        public bool PlaySkill3Anim ()
        {
            return Play( "Skill3" );
        }

        /// <summary>
        /// 播放技能4动画
        /// </summary>
        public bool PlaySkill4Anim ()
        {
            return Play( "Skill4" );
        }

        /// <summary>
        /// 播放技能5动画
        /// </summary>
        public bool PlaySkill5Anim ()
        {
            return Play( "Skill5" );
        }

        /// <summary>
        /// 播放技能6动画
        /// </summary>
        public bool PlaySkill6Anim ()
        {
            return Play( "Skill6" );
        }

        /// <summary>
        /// 播放被击退动画
        /// </summary>
        public bool PlayKnockBackAnim ()
        {
            return Play( "KnockBack" );
        }

        /// <summary>
        /// 播放互动动画
        /// </summary>
        public bool PlayInteractionAnim ()
        {
            return Play( "Interaction" );
        }

        public bool PlayVertigoAnim ()
        {
            return Play( "Vertigo" );
        }

        public bool PlayHitAnim ()
        {
            return Play( "Hit" );
        }


        #endregion

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

            if (_animator == null)
            {
                Debug.LogError( "<color=red>faild to get animator</color>" );
                _animator = Actor.gameObject.AddComponent<Animator>();
            }
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
        public bool Play (string clip_name)
        {
            //Debug.Log( $"<color=white>Actor{Actor.ActorID}.Play()---->clipName:{clipName}</color>" );
            if (string.IsNullOrEmpty( clip_name ))
            {
                Debug.LogError( "string.IsNullOrEmpty( clipName )" );
                return false;
            }

            if (_animator == null)
                return false;

            _animator.SetBool( clip_name ,true);
            var len = AnimClipNameArr.Length;
            var nameToHash = Animator.StringToHash( clip_name );
            var succFlag = false;
            bool matchFlag;
            for (int i = 0; i < len; i++)
            {
                matchFlag = AnimClipNameArr[i] == nameToHash;
                //这里有个坑，为什么要用SetBool的方法？
                _animator.SetBool( AnimClipNameArr[i], matchFlag );
                if(matchFlag)
                    succFlag = matchFlag;
            }

            //给的clipName不匹配任何已指定的clipName
            if (!succFlag)
                Debug.LogError( $"clip {clip_name} dosent match any exist clipArr" );
            else
                CurrClipName = clip_name;

            return succFlag;
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
            Animator.StringToHash( "Stand" ),//待机
            Animator.StringToHash( "Run" ),//移动
            //Animator.StringToHash( "Attack" ),//攻击1
            Animator.StringToHash( "Hit" ),//命中
            Animator.StringToHash( "Die" ),//死亡

            Animator.StringToHash( "Attack1" ),//攻击1
            Animator.StringToHash( "Attack2" ),//攻击2
            Animator.StringToHash( "Attack3" ),//攻击3
            Animator.StringToHash( "Skill1" ),//技能1
            Animator.StringToHash( "Skill2" ),//技能2
            Animator.StringToHash( "Skill3" ),//技能3
            Animator.StringToHash( "Skill4" ),//技能4
            Animator.StringToHash( "Skill5" ),//技能5
            Animator.StringToHash( "Skill6" ),//技能6
            Animator.StringToHash( "KnockBack" ),//击退
            Animator.StringToHash( "Interaction" ),//
            Animator.StringToHash( "Vertigo" ),//
        };
    }
}


