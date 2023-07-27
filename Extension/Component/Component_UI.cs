using Aquial.UI;
using Aquila.UI;
using GameFramework;
using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// UI模块
    /// </summary>
    public class Component_UI : GameFrameworkComponent
    {
        //--------------------------- pub ---------------------------

        /// <summary>
        /// 打开
        /// </summary>
        public void OpenForm( FormIdEnum id ,object userData = null)
        {
            OpenForm( ( int ) id ,userData);
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void OpenForm( int formID,object userData = null)
        {
            IDataTable<DRUIForm> table = GameEntry.DataTable.GetDataTable<DRUIForm>();
            var row = table.GetDataRow( formID );
            if ( row is null )
            {
                Log.Warning( $"<color=yellow>Component_UI.Open()--->row is null,id:{formID}</color>" );
                return;
            }

            if ( !row.AllowMultiInstance )
            {
                if ( _uiComp.IsLoadingUIForm( row.Id ) || _uiComp.HasUIForm( row.Id ) )
                {
                    Log.Warning( $"_uiComp.IsLoadingUIForm( row.Id ) || _uiComp.HasUIForm( row.Id ),id:{row.Id} " );
                    return;
                }
            }

            var param = ReferencePool.Acquire<FormParam>();
            param._formTable = row;
            param._userData = userData;
            _uiComp.OpenUIForm( row.AssetName, row.UIGroupName, row.PauseCoveredUIForm, param );
        }

        public void CloseForm( FormIdEnum id )
        {
            CloseForm( ( int ) id );
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void CloseForm( int formID)
        {
            IDataTable<DRUIForm> table = GameEntry.DataTable.GetDataTable<DRUIForm>();
            var row = table.GetDataRow( formID );
            if ( row is null )
            {
                Log.Warning( $"<color=yellow>Component_UI.Close()--->row is null,id:{formID}</color>" );
                return;
            }

            var form = _uiComp.GetUIForm( row.AssetName );
            if ( form is null )
            {
                Log.Warning( $"<color=yellow>Component_UI.Close()--->form is null,asset:{row.AssetName}</color>" );
                return;
            }

            _uiComp.CloseUIForm( form );
        }

        /// <summary>
        /// 关闭所有
        /// </summary>
        public void CloseAll()
        {
            _uiComp.CloseAllLoadedUIForms();
            _uiComp.CloseAllLoadingUIForms();
        }

        //--------------------------- priv ---------------------------

        private void Start()
        {
            _uiComp = GameEntry.BaseUI;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public UIComponent _uiComp = null;
    }
}
