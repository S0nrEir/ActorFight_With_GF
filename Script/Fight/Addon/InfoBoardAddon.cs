using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 信息面板组件
    /// </summary>
    public class InfoBoardAddon : AddonBase
    {
        #region override
        public override AddonTypeEnum AddonType => AddonTypeEnum.INFO_BOARD;

        public override void OnAdd()
        {
        }

        public override void SetEnable( bool enable )
        {
        }
        #endregion
    }
}
