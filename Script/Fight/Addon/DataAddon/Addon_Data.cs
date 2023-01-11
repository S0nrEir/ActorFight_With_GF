using Aquila.Fight.Actor;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 数据组件 by yhc 
    /// </summary>
    public class Addon_Data : AddonBase
    {
        public override AddonTypeEnum AddonType => AddonTypeEnum.DATA;

        public override void OnAdd ()
        {
            _numric_data_dic = new Dictionary<int, float>();
            _object_data_dic = new Dictionary<int, object>();
        }

        public override void Dispose ()
        {
            base.Dispose();

            _numric_data_dic?.Clear();
            _numric_data_dic = null;

            _object_data_dic?.Clear();
            _object_data_dic = null;
        }

        public override uint Valid ()
        {
            if (
                _numric_data_dic.Count == 0 ||
                _object_data_dic.Count == 0 /*|| _stringDataDic.Count == 0*/
                )
            {
                return AddonValidErrorCodeEnum.ZERO_DATA_COUNT;
            }
            return AddonValidErrorCodeEnum.NONE;
        }

        public override void Reset ()
        {
            base.Reset();
        }

        public override void Init ( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );
        }

        /// <summary>
        /// 获取数字型的数据字段，获取失败返回默认值
        /// </summary>
        public float GetNumricValue ( int type, float defaultValue = 0)
        {
            if (_numric_data_dic is null || _numric_data_dic.Count == 0)
                return defaultValue;

            if (!_numric_data_dic.TryGetValue( (int)type, out var value ))
                return defaultValue;

            return value;
        }

        /// <summary>
        /// 获取数字型的数据字段，获取失败返回默认值
        /// </summary>
        public float GetNumricValue ( DataAddonFieldTypeEnum type, float defaultValue = 0)
        {
            return GetNumricValue( (int)type, defaultValue );
        }

        /// <summary>
        /// 设置数字型的数据字段，如果是没有的字段则会自动添加，已有的覆盖
        /// </summary>
        public bool SetNumricValue ( int type, float value )
        {
            if (_numric_data_dic is null)
                return false;

            var intType = (int)type;
            if (!_numric_data_dic.ContainsKey( intType ))
            {
                _numric_data_dic.Add( intType, value );
                return true;
            }

            //已有的直接覆盖
            _numric_data_dic[intType] = value;
            return true;
        }

        /// <summary>
        /// 设置float型的字段，如果是没有的字段则会自动添加，已有的覆盖
        /// </summary>
        public bool SetNumricValue( DataAddonFieldTypeEnum type, float value )
        {
            return SetNumricValue( (int)type, value );
        }

        /// <summary>
        /// 设置object型的字段，如果是没有的字段则会自动添加，已有的覆盖
        /// </summary>
        public bool SetObjectDataValue ( DataAddonFieldTypeEnum type, object objData )
        {
            if (_object_data_dic is null)
                return false;

            var intType = (int)type;
            if (!_object_data_dic.ContainsKey( intType ))
            {
                _object_data_dic.Add( intType, objData );
                return true;
            }

            //已有的直接覆盖
            _object_data_dic[intType] = objData;
            return true;
        }

        /// <summary>
        /// 获取object型的字段，获取失败返回null
        /// </summary>
        public T GetObjectDataValue<T> ( DataAddonFieldTypeEnum type ) where T : class , new()
        {
            if (_object_data_dic is null || _object_data_dic.Count == 0)
                return null;

            if (!_object_data_dic.TryGetValue( (int)type, out var value ))
                return null;

            return value as T;
        }

        public override void SetEnable ( bool enable )
        {
            _enable = enable;
        }

        /// <summary>
        /// 数字型字段数据集合
        /// </summary>
        private Dictionary<int, float> _numric_data_dic;

        /// <summary>
        /// object数据字段集合
        /// </summary>
        private Dictionary<int, object> _object_data_dic;
    }

    /// <summary>
    /// 数据字段定义类型
    /// </summary>
    public enum DataAddonFieldTypeEnum
    {
        /// <summary>
        /// 血量上限
        /// </summary>
        NUM_MAX_HP = 1,

        /// <summary>
        /// 当前血量
        /// </summary>
        NUM_CURR_HP,

        /// <summary>
        /// 当前血量
        /// </summary>
        NUM_CURR_MP,

        /// <summary>
        /// 当前血量
        /// </summary>
        NUM_MAX_MP,

        /// <summary>
        /// 移速
        /// </summary>
        NUM_MOVE_SPEED,

        /// <summary>
        /// 攻
        /// </summary>
        NUM_ACK,

        /// <summary>
        /// 防
        /// </summary>
        NUM_DEF,

        /// <summary>
        /// 护盾
        /// </summary>
        NUM_SHIELD,

        /// <summary>
        /// 表id
        /// </summary>
        NUM_META_ID,

        /// <summary>
        /// 警戒范围
        /// </summary>
        NUM_ALERT_RADIUS,

        /// <summary>
        /// 伤害半径
        /// </summary>
        NUM_RADIUS,

        /// <summary>
        /// ROLEBase表数据
        /// </summary>
        OBJ_META_ROLEBASE,
    }
}

