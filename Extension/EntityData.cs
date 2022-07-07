﻿using System;
using UnityEngine;

namespace Aquila
{
    [Serializable]
    public abstract class EntityData
    {
        [SerializeField]
        private int m_Id = 0;

        [SerializeField]
        private int m_TypeId = 0;

        [SerializeField]
        private Vector3 m_Position = Vector3.zero;

        [SerializeField]
        private Quaternion m_Rotation = Quaternion.identity;

        [SerializeField]
        private Vector3 m_Scale = Vector3.one;

        [SerializeField]
        private string m_ModelPath = string.Empty;

        public EntityData( int entityId, int typeId )
        {
            m_Id = entityId;
            m_TypeId = typeId;
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
    }
}
