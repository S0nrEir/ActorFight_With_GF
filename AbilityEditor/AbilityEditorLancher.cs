using Aquila.Toolkit;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// 技能编辑器场景启动器
    /// </summary>
    public class AbilityEditorLancher : MonoBehaviour
    {
        void Start()
        {
            Tools.SetActive(_entryObject,true);
            Destroy(gameObject);
        }

        [SerializeField] private GameObject _entryObject;
    }
}