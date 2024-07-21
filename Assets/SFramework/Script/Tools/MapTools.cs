using UnityEngine;

namespace SFramework.Tools
{
    public class MapTools
    {
        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static Vector3 GetDirToMouse(Vector3 fromPosition)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            return (mouseWorldPosition - fromPosition).normalized;
        }

        public static Vector3 Screen2WorldPos(Vector3 pos)
        {
            return Screen2WorldPos(pos, Camera.main);
        }

        public static Vector3 Screen2WorldPos(Vector3 pos, Camera worldCamera)
        {
            pos.z = 0;
            var viewRay = worldCamera.ScreenPointToRay(pos);
            var worldPos = viewRay.GetPoint(-viewRay.origin.z / viewRay.direction.z);
            return worldPos;
        }
    }
}
