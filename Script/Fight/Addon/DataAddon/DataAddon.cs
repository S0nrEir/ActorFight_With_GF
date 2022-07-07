using Aquila.Fight.Actor;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 数据组件 by yhc 
    /// </summary>
    public class DataAddon : AddonBase
    {
        public override AddonTypeEnum AddonType => AddonTypeEnum.DATA;

        public override void OnAdd ()
        {
            intDataDic = new Dictionary<int, int>();
            floatDataDic = new Dictionary<int, float>();
            _objectDataDic = new Dictionary<int, object>();
        }

        public override void Dispose ()
        {
            base.Dispose();

            intDataDic?.Clear();
            intDataDic = null;

            floatDataDic?.Clear();
            floatDataDic = null;

            _objectDataDic?.Clear();
            _objectDataDic = null;
        }

        public override uint Valid ()
        {
            if (
                intDataDic.Count == 0 ||
                floatDataDic.Count == 0 ||
                _objectDataDic.Count == 0 ||
                _stringDataDic.Count == 0
                )
            {
                return AddonValidErrorCodeEnum.ZERO_DATA_COUNT;
            }
            return AddonValidErrorCodeEnum.NONE;
        }

        public override void Reset ()
        {
            base.Reset();
            //intDataDic?.Clear();
            //floatDataDic?.Clear();
            //_objectDataDic?.Clear();
        }

        public override void Init ( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );
        }

        /// <summary>EN
        /// 获取int型的字段，获取失败返回默认值
        /// </summary>
        public int GetIntDataValue ( int type ,int defaultValue = 0)
        {
            if (intDataDic is null || intDataDic.Count == 0)
                return defaultValue;

            if (!intDataDic.TryGetValue( (int)type, out var value ))
                return defaultValue;

            return value;
        }

        /// <summary>
        /// 获取int型的字段，获取失败返回默认值
        /// </summary>
        public int GetIntDataValue ( DataAddonFieldTypeEnum type, int defaultValue = 0)
        {
            return GetIntDataValue( (int)type, defaultValue );
        }

        /// <summary>
        /// 设置int型的字段，如果是没有的字段则会自动添加，已有的覆盖
        /// </summary>
        public bool SetIntDataValue ( int type, int value )
        {
            if (intDataDic is null)
                return false;

            var intType = (int)type;
            if (!intDataDic.ContainsKey( intType ))
            {
                intDataDic.Add( intType, value );
                return true;
            }

            //已有的直接覆盖
            intDataDic[intType] = value;
            return true;
        }

        /// <summary>
        /// 设置int型的字段，如果是没有的字段则会自动添加，已有的覆盖
        /// </summary>
        public bool SetIntDataValue ( DataAddonFieldTypeEnum type, int value )
        {
            return  SetIntDataValue( (int)type, value );
        }

        /// <summary>
        /// 获取float型的字段，获取失败返回默认值
        /// </summary>
        public float GetFloatDataValue ( int type, float defaultValue = 0)
        {
            if (floatDataDic is null || floatDataDic.Count == 0)
                return defaultValue;

            if (!floatDataDic.TryGetValue( (int)type, out var value ))
                return defaultValue;

            return value;
        }

        /// <summary>
        /// 获取float型的字段，获取失败返回默认值
        /// </summary>
        public float GetFloatDataValue ( DataAddonFieldTypeEnum type, float defaultValue = 0)
        {
            return GetFloatDataValue( (int)type, defaultValue );
        }

        /// <summary>
        /// 设置float型的字段，如果是没有的字段则会自动添加，已有的覆盖
        /// </summary>
        public bool SetFloatDataValue ( int type, float value )
        {
            if (floatDataDic is null)
                return false;

            var intType = (int)type;
            if (!floatDataDic.ContainsKey( intType ))
            {
                floatDataDic.Add( intType, value );
                return true;
            }

            //已有的直接覆盖
            floatDataDic[intType] = value;
            return true;
        }

        /// <summary>
        /// 设置float型的字段，如果是没有的字段则会自动添加，已有的覆盖
        /// </summary>
        public bool SetFloatDataValue ( DataAddonFieldTypeEnum type, float value )
        {
            return SetFloatDataValue( (int)type, value );
        }

        /// <summary>
        /// 设置object型的字段，如果是没有的字段则会自动添加，已有的覆盖
        /// </summary>
        public bool SetObjectDataValue ( DataAddonFieldTypeEnum type, object objData )
        {
            if (_objectDataDic is null)
                return false;

            var intType = (int)type;
            if (!_objectDataDic.ContainsKey( intType ))
            {
                _objectDataDic.Add( intType, objData );
                return true;
            }

            //已有的直接覆盖
            _objectDataDic[intType] = objData;
            return true;
        }

        /// <summary>
        /// 获取object型的字段，获取失败返回null
        /// </summary>
        public T GetObjectDataValue<T> ( DataAddonFieldTypeEnum type ) where T : class , new()
        {
            if (_objectDataDic is null || _objectDataDic.Count == 0)
                return null;

            if (!_objectDataDic.TryGetValue( (int)type, out var value ))
                return null;

            return value as T;
        }

        public override void SetEnable ( bool enable )
        {
            _enable = enable;
        }


        /// <summary>
        /// int型数据字段集合
        /// </summary>
        private Dictionary<int, int> intDataDic;

        /// <summary>
        /// float型字段数据集合
        /// </summary>
        private Dictionary<int, float> floatDataDic;

        /// <summary>
        /// object数据字段集合
        /// </summary>
        private Dictionary<int, object> _objectDataDic;

        /// <summary>
        /// string数据字段集合
        /// </summary>
        private Dictionary<int, string> _stringDataDic;
    }

    /// <summary>
    /// 数据字段定义类型
    /// </summary>
    public enum DataAddonFieldTypeEnum
    {
        /// <summary>
        /// 血量上限
        /// </summary>
        INT_MAX_HP = 1,

        /// <summary>
        /// 当前血量
        /// </summary>
        INT_CURR_HP,

        /// <summary>
        /// 当前血量
        /// </summary>
        INT_CURR_MP,

        /// <summary>
        /// 当前血量
        /// </summary>
        INT_MAX_MP,

        /// <summary>
        /// 移速
        /// </summary>
        INT_MOVE_SPEED,

        /// <summary>
        /// 攻
        /// </summary>
        INT_ACK,

        /// <summary>
        /// 防
        /// </summary>
        INT_DEF,

        /// <summary>
        /// 护盾
        /// </summary>
        INT_SHIELD,

        /// <summary>
        /// 表id
        /// </summary>
        INT_META_ID,

        /// <summary>
        /// 警戒范围
        /// </summary>
        FLOAT_ALERT_RADIUS,

        /// <summary>
        /// 伤害半径
        /// </summary>
        FLOAT_RADIUS,

        /// <summary>
        /// ROLEBase表数据
        /// </summary>
        OBJ_META_ROLEBASE,
    }
}

