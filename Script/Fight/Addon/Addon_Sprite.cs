using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// Actor的sprite组件类
    /// </summary>
    public class SpriteAddon : Addon_Base
    {
        public override AddonTypeEnum AddonType => AddonTypeEnum.SPRITE;

        public override void OnAdd()
        {
            var sprite_go = Actor.transform.Find( "Sprite" );
            if ( sprite_go == null )
            {
                Log.Warning( "sprite_go == null", LogColorTypeEnum.Red );
                return;
            }
            //#todo_spriteRender组件初始化
        }
        
        /// <summary>
        /// 精灵图渲染器
        /// </summary>
        private SpriteRenderer _sprite_renderer = null;
    }
}