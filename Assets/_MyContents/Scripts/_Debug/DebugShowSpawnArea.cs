#if ENABLE_DEBUG && UNITY_EDITOR
using UnityEngine;

namespace MainContents.DebugUtility
{
    public sealed class DebugShowSpawnArea : MonoBehaviour
    {
        [SerializeField] Color _color;
        [SerializeField] Rect _areaRect;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = this._color;
            var center = this._areaRect.center;
            var size = this._areaRect.size;
            Gizmos.DrawCube(new Vector3(center.x, 0f, center.y), new Vector3(size.x, 0f, size.y));
        }
    }
}
#endif