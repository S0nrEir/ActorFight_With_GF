using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// Actor��sprite�����
    /// </summary>
    public class SpriteAddon : AddonBase
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
            //#todo_spriteRender�����ʼ��
        }

        public override void SetEnable( bool enable )
        {
        }

        /// <summary>
        /// ����ͼ��Ⱦ��
        /// </summary>
        private SpriteRenderer _sprite_renderer = null;
    }
}