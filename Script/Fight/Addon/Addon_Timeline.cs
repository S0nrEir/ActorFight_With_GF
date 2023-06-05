using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Aquila.Fight.Addon
{
    public class Addon_Timeline : Addon_Base
    {
        /// <summary>
        /// 获取当前的播放状态
        /// </summary>
        public PlayState State()
        {
            return _director.state;
        }

        /// <summary>
        /// 播放一个Timeline
        /// </summary>
        public void Play( string assetPath )
        {
            GameEntry.Timeline.Play( assetPath ,_director);
        }

        public override void Init( TActorBase actor, GameObject target_go, Transform target_transform )
        {
            base.Init( actor, target_go, target_transform );
        }

        public override void Init( Module_Proxy_Actor.ActorInstance instance )
        {
            base.Init( instance );
            _director = Tools.GetComponent<PlayableDirector>( instance.Actor.transform );
            if ( _director == null )
                return;

            _director.playOnAwake = false;
        }

        public override void OnAdd()
        {
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.TIMELINE;

        /// <summary>
        /// 持有的播放组件
        /// </summary>
        private PlayableDirector _director = null;
    }
}

