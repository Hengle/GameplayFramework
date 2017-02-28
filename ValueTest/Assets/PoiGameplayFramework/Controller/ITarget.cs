using UnityEngine;
namespace Poi
{
    public interface ITarget
    {
        /// <summary>
        /// 
        /// </summary>
        Transform First { get; }
        Vector3 Point { get; }
    }

    public interface IPawnTarget
    {

    }
}