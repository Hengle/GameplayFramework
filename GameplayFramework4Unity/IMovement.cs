using UnityEngine;

namespace Poi
{
    public interface IMovement
    {
        void MoveTo(Vector3 destination);
        void MoveTo(Vector2 destination);
    }
}