using Aquila.Fight.Addon;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;
using UnityEngine.AI;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 导航
    /// </summary>
    public class Addon_Nav : Addon_Base
    {
        /// <summary>
        /// 是否达到了目标点，达到了返回true
        /// </summary>
        public bool IsReachDestination()
        {
            if ( !_openFlag )
                return false;

            var isReached = ( TargetTransform.position - _targetPos ).sqrMagnitude <= StopDistance;
            return isReached;
        }

        public void SamplePosition( Vector3 pos )
        {
            if ( NavMesh.SamplePosition( pos, out var hit, 15, 1 ) )
            {
                var oldPos = Actor.CachedTransform.position;
                Actor.SetWorldPosition( new Vector3( oldPos.x, hit.position.y, oldPos.z ) );
            }
        }

        /// <summary>
        /// 设置目标点
        /// </summary>
        public void SetDestination( Vector3 targetPos )
        {
            //var navPath = new NavMeshPath();
            //var isFoundPath = NavMesh.CalculatePath( Actor.CachedTransform.position, targetPos, NavMesh.AllAreas, navPath );
            //if (!isFoundPath)
            //    return;


            if ( !_agent.isOnNavMesh )
            {
                _agent.enabled = false;
                _agent.enabled = true;
            }

            _agent.isStopped = false;
            //_agent.SetPath( navPath );
            _targetPos = targetPos;

            SetStopDistance( .001f );
            _openFlag = true;
            //_agent.SetPath( navPath );
            _agent.SetDestination( _targetPos );
        }

        /// <summary>
        /// 更新位置
        /// </summary>
        public void Warp( Vector3 pos )
        {
            _agent.Warp( pos );
        }

        /// <summary>
        /// 设定停止距离
        /// </summary>
        public void SetStopDistance( float dis )
        {
            if ( dis - float.Epsilon <= 0 )
                return;

            StopDistance = dis;
            _agent.stoppingDistance = StopDistance;
        }

        /// <summary>
        /// 设定速度
        /// </summary>
        public void SetSpeed( float spd )
        {
            if ( spd - float.Epsilon <= 0 )
                return;

            _agent.speed = spd;
        }

        public void Stop()
        {
            if ( !_agent.isOnNavMesh )
                return;

            Warp( _targetPos );
            StopInmidiate();
        }

        public void StopInmidiate()
        {
            if ( !_agent.isOnNavMesh )
                return;

            _agent.ResetPath();
            _agent.isStopped = true;
            _targetPos = Vector3.zero;
        }

        //------------------------override------------------------
        public override AddonTypeEnum AddonType => AddonTypeEnum.NAV;

        public override void OnAdd()
        {
            _agent = Tools.GetComponent<NavMeshAgent>( Actor.gameObject );
            if ( _agent == null )
            {
                if ( !Tools.TryAddComponent<NavMeshAgent>( Actor.gameObject, out _agent ) )
                    throw new GameFrameworkException( $"faild to add navMeshAgent to actor:{Actor.ActorID}" );
            }
            //初始参数设置
            _agent.enabled               = true;
            _agent.updateRotation        = true;
            _agent.updatePosition        = true;
            _agent.stoppingDistance      = StopDistance;
            _agent.autoBraking = false;
            _agent.angularSpeed = 360f;
            _agent.autoRepath            = false;
            //避障半径，actor穿行的临时解决方案
            _agent.radius                = 0.1f;
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            //if ( Actor.TryGetAddon<Addon_Data>( out var dataAddon ) )
            //    _agent.speed = dataAddon.GetNumricValue( DataAddonFieldTypeEnum.NUM_MOVE_SPEED, 1f ) / 1000f;
            //else
            //    _agent.speed = 1f;
        }

        public override void Dispose()
        {
            base.Dispose();
            _agent = null;
        }

        public override void Reset()
        {
            base.Reset();
            SetStopDistance( 1f );
            SetSpeed( 1f );
            _targetPos = Vector3.zero;
            _openFlag   = false;

            //if ( !Actor.TryGetAddon<Addon_Data>( out var _dataAddon ) )
            //    _agent.speed = 1f;
            //else
            //{
            //    _agent.speed = _dataAddon.GetNumricValue( DataAddonFieldTypeEnum.NUM_MOVE_SPEED, 1f ) / 1000f;
            //}
        }

        private bool _openFlag = false;

        /// <summary>
        /// 目标点
        /// </summary>
        private Vector3 _targetPos;

        /// <summary>
        /// 停止距离
        /// </summary>
        public float StopDistance { get; private set; } = .01f;

        /// <summary>
        /// 寻路
        /// </summary>
        private NavMeshAgent _agent;
    }

}