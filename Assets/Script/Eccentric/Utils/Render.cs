using UnityEngine;
namespace GalaxySeeker {
    public static class Render {
        /// <summary>flip target due to IsFacingRight</summary>
        /// <param name="IsFacingRight">is target facing at right direction</param>
        /// <param name="target">which transform to change</param>
        public static void ChangeDirection (bool IsFacingRight, Transform target, bool IsInvert = false) {
            Vector3 tmp = target.localScale;
            tmp.x = Mathf.Abs (tmp.x) * (IsFacingRight?1f: -1f) * (IsInvert? - 1 : 1);
            target.localScale = tmp;
        }
        /// <summary>flip target due to IsFacingRight</summary>
        /// <param name="IsFacingRight">is target facing at right direction</param>
        /// <param name="target">which transform to change</param>
        public static void ChangeDirectionY (bool IsFacingRight, Transform target, bool IsInvert = false) {
            Vector3 tmp = target.localScale;
            tmp.y = Mathf.Abs (tmp.y) * (IsFacingRight?1f: -1f) * (IsInvert? - 1 : 1);
            target.localScale = tmp;
        }

        public static void ChangeDirectionXWithSpriteRender (bool flipX, SpriteRenderer renderer) {
            renderer.flipX = flipX;
        }
        public static void ChangeDirectionYWithSpriteRender (bool flipY, SpriteRenderer renderer) {
            renderer.flipY = flipY;
        }
    }
}
