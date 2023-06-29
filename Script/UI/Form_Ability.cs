using System;
using System.Collections;
using System.Collections.Generic;
using Aquila.Module;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Aquila.UI
{
    /// <summary>
    /// 技能form
    /// </summary>
    public class Form_Ability : UIFormLogic
    {
        /// <summary>
        /// 初始化缓存item
        /// </summary>
        private void InitItem()
        {
            _iconItemDic = new Dictionary<int, AbilityIconItem>();
            Tools.SetActive(_tempGameObejct,false);
            var len = _abilityIdArr.Length;
            GameObject generated = null;
            AbilityIconItem tempItem = null;
            for (int i = 0; i < len; i++)
            {
                var id = _abilityIdArr[i];
                generated = GameObject.Instantiate(_tempGameObejct);
                generated.transform.SetParent(_grid.transform);
                generated.transform.localScale = Vector3.one;
                generated.transform.eulerAngles = Vector3.zero;
                tempItem = ReferencePool.Acquire<AbilityIconItem>();
                tempItem.Setup(generated,id);
                _iconItemDic.Add(id,tempItem);
            }
        }

        /// <summary>
        /// 清掉item缓存
        /// </summary>
        private void ClearItemCacheDic()
        {
            if(_iconItemDic is null || _iconItemDic.Count == 0)
                return;

            AbilityIconItem temp = null;
            var iter = _iconItemDic.GetEnumerator();
            while (iter.MoveNext())
            {
                temp = iter.Current.Value;
                ReferencePool.Release(temp);
            }

            _iconItemDic.Clear();
            _iconItemDic = null;
        }

        private void Update()
        {
            
        }

        protected override void OnReveal()
        {
            base.OnReveal();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            var param = userData as Form_AbilityParam;
            if (param is null)
                return;

            _actorID         = param._mainActorID;
            _enemyActorIdArr = param._enemyActorID;
            _abilityIdArr    = param._abilityID;
            _actorProxy      = GameEntry.Module.GetModule<Module_ProxyActor>();
            InitItem();
            ReferencePool.Release(param);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _actorID = -1;
            _actorProxy = null;
            ClearItemCacheDic();
            base.OnClose(isShutdown, userData);
        }

        /// <summary>
        /// 模板
        /// </summary>
        [SerializeField] private GameObject _tempGameObejct = null;

        /// <summary>
        /// grid
        /// </summary>
        [SerializeField] private GridLayoutGroup _grid = null;
        
        /// <summary>
        /// actor模块
        /// </summary>
        private Module_ProxyActor _actorProxy = null;
        
        /// <summary>
        /// 要操作的actorID
        /// </summary>
        private int _actorID = -1;

        /// <summary>
        /// 敌人actorID
        /// </summary>
        private int[] _enemyActorIdArr = null;
        
        /// <summary>
        /// 技能ID
        /// </summary>
        private int[] _abilityIdArr = null;
        /// <summary>
        /// item缓存
        /// </summary>
        private Dictionary<int, AbilityIconItem> _iconItemDic = null;

        /// <summary>
        /// 图标item
        /// </summary>
        private class AbilityIconItem : IReference
        {
            public void Setup(GameObject go,int abilityID)
            {
                _root = go;
                _abilityID = abilityID;
                
                
            }

            public void Clear()
            {
                _root          = null;
                _abilityID     = -1;
                _cd            = null;
                _text          = null;
                _abilityIdText = null;
            }
            
            private Text _abilityIdText = null;
            private Text _text = null;
            private Image _cd = null;
            
            /// <summary>
            /// 技能ID
            /// </summary>
            private int _abilityID = -1;
            
            /// <summary>
            /// 根节点
            /// </summary>
            private GameObject _root = null;
        }
    }
    
    /// <summary>
    /// 界面参数
    /// </summary>
    public class Form_AbilityParam : IReference
    {
        public int _mainActorID = -1;
        public int[] _enemyActorID = null;
        public int[] _abilityID = null;
        
        public void Clear()
        {
            _mainActorID = -1;
            _enemyActorID = null;
            _abilityID = null;
        }
    }
}
