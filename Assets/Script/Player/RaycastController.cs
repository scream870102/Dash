using System.Collections.Generic;

using Eccentric.Utils;

using UnityEngine;
[System.Serializable]
class RayCastController {
#if UNITY_EDITOR
    [ReadOnly, SerializeField]
#endif
    List<HitResult> result = new List<HitResult> ( );
    BoxCollider2D col = null;
#if UNITY_EDITOR
    [ReadOnly, SerializeField]
#endif
    RayCastInfo info = null;
    RayCastPoint points = null;
    float horiSpace = 0f;
    float vertSpace = 0f;
    [SerializeField] LayerMask layers = -1;
    [SerializeField] Vector2 rayNums = new Vector2 (3f, 3f);
    [SerializeField] [Range (-.5f, .5f)] float offset = -.015f;
    [SerializeField] float rayLength = .5f;
    public bool Up => info.up;
    public bool Down => info.down;
    public bool Right => info.right;
    public bool Left => info.left;
    public bool IsCollide => (Up || Down || Right || Left);
    public List<HitResult> Result => result;
    
    public RayCastController (LayerMask layers, Vector2 rayNums, float offset, float rayLength, BoxCollider2D collider2D) {
        this.layers = layers;
        this.rayNums = rayNums;
        this.offset = offset;
        this.rayLength = rayLength;
        Init (collider2D);
    }

    public void Init (BoxCollider2D collider2D) {
        col = collider2D;
        points = new RayCastPoint ( );
        info = new RayCastInfo ( );
        result = new List<HitResult> ( );
        CalculateSpace ( );
    }
    public void Tick ( ) {
        UpdateRaycastPoint ( );
        UpdateInfo ( );
    }

    void UpdateRaycastPoint ( ) {
        Bounds bounds = col.bounds;
        bounds.Expand (offset * 2);
        points.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
        points.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
        points.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
        points.topRight = new Vector2 (bounds.max.x, bounds.max.y);
    }

    void UpdateInfo ( ) {
        List<HitResult> results = new List<HitResult> ( );
        info.Reset ( );
        // Update all horizontal ray and save result
        for (int i = 0; i < rayNums.x; i++) {
            #region UPDATE_RIGHT
            Vector2 originPoint = points.topRight;
            originPoint.y -= vertSpace * i;
            RaycastHit2D hit = Physics2D.Raycast (originPoint, Vector2.right, rayLength, layers);
            if (hit.collider) {
                results.Add (new HitResult (hit, EHitDirection.RIGHT));
                info.right = true;
            }
#if UNITY_EDITOR
            Debug.DrawRay (originPoint, Vector2.right * rayLength, Color.red);
#endif
            #endregion
            #region UPDATE_LEFT
            originPoint = points.topLeft;
            originPoint.y -= vertSpace * i;
            hit = Physics2D.Raycast (originPoint, Vector2.left, rayLength, layers);
            if (hit.collider) {
                results.Add (new HitResult (hit, EHitDirection.LEFT));
                info.left = true;
            }
#if UNITY_EDITOR
            Debug.DrawRay (originPoint, Vector2.left * rayLength, Color.red);
#endif
            #endregion
        }
        //Update all vertical ray and save result
        for (int i = 0; i < rayNums.y; i++) {
            #region UPDATE_UP
            Vector2 originPoint = points.topLeft;
            originPoint.x += horiSpace * i;
            RaycastHit2D hit = Physics2D.Raycast (originPoint, Vector2.up, rayLength, layers);
            if (hit.collider) {
                results.Add (new HitResult (hit, EHitDirection.UP));
                info.up = true;
            }
#if UNITY_EDITOR
            Debug.DrawRay (originPoint, Vector2.up * rayLength, Color.green);
#endif
            #endregion
            #region UPDATE_DOWN
            originPoint = points.bottomLeft;
            originPoint.x += horiSpace * i;
            hit = Physics2D.Raycast (originPoint, Vector2.down, rayLength, layers);
            if (hit.collider) {
                results.Add (new HitResult (hit, EHitDirection.DOWN));
                info.down = true;
            }
#if UNITY_EDITOR
            Debug.DrawRay (originPoint, Vector2.down * rayLength, Color.green);
#endif
            #endregion
        }
        result.Clear ( );
        if (results.Count != 0)
            result.AddRange (results);
    }

    void CalculateSpace ( ) {
        Bounds bounds = col.bounds;
        bounds.Expand (offset * 2);
        horiSpace = bounds.size.x / (rayNums.y - 1);
        vertSpace = bounds.size.y / (rayNums.x - 1);
    }
}

[System.Serializable]
public class RayCastInfo {
    public bool up, down, right, left = false;
    public void Reset ( ) {
        up = down = right = left = false;
    }
}

[System.Serializable]
class HitResult {
    public RaycastHit2D hit2D;
    public EHitDirection direction = EHitDirection.NONE;
    public HitResult (RaycastHit2D hit2D, EHitDirection direction) {
        this.hit2D = hit2D;
        this.direction = direction;
    }

}

[System.Serializable]
class RayCastPoint {
    public Vector2 topLeft, topRight = new Vector2 ( );
    public Vector2 bottomLeft, bottomRight = new Vector2 ( );
}

public enum EHitDirection {
    UP,
    DOWN,
    RIGHT,
    LEFT,
    NONE,
}
