using UnityEngine;
namespace Poi
{
    public interface ITarget
    {
        /// <summary>
        /// 
        /// </summary>
        ISkillTarget First { get; }
        Vector3 Point { get; }
    }

    public interface ISkillTarget
    {
        Transform transform { get; }
        Transform Chest { get; }
    }
}