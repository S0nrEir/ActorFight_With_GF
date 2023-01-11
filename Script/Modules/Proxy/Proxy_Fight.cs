using Aquila;
using Aquila.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ս���������
/// </summary>
public partial class Proxy_Fight : GameFrameworkModuleBase
{
    #region pub



    #endregion


    #region fields

    /// <summary>
    /// ���е�actor����
    /// </summary>
    private Proxy_Actor _proxy_actor = null;

    #endregion

    #region override

    public override void Start( object param )
    {
        base.Start( param );
        _proxy_actor = GameEntry.Module.GetModule<Proxy_Actor>();
    }

    public override void End()
    {
        _proxy_actor = null;
        base.End();
    }

    protected override bool Contains_Sub_Module => false;

    #endregion
}
