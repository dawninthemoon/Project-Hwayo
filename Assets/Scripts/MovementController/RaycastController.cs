using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class RaycastController : MonoBehaviour, ISetupable {

	public LayerMask collisionMask;
	
	public const float _skinWidth = 0.3f;
	const float _dstBetweenRays = 5f;
	[HideInInspector]
	public int _horizontalRayCount;
	[HideInInspector]
	public int _verticalRayCount;

	[HideInInspector]
	public float _horizontalRaySpacing;
	[HideInInspector]
	public float _verticalRaySpacing;

	[HideInInspector]
	public BoxCollider2D _collider = null;
	public RaycastOrigins raycastOrigins;

	public virtual void Initalize() {
		_collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}

	public void UpdateRaycastOrigins() {
		Bounds bounds = _collider.bounds;
		bounds.Expand (_skinWidth * -2);
		
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}
	
	public void CalculateRaySpacing() {
		Bounds bounds = _collider.bounds;
		bounds.Expand (_skinWidth * -2);

		float boundsWidth = bounds.size.x;
		float boundsHeight = bounds.size.y;
		
		_horizontalRayCount = Mathf.RoundToInt (boundsHeight / _dstBetweenRays);
		_verticalRayCount = Mathf.RoundToInt (boundsWidth / _dstBetweenRays);
		
		_horizontalRaySpacing = bounds.size.y / (_horizontalRayCount - 1);
		_verticalRaySpacing = bounds.size.x / (_verticalRayCount - 1);
	}
	
	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
