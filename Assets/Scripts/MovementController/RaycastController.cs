using UnityEngine;
using System.Collections;
using CustomPhysics;

[RequireComponent (typeof (RectCollider))]
public class RaycastController : MonoBehaviour, ISetupable {
	protected float _skinWidth = 0.3f;
	public LayerMask collisionMask;
	
	const float _dstBetweenRays = 5f;
	[HideInInspector]
	public int _horizontalRayCount;
	[HideInInspector]
	public int _verticalRayCount;

	[HideInInspector]
	public float _horizontalRaySpacing;
	[HideInInspector]
	public float _verticalRaySpacing;
 	RectCollider _collider = null;
	public RaycastOrigins raycastOrigins;

	public virtual void Initalize() {
		_collider = GetComponent<RectCollider>();
		CalculateRaySpacing();
	}

	public void UpdateRaycastOrigins() {
		Rectangle bounds = _collider.GetBounds();
		bounds.width -= (_skinWidth * 2f);
		bounds.height -= (_skinWidth * 2f);

		raycastOrigins.bottomLeft = new Vector2(bounds.position.x, bounds.position.y - bounds.height);
		raycastOrigins.bottomRight = new Vector2(bounds.position.x, bounds.position.y - bounds.height);
		raycastOrigins.topLeft = new Vector2(bounds.position.x, bounds.position.y + bounds.height);
		raycastOrigins.topRight = new Vector2(bounds.position.x, bounds.position.y + bounds.height);
	}

	public void CalculateRaySpacing() {
		Rectangle bounds = _collider.GetBounds();
		bounds.width -= (_skinWidth * 2f);
		bounds.height -= (_skinWidth * 2f);

		float boundsWidth = bounds.width + (-_skinWidth * 2f);
		float boundsHeight = bounds.height + (-_skinWidth * 2f);
		
		_horizontalRayCount = Mathf.RoundToInt(boundsHeight / _dstBetweenRays);
		_verticalRayCount = Mathf.RoundToInt(boundsWidth / _dstBetweenRays);
		
		_horizontalRaySpacing = bounds.height / (_horizontalRayCount - 1);
		_verticalRaySpacing = bounds.width / (_verticalRayCount - 1);
	}
	
	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
