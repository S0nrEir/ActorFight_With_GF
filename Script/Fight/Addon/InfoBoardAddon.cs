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
        public void SetHPFlagDone()
        {
            HPBarInitFlag = true;
        }

        /// <summary>
        /// 隐藏所有
        /// </summary>
        public void HideAll()
        {
            Log.Info( $"<color=green>HideAll:</color>,actorID:{Actor.ActorID}" );
            if ( _headInfo != null )
            {
                GameEntry.InfoBoard.HideHeadInfo( _headInfo.ObjectReference );
                _headInfo = null;
            }

            if ( _hpBarItem != null )
            {
                GameEntry.InfoBoard.HideHPBar( _hpBarItem.ObjectReference );
                _hpBarItem = null;
            }
        }

        public void ShowDamageNum( float arriveTime, string num )
        {
            GameEntry.InfoBoard.ShowDamageNum( Actor, arriveTime, num );
        }

        public void ChangeHPValue( float valueToChange, int currHP = -1 )
        {
            if ( _hpBarItem == null )
                return;

            _hpBarItem.SetValueDire( valueToChange, currHP );
            //ShowHPBarItem( true );
        }

        public void ShowHPBarItem( bool show )
        {
            //Log.Info( $"<color=green>ShowHPBarItem,</color>ActorID:{Actor.ActorID},result:{show}" );
            if ( _hpBarItem != null )
                Utils.SetActive( _hpBarItem.gameObject, show );
        }

        /// <summary>
        /// 设置护盾值
        /// </summary>
        public void SetShieldValue( int shieldValue, int currHP, int maxHP )
        {
            var total = shieldValue + currHP > maxHP ? shieldValue + currHP : maxHP;
            _hpBarItem.SetValueDire( currHP / ( float ) total );
            _hpBarItem.SetShieldValue( _hpBarItem.Value() + shieldValue / ( float ) total );
        }

        //这里写的不好，加一个类型就要写一次，以后再说
        /// <summary>
        /// 添加一个类型
        /// </summary>
        public bool AddType( ObjectPoolItemTypeEnum itemType )
        {
            switch ( itemType )
            {
                case ObjectPoolItemTypeEnum.HEAD_INFO:
                    return CreateHeadInfo();

                case ObjectPoolItemTypeEnum.HP_BAR:
                    return CreateHPBar();

                default:
                    return false;
            }
        }

        /// <summary>
        /// 创建HeadInfo
        /// </summary>
        private bool CreateHeadInfo()
        {
            if ( !Actor.TryGetAddon<DataAddon>( out var addon ) )
                return false;

            var meta = addon.GetObjectDataValue<Tab_RoleBaseAttr>( DataAddonFieldTypeEnum.OBJ_META_ROLEBASE );
            if ( meta is null )
                return false;

            _headInfo = GameEntry.InfoBoard.ShowHeadInfo( Actor, meta.Level, meta.Name );
            Utils.SetActive( _headInfo.gameObject, true );
            var followTarget = Utils.GetComponent<UIFollowTarget>( _headInfo.gameObject );
            if ( followTarget != null )
            {
                followTarget.target = Actor.CachedTransform;
                followTarget.targetPos = Actor.CachedTransform.position;
            }
            //#TODO隐藏headInfo
            Utils.SetActive( _headInfo.gameObject, false );
            return true;
        }

        /// <summary>
        /// 创建HPBar
        /// </summary>
        private bool CreateHPBar()
        {
            _hpBarItem = GameEntry.InfoBoard.ShowHPBar( Actor, 1f, 1f );
            //Utils.SetActive( _hpBarItem.gameObject, true );
            var followTarget = Utils.GetComponent<UIFollowTarget>( _hpBarItem.gameObject );
            if ( followTarget != null )
            {
                followTarget.target = Actor.CachedTransform;
                followTarget.targetPos = Actor.CachedTransform.position;
            }
            //创建出来先隐藏
            ShowHPBarItem( false );
            return true;
        }

        #region override
        public override AddonTypeEnum AddonType => AddonTypeEnum.HEALTH_BAR;

        public override void OnAdd()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Reset()
        {
            base.Reset();
            //if ( _hpBarItem != null )
            //    _hpBarItem.SetCurrHP( -1 );

            _hpBarItem = null;
            _headInfo = null;

            HPBarInitFlag = false;
        }

        public override void Init( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );

            Reset();
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }

        public override void SetEnable( bool enable )
        {
            //if (_hpBarItem.gameObject == null)
            //    return;

            ShowHPBarItem( false );
        }

        public float Value => _hpBarItem is null ? 0f : _hpBarItem.Value();

        #endregion

        //#todo对象池类型用统一的集合管理，用泛型处理逻辑
        private HPBarItem _hpBarItem = null;
        private HeadInfoObjectItem _headInfo = null;

        public bool HPBarInitFlag { get; private set; } = false;

    }


}
