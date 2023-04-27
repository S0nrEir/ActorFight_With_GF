namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 战斗流程处理器by yhc
    /// </summary>
    public class Addon_Processor : Addon_Base
    {

        //    /// <summary>
        //    /// 找出一个空闲的processor，开始流程处理
        //    /// </summary>
        //    public bool StartProcess( Tab_FightProcessor processorMeta )
        //    {
        //        if ( !Enable )
        //            return false;

        //        var unit = GetReadyNode();
        //        if ( unit is null )
        //        {
        //            Debug.Log( "ProcessorUnit队列没有空闲处理器" );
        //            return false;
        //        }

        //        InitProcessorUnit( unit, processorMeta );
        //        Do( 0f );
        //        return true;
        //    }

        //    public override AddonTypeEnum AddonType => AddonTypeEnum.PROCESSOR;

        //    public override void OnAdd()
        //    {
        //    }

        //    //public override void OnUpdate ( float elapseSeconds, float realElapseSeconds )
        //    //{
        //    //    //base.OnUpdate( elapseSeconds, realElapseSeconds );
        //    //    Do( elapseSeconds );
        //    //}

        //    public override void Init( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        //    {
        //        base.Init( actor, targetGameObject, targetTransform );
        //        //_processorUnitArr = new ProcessorUnit[PROCESSOR_UNIT_MAX_COUNT];
        //        InitProcessorUnitArr();
        //    }

        //    public override void Dispose()
        //    {
        //        base.Dispose();
        //        if ( _processorUnitArr != null )
        //        {
        //            foreach ( var unit in _processorUnitArr )
        //                unit.Dispose();
        //        }
        //        _processorUnitArr = null;
        //    }

        //    public override void Reset()
        //    {
        //        base.Reset();
        //        if ( _processorUnitArr == null )
        //            InitProcessorUnitArr();

        //        foreach ( var unit in _processorUnitArr )
        //            unit?.Reset();
        //    }

        //    private void InitProcessorUnitArr()
        //    {
        //        _processorUnitArr = new ProcessorUnit[PROCESSOR_UNIT_MAX_COUNT];
        //        var len = _processorUnitArr.Length;
        //        for ( int i = 0; i < len; i++ )
        //            _processorUnitArr[i] = new ProcessorUnit();
        //    }

        //    /// <summary>
        //    /// 找到一个处于准备状态中的TriggerNode
        //    /// </summary>
        //    private ProcessorUnit GetReadyNode()
        //    {
        //        foreach ( var unit in _processorUnitArr )
        //        {
        //            if ( unit.Ready )
        //                return unit;
        //        }

        //        return null;
        //    }

        //    /// <summary>
        //    /// 初始化一个ProcessorUnit
        //    /// </summary>
        //    private void InitProcessorUnit( ProcessorUnit unit, Tab_FightProcessor processorMeta )
        //    {
        //        unit.Setup( processorMeta, Actor.CachedTransform );
        //    }

        //    private void Do( float elapsedTime )
        //    {
        //        if ( _processorUnitArr is null )
        //        {
        //            Debug.LogError( "ProcessorUniArr is null" );
        //            return;
        //        }

        //        foreach ( var unit in _processorUnitArr )
        //        {
        //            if ( !unit.Ready )
        //                continue;

        //            unit.Next( elapsedTime );
        //        }
        //    }

        //    public override void SetEnable( bool enable )
        //    {
        //        _enable = enable;
        //    }

        //    /// <summary>
        //    /// 准备状态
        //    /// </summary>
        //    public bool Ready = false;

        //    /// <summary>
        //    /// 当前的处理队列
        //    /// </summary>
        //    private ProcessorUnit[] _processorUnitArr;

        //    /// <summary>
        //    /// 单位队列的最大数量上限，就三个，简单的数组就可以，不用太麻烦
        //    /// </summary>
        //    private const int PROCESSOR_UNIT_MAX_COUNT = 0x3;
        //}

        ///// <summary>
        ///// 战斗流程处理单元，描述每一个战斗流程
        ///// </summary>
        //internal class ProcessorUnit
        //{
        //    /// <summary>
        //    /// 准备状态
        //    /// </summary>
        //    public bool Ready => _triggerList.Count == 0;

        //    /// <summary>
        //    /// 进行当前队列的下一个战斗表现，成功返回true，失败返回false
        //    /// </summary>
        //    private bool DoNextAction( float elapsedTime )
        //    {
        //        _passedTime += elapsedTime;
        //        var currNode = _triggerList[_currTimelineStep];
        //        if ( _passedTime >= currNode.TriggerTime )
        //        {
        //            currNode.OnStartTimesUp( GetEffectParentTransform() );
        //            _currTimelineStep++;
        //            return true;
        //        }

        //        return false;
        //    }

        //    public bool Next( float elapsedTime )
        //    {
        //        if ( !Ready )
        //            return false;

        //        OnAllNodeFadeOut();
        //        if ( DoNextAction( elapsedTime ) )
        //        {
        //            //走到尾部，没有可触发的了
        //            if ( _currTimelineStep == _triggerList.Count - 1 )
        //                Reset();

        //            return true;
        //        }

        //        return false;
        //    }

        //    /// <summary>
        //    /// 轮询处理消逝状态
        //    /// </summary>
        //    private void OnAllNodeFadeOut()
        //    {
        //        foreach ( var node in _triggerList )
        //        {
        //            if ( node.FadeOutTime >= _passedTime )
        //                node.OnEndTimesUp();
        //        }
        //    }

        //    public void Setup( Tab_FightProcessor meta, Transform effectStartTransform )
        //    {
        //        if ( meta is null )
        //        {
        //            Debug.LogError( "processor meta is null at ProcessorUnit" );
        //            return;
        //        }
        //        _processorID = meta.Id;
        //        Reset();
        //        _effectStartTransform = effectStartTransform;

        //        var cnt = meta.getTriggerTimeLineCount();

        //        var validTimelineCount = 0;
        //        float triggerTime = 0f;
        //        ProcessorTriggerNode node;
        //        //把有效的时间点加进去
        //        for ( var i = 0; i < cnt; i++ )
        //        {
        //            triggerTime = meta.GetTriggerTimeLinebyIndex( i );
        //            if ( triggerTime < 0 )
        //                continue;

        //            validTimelineCount++;
        //            node = new ProcessorTriggerNode();
        //            node.TriggerTime = triggerTime;
        //            _triggerList.Add( node );
        //        }
        //        //之后分别处理startTime、endTime并对应到相应的时间节点下标
        //        SetNodeInfo( meta );
        //    }

        //    /// <summary>
        //    /// 返回特效初始节点或触发节点，如果拿不到则始终返回触发节点
        //    /// </summary>
        //    private Transform GetEffectParentTransform()
        //    {
        //        if ( _currTimelineStep == 0 )
        //            return _effectStartTransform;

        //        if ( _effectMainTransform != null )
        //            return _effectMainTransform;

        //        var parentObj = _triggerList[1]._effectObject;
        //        if ( parentObj != null )
        //            _effectMainTransform = parentObj.transform;

        //        if ( _effectMainTransform != null )
        //            return _effectMainTransform;

        //        return _effectStartTransform;
        //    }

        //    /// <summary>
        //    /// 设置node的特效，消逝时间,开始的触发特效，也用timeline触发
        //    /// </summary>
        //    private void SetNodeInfo( Tab_FightProcessor meta )
        //    {
        //        //var pathEffectCount = meta.getSubEffectPathCount();
        //        //var startCount = meta.getSubEffectStartTimeCount();
        //        var endCount = meta.getSubEffectFadeOutTimeCount();

        //        //检查表列的一致性
        //        if (
        //                meta.getSubEffectPathCount() != meta.getSubEffectStartTimeCount() ||
        //                meta.getSubEffectStartTimeCount() != meta.getSubEffectFadeOutTimeCount()
        //           )
        //        {
        //            Debug.LogError( "pathEffectCount != startCount || endCount != startCount" );
        //            return;
        //        }

        //        //var startTimeIndex = -1;
        //        var endTimeIndex = -1;
        //        for ( var i = 0; i < endCount; i++ )
        //        {
        //            //effectPaht = meta.GetSubEffectPathbyIndex( i );
        //            //startTimeIndex = meta.GetSubEffectStartTimebyIndex( i );
        //            endTimeIndex = meta.GetSubEffectFadeOutTimebyIndex( i );

        //            if (/*string.IsNullOrEmpty( effectPaht ) || startTimeIndex < 0 || */endTimeIndex < 0 )
        //                continue;

        //            if ( i >= _triggerList.Count )
        //            {
        //                Debug.LogError( "i >= _triggerList.Count" );
        //                continue;
        //            }

        //            //因为特效信息是跟时间点走的，所以这里只能分开写
        //            //_triggerList[i]._effectPath = effectPaht;
        //            _triggerList[i].FadeOutTime = meta.GetSubEffectFadeOutTimebyIndex( endTimeIndex );
        //            //_triggerList[i]._effectPath = meta.GetSubEffectPathbyIndex()
        //        }

        //        //按触发时间由先到后进行排列
        //        _triggerList.Sort( ( x, y ) =>
        //        {
        //            return x.TriggerTime > y.TriggerTime ? -1 : 1;
        //        } );

        //        //特效按时间顺序加进去
        //        string effectPath = string.Empty;
        //        var cnt = _triggerList.Count;
        //        for ( var i = 0; i < cnt && i < cnt; i++ )
        //        {
        //            effectPath = meta.GetSubEffectPathbyIndex( i );
        //            if ( string.IsNullOrEmpty( effectPath ) )
        //                continue;

        //            _triggerList[i]._effectPath = meta.GetSubEffectPathbyIndex( i );
        //        }

        //    }

        //    public ProcessorUnit()
        //    {
        //        _triggerList = new List<ProcessorTriggerNode>();
        //        _currTimelineStep = 0;
        //    }

        //    /// <summary>
        //    /// processorID
        //    /// </summary>
        //    public int ProcessorID => _processorID;

        //    public void Dispose()
        //    {
        //        _processorID = -1;
        //        if ( _triggerList != null )
        //        {
        //            foreach ( var node in _triggerList )
        //                node.Dispose();
        //        }
        //        Reset();
        //        _triggerList = null;
        //    }

        //    /// <summary>
        //    /// 清掉触发列表
        //    /// </summary>
        //    public void Reset()
        //    {
        //        if ( _triggerList != null )
        //            _triggerList.Clear();

        //        _currTimelineStep = 0;
        //        _passedTime = 0f;
        //        _effectMainTransform = null;
        //        _effectStartTransform = null;
        //    }

        //    private int _processorID = -1;
        //    private List<ProcessorTriggerNode> _triggerList = null;
        //    /// <summary>
        //    /// 当前保存的时间节点下标位置
        //    /// </summary>
        //    private int _currTimelineStep = 0;

        //    /// <summary>
        //    /// 经过时间
        //    /// </summary>
        //    private float _passedTime = 0f;

        //    /// <summary>
        //    /// 特效的主节点
        //    /// </summary>
        //    private Transform _effectMainTransform = null;

        //    /// <summary>
        //    /// 触发主节点
        //    /// </summary>
        //    private Transform _effectStartTransform = null;
        //}

        ///// <summary>
        ///// 流程单元每时间点的触发节点
        ///// </summary>
        //internal class ProcessorTriggerNode
        //{
        //    /// <summary>
        //    /// 触发世家年开始
        //    /// </summary>
        //    public void OnStartTimesUp( Transform parentTransform )
        //    {
        //        if ( string.IsNullOrEmpty( _effectPath ) )
        //        {
        //            Debug.LogError( "string.IsNullOrEmpty( _effectPath )" );
        //            return;
        //        }

        //        var path = _effectPath.Split( '/' );
        //        if ( path == null || path.Length == 0 )
        //        {
        //            Debug.LogError( "path == null || path.Length == 0" );
        //            return;
        //        }

        //        var endPathArr = path[path.Length - 1].Split( '.' );
        //        var endPath = endPathArr[0];
        //        if ( string.IsNullOrEmpty( endPath ) )
        //        {
        //            Debug.LogError( "string.IsNullOrEmpty( endPath )" );
        //            return;
        //        }

        //        EffectObjName = endPath;
        //        _effectObject = EffectManager.Instance.GetEffect( EffectObjName, _effectPath );
        //        var tran = _effectObject.transform;
        //        tran.SetParent( parentTransform );
        //        tran.localPosition = Vector3.zero;
        //        tran.localScale = Vector3.one;
        //    }

        //    /// <summary>
        //    /// 到了消逝触发时间节点
        //    /// </summary>
        //    public void OnEndTimesUp()
        //    {
        //        if ( _effectObject == null )
        //            return;

        //        Utils.SetActive( _effectObject, false );

        //        if ( string.IsNullOrEmpty( EffectObjName ) )
        //            return;

        //        EffectManager.Instance.Recycle( EffectObjName, _effectObject );
        //    }

        //    public void Setup( float triggerTime, float fadeOutTime, string effectPath )
        //    {
        //        TriggerTime = triggerTime;
        //        FadeOutTime = fadeOutTime;
        //        _effectPath = effectPath;
        //    }

        //    public void Dispose()
        //    {
        //        TriggerTime = 0f;
        //        DamageNumber = 0;
        //        FadeOutTime = -1f;
        //        _effectObject = null;
        //    }

        //    /// <summary>
        //    /// 触发时间
        //    /// </summary>
        //    public float TriggerTime;

        //    /// <summary>
        //    /// 消逝时间
        //    /// </summary>
        //    public float FadeOutTime = -1f;

        //    /// <summary>
        //    /// 特效路径
        //    /// </summary>
        //    public string _effectPath = string.Empty;

        //    /// <summary>
        //    /// 伤害数字
        //    /// </summary>
        //    public int DamageNumber;

        //    /// <summary>
        //    /// 特效对象
        //    /// </summary>
        //    public GameObject _effectObject { get; private set; }

        //    /// <summary>
        //    /// 从effectManager中取出的EffectObjName
        //    /// </summary>
        //    private string EffectObjName = string.Empty;
        //}
        public override AddonTypeEnum AddonType => throw new System.NotImplementedException();

        public override void OnAdd()
        {
            throw new System.NotImplementedException();
        }

        public override void SetEnable( bool enable )
        {
            throw new System.NotImplementedException();
        }
    }
}