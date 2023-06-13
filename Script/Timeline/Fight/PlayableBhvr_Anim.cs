using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityGameFramework.Runtime;

namespace Aquila.Timeline
{
    public class PlayableBhvr_Anim : PlayableBehaviour
    {
        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
            if ( string.IsNullOrEmpty( _animName ) )
                Log.Warning( "<color=yellow>PlaybleBhvr_Anim.OnGraphStart()--->string.IsNullOrEmpty( _animName ) </color>" );

            if(_director == null)
                Log.Warning( "<color=yellow>PlaybleBhvr_Anim.OnGraphStart()--->_director == null </color>" );

            //_director.Play();
            //#todo暂时先用animator模拟，没时间整playable
            _animator.Play( Animator.StringToHash( _animName ) );
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
        }

        public override void OnGraphStop( Playable playable )
        {
            base.OnGraphStop( playable );
            _director = null;
            _animator = null;
            _animName = string.Empty;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
        }

        /// <summary>
        /// 暂时先用animator。还没时间做playable相关的东西
        /// </summary>
        public Animator _animator = null;

        /// <summary>
        /// director
        /// </summary>
        public PlayableDirector _director = null;

        /// <summary>
        /// 动画名称
        /// </summary>
        public string _animName = string.Empty;
    }
}