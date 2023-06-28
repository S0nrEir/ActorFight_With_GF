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
        }

        /// <summary>
        /// 精灵图渲染器
        /// </summary>
#pragma warning disable IDE0051 // 删除未使用的私有成员
        private SpriteRenderer _spriteRender = null;
#pragma warning restore IDE0051 // 删除未使用的私有成员
    }
}