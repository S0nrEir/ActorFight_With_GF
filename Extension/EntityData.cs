using System;
using GameFramework;
using UnityEngine;

namespace Aquila
{
    [Serializable]
    public abstract class EntityData : IReference
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private int m_TypeId;

        [SerializeField]
        private Vector3 m_Position = Vector3.zero;

        [SerializeField]
        private Quaternion m_Rotation = Quaternion.identity;

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;

        [SerializeField]
        private string m_ModelPath = string.Empty;

        public void Init(int EntityId, int TypeId)
        {
            m_Id = EntityId;
            m_TypeId = TypeId;
        }
        
        
        
        public EntityData( int entityId, int typeId )
        {
            Init(entityId, typeId);
        }


        public EntityData()
        {
            m_Id = -1;
            m_TypeId = -1;
        }

        /// <summary>
        /// 实体编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 实体类型编号。
        /// </summary>
        public int TypeId
        {
            get
            {
                return m_TypeId;
            }
        }

        /// <summary>
        /// 实体位置。
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        /// <summary>
        /// 实体朝向。
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                m_Rotation = value;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return m_Scale;
            }
            set
            {
                m_Scale = value;
            }
        }

        public string ModelPath
        {
            get
            {
                return m_ModelPath;
            }
            set
            {
                m_ModelPath = value;
            }
        }

        public virtual void Clear()
        {
            m_Id = -1;
            m_TypeId = -1;
        }
    }
}
