using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Toolkit;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 移动组件 负责处理位移 by yhc
    /// </summary>
    public class Addon_Move : Addon_Base
    {
        public void SetSpeed ( float speed )
        {
            _speed = speed;
        }

        /// <summary>
        /// stop
        /// </summary>
        public void Stop ()
        {
            _controller.Move( Vector3.zero );
            _pathIndex = 0;
            _path_list?.Clear();
            Actor.Trigger( ActorEventEnum.MOVE_TO_FINAL_POINT );
        }

        public void SetPathList ( List<Vector2> pathArr )
        {
            if (pathArr is null || pathArr.Count == 0)
                return;

            _path_list.Clear();
            _path_list = pathArr;
            //if (Actor.TryGetAddon<Addon_Data>( out var addon ))
            //{
            //    SetSpeed( addon.GetNumricValue( DataAddonFieldTypeEnum.NUM_MOVE_SPEED, 1 ) );
            //}
        }

        public void SetTargetPahtList ( List<Vector2> path_arr )
        {
            if (path_arr is null || path_arr.Count == 0)
                return;

            _path_list.Clear();
            _path_list = path_arr;
            //if (Actor.TryGetAddon<Addon_Data>( out var addon ))
            //{
            //    _speed = (float)addon.GetNumricValue( DataAddonFieldTypeEnum.NUM_MOVE_SPEED, 1f );
            //}
            _pathIndex = 0;
        }

        /// <summary>
        /// 是否到达了最后一个路点，是返回true
        /// </summary>
        public bool IsReachedFinalPoint ()
        {
            return _pathIndex >= _path_list.Count;
        }

        public bool IsReachedFinalTarget()
        {
            return _pathIndex >= _path_list.Count;
        }

        public void TargetNext ( float elapsedSeconds )
        {
            if (_path_list.Count == 0)
                return;

            if (IsReachedFinalPoint())
            {
                Stop();
                return;
            }

            var nextPos = Vector3.zero;
            nextPos.x = _path_list[_pathIndex].x;
            nextPos.z = _path_list[_pathIndex].y;
            nextPos.y = Tools.Fight.TerrainPositionY(string.Empty, nextPos.x, nextPos.z, 0f );//修改layer
            var actor_pos = Actor.CachedTransform.position;

            Rotate( _path_list[_pathIndex] );
            var dis = Mathf.Abs( Vector3.Distance( actor_pos, nextPos ) );
            if (dis <= .1f)
            {
                Actor.SetWorldPosition( nextPos );
                SetToNextPathPoint( true );
                return;
            }

            Actor.SetWorldPosition( actor_pos + ( nextPos - actor_pos ).normalized * _speed * elapsedSeconds );
        }

        public void Next ( float elapsedSeconds )
        {
            if (_path_list.Count == 0)
                return;

            if (IsReachedFinalPoint())
            {
                Stop();
                return;
            }

            Vector3 next_pos = Vector3.zero;
            next_pos.x = _path_list[_pathIndex + 1].x;
            next_pos.z = _path_list[_pathIndex + 1].y;
            next_pos.y = Tools.Fight.TerrainPositionY(string.Empty, next_pos.x, next_pos.z, 0f );//修改layer

            _cachedTargetPos.x = _path_list[_pathIndex].x;
            _cachedTargetPos.z = _path_list[_pathIndex].y;
            _cachedTargetPos.y = Tools.Fight.TerrainPositionY(string.Empty, _cachedTargetPos.x, _cachedTargetPos.z, 0f );//修改layer

            var dis = Vector3.Distance( Actor.CachedTransform.position, next_pos );
            if (dis <= 0.1f)
            {
                SetToNextPathPoint( true );
                Actor.SetWorldPosition( _cachedTargetPos );
                return;
            }

            var currPos = Actor.CachedTransform.position;
            var moveTo = currPos + ( _cachedTargetPos - Actor.CachedTransform.position ).normalized * 1f * elapsedSeconds;

            if((moveTo - currPos).sqrMagnitude >= (next_pos - currPos).magnitude)
            {
                SetToNextPathPoint( true );
                Actor.SetWorldPosition( _cachedTargetPos );
                return;
            }

            Actor.SetWorldPosition( currPos + ( _cachedTargetPos - Actor.CachedTransform.position ).normalized * _speed * elapsedSeconds );
        }


        public void MoveTo ( Vector3 start, Vector3 target, float elapsedSeconds )
        {
            //Rotate( target );
            //Log.Info( $"ActorID:{Actor.ActorID},pos:{Actor.CachedTransform.position}" );
            Actor.SetWorldPosition( Actor.CachedTransform.position + ( target - start ).normalized * _speed * elapsedSeconds );

            Actor.CachedTransform.rotation = Quaternion.Slerp
                (
                    Actor.CachedTransform.rotation,
                    Quaternion.LookRotation( target - Actor.CachedTransform.position ),
                    .8f
                );
        }

        /// <summary>
        /// move actor
        /// </summary>
        public CollisionFlags Move ( Vector3 direction, float time, float speed )
        {
            //direction = new Vector3( direction.x, SceneConfig.SCENE_LAND_Y, direction.z );
            CollisionFlag = _controller.Move( direction * speed * time );

            return CollisionFlag;
        }

        public CollisionFlags MoveByElapesedTime ( float elapsedSeconds, Vector3 direction, float speed )
        {
            //direction = new Vector3( direction.x, SceneConfig.SCENE_LAND_Y, direction.z );
            CollisionFlag = _controller.Move( direction * speed * elapsedSeconds );

            return CollisionFlag;
        }

        /// <summary>
        /// Move
        /// </summary>
        public override AddonTypeEnum AddonType => AddonTypeEnum.MOVE;

        public override void OnAdd ()
        {
            _path_list = new List<Vector2>();
            //if (Actor.TryGetAddon<DataAddon>( out var addon ))
            //    SetSpeed( (float)addon.GetFloatDataValue( DataAddonFieldTypeEnum.INT_MOVE_SPEED, 1f ) / 1000f );
            SetSpeed( 1f );
        }

        public override void Init ( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );
            _controller = Tools.GetComponent<CharacterController>( Actor.gameObject );

            if (_controller == null)
            {
                //Debug.LogError( "_controller == null" );
                _controller = Actor.gameObject.AddComponent<CharacterController>();
            }
        }

        public override void Dispose ()
        {
            base.Dispose();
            _path_list = null;
            _pathIndex = 0;
        }

        public override void Reset ()
        {
            base.Reset();
            CollisionFlag = CollisionFlags.None;
            _pathIndex = 0;
            SetSpeed( 1f );
        }

        /// <summary>
        /// 设置到下一个路点，成功返回false，每次前往下一个路点的时候是否自动将角色转向
        /// </summary>
        private bool SetToNextPathPoint ( bool needToRotate )
        {
            if (_path_list is null || _path_list.Count == 0)
                return false;

            //if (IsReachedFinalPoint())
            //    return false;

            if (IsReachedFinalTarget())
                return false;

            _pathIndex++;
            //需要旋转且朝向不等，才做旋转，否则不做处理
            if (needToRotate && _pathIndex < _path_list.Count)
                Rotate( _path_list[_pathIndex] );

            return true;
        }

        /// <summary>
        /// 根据自身当前位置转向下一个位置
        /// </summary>
        private void Rotate ( Vector3 target )
        {
            var currPos = Actor.CachedTransform.position;
            var angleOfL = Mathf.Atan2( ( target.x - currPos.x ), ( target.z - currPos.z ) ) * Mathf.Rad2Deg;
            if (angleOfL < 0)
                angleOfL += 360;
            //角度差距过小，不做转向
            var tempEuler = Actor.CachedTransform.localEulerAngles;
            if (Mathf.Abs( tempEuler.y - angleOfL ) < 15)
                return;

            if (angleOfL + tempEuler.y > 360)
                angleOfL = 360 - tempEuler.y;

            Actor.CachedTransform.localEulerAngles = new Vector3( tempEuler.x, tempEuler.y + angleOfL, tempEuler.z );
        }

        private void Rotate ( Vector2 target )
        {
            var currPos = Actor.CachedTransform.position;
            var angleOfL = Mathf.Atan2( ( target.x - currPos.x ), ( target.y - currPos.z ) ) * Mathf.Rad2Deg;
            if (angleOfL < 0)
                angleOfL += 360;
            //角度差距过小，不做转向
            var tempEuler = Actor.CachedTransform.localEulerAngles;
            if (Mathf.Abs( tempEuler.y - angleOfL ) < 15)
                return;

            if (angleOfL + tempEuler.y > 360)
                angleOfL = 360 - tempEuler.y;

            Actor.CachedTransform.localEulerAngles = new Vector3( tempEuler.x, tempEuler.y + angleOfL, tempEuler.z );
        }
        
        /// <summary>
        /// 碰撞标识
        /// </summary>
        public CollisionFlags CollisionFlag { get; private set; } = CollisionFlags.None;

        /// <summary>
        /// controller
        /// </summary>
        private CharacterController _controller = null;

        /// <summary>
        /// 从寻路获取的路点信息集合，以此为据进行移动处理
        /// </summary>
        private List<Vector2> _path_list = null;

        /// <summary>
        /// 路点信息索引下标
        /// </summary>
        private int _pathIndex = 0;

        /// <summary>
        /// 目标位置
        /// </summary>
        private Vector3 _cachedTargetPos = Vector3.zero;

        /// <summary>
        /// 当前index的寻路目标点
        /// </summary>
        private Vector3 _currStepTargetPos;

        /// <summary>
        /// 速度
        /// </summary>
        private float _speed = 1f;
    }
}
