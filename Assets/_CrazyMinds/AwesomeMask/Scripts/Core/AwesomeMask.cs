
//
// Developer: Julio Dutra - Crazy Minds Game Studio
// Contact: julio@crazyminds.net
//

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Crazyminds.AwesomeMask
{
	[AddComponentMenu("UI/CrazyMinds/AwesomeMask")]
	[DisallowMultipleComponent]
	public class AwesomeMask : MonoBehaviour, ICanvasRaycastFilter
	{
		[SerializeField] private List<AwesomeMaskHollow> _hollowElements = new List<AwesomeMaskHollow>();

		private Camera _camera;

		/// <summary>
		/// Add a new element to the interactive hollow list.
		/// This new element will be taken into account when checking user interaction.
		/// </summary>
		/// <param name="elementIndex"></param>
		/// <param name="worldPosition"></param>
		public void Add(AwesomeMaskHollow hollow)
		{
			if (_hollowElements == null)
			{
				_hollowElements = new List<AwesomeMaskHollow>();
			}

			if (_hollowElements.Contains(hollow))
			{
				Debug.LogWarning("The Hollow list already contain this hollow instance. Ignoring.");
				return;
			}

			_hollowElements.Add(hollow);
		}

		/// <summary>
		/// Add a new element to the interactive hollow list.
		/// This new element will be taken into account when checking user interaction.
		/// </summary>
		/// <param name="elementIndex"></param>
		/// <param name="worldPosition"></param>
		public void Remove(AwesomeMaskHollow hollow)
		{
			if (_hollowElements == null || _hollowElements.Count == 0)
			{
				Debug.LogWarning("The Hollow list is null or empty. Ignoring.");
				return;
			}

			if (!_hollowElements.Contains(hollow))
			{
				Debug.LogWarning("The Hollow list doesn't contain this hollow instance. Ignoring.");
				return;
			}

			_hollowElements.Remove(hollow);
		}


		/// <summary>
		/// Set the hollow element position in the screen using the worldPosition as the reference position.
		/// Puts the UI element above the 
		/// </summary>
		/// <param name="hollowElementIndex">The index of the hollow element in the hollowElements list</param>
		/// <param name="worldPosition"></param>
		public void SetElementPosition(int hollowElementIndex, Vector3 worldPosition)
		{
			if (_hollowElements == null || _hollowElements.Count == 0)
			{
				Debug.LogWarning("There is no hollow to posit. List is null or empty. Ignoring.", this);
				return;
			}
			else if (hollowElementIndex >= _hollowElements.Count)
			{
				Debug.LogWarning("The hollow index is invalid. Ignoring.", this);
				return;
			}

			SetElementPosition(_hollowElements[hollowElementIndex], worldPosition);
		}

		/// <summary>
		/// Set the element position in the screen using the worldPosition as the reference position.
		/// Puts the UI element above the 
		/// </summary>
		/// <param name="elementIndex"></param>
		/// <param name="worldPosition"></param>
		public void SetElementPosition(AwesomeMaskHollow hollowElement, Vector3 worldPosition)
		{
			if (_camera == null || !_camera.isActiveAndEnabled)
				_camera = Camera.main;

			if (hollowElement == null)
			{
				Debug.LogWarning("HollowElement is null. Ignoring.", this);
				return;
			}
			else if (_camera == null)
			{
				Debug.LogWarning("Camera.main is null. Can't calculate screen point position. Ignoring.", this);
				return;
			}

			Vector2 screenPoint = (Vector2)_camera.WorldToScreenPoint(worldPosition);
			hollowElement.rectTransform.position = screenPoint;
		}

		/// <summary>
		/// Cast a 3D ray against the Colliders in the Scene returning the first Collider along the ray.
		/// </summary>
		/// <param name="mousePosition"></param>
		/// <returns></returns>
		public bool CanRayCast(Vector2 mousePosition)
		{
			_camera = GetCamera();

			if ((this as ICanvasRaycastFilter).IsRaycastLocationValid(mousePosition, _camera))
			{
				return false;
			}

			return true;
		}

		bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			// Verify there is any configured transparent elemet - if NOT, block raycast
			if (_hollowElements == null || _hollowElements.Count == 0)
			{
				return true;
			}

			// verify if the screen point is inside any transparent element
			foreach (var hollow in _hollowElements)
			{
				if (hollow != null && !hollow.gameObject.activeSelf || !hollow.enabled)
					continue;

				if (VerifyHit(hollow, screenPoint, eventCamera))
				{
					return false;
				}
			}

			// block raycast if anything else
			return true;
		}

		/// <summary>
		/// Verify if the screen point is a valid point inside the transparent element. 
		/// </summary>
		/// <returns>True if hits the transparentElement. 
		/// False if the screen point is outside the transparentElement or if it hits a transparent pixel.
		/// </returns>
		private bool VerifyHit(AwesomeMaskHollow hollowElement, Vector2 screenPoint, Camera eventCamera)
		{
			var rectTransform = hollowElement.rectTransform;

			// if screenpoint is outside the transparentElement rectangle, return false (NO HIT).
			Vector2 localPositionPivotRelative;
			bool hit = RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPositionPivotRelative);
			if (!hit)
			{
				return false;
			}

			if (!rectTransform.rect.Contains(localPositionPivotRelative))
			{
				return false;
			}

			var sprite = hollowElement.sprite;
			// if screenpoint is inside the transparentElement rectangle and the sprite is NULL, return true
			if (sprite == null)
			{
				return true;
			}

			// removing pivot offset from local position
			var localPosition = new Vector2(localPositionPivotRelative.x + rectTransform.pivot.x * rectTransform.rect.width,
											localPositionPivotRelative.y + rectTransform.pivot.y * rectTransform.rect.height);

			var spriteRect = sprite.textureRect;
			var maskRect = rectTransform.rect;

			var x = 0;
			var y = 0;
			// convert to texture space
			switch (hollowElement.type)
			{

				case Image.Type.Sliced:
					{
						var border = sprite.border;
						// x slicing
						if (localPosition.x < border.x)
						{
							x = Mathf.FloorToInt(spriteRect.x + localPosition.x);
						}
						else if (localPosition.x > maskRect.width - border.z)
						{
							x = Mathf.FloorToInt(spriteRect.x + spriteRect.width - (maskRect.width - localPosition.x));
						}
						else
						{
							x = Mathf.FloorToInt(spriteRect.x + border.x +
												 ((localPosition.x - border.x) /
												 (maskRect.width - border.x - border.z)) *
												 (spriteRect.width - border.x - border.z));
						}
						// y slicing
						if (localPosition.y < border.y)
						{
							y = Mathf.FloorToInt(spriteRect.y + localPosition.y);
						}
						else if (localPosition.y > maskRect.height - border.w)
						{
							y = Mathf.FloorToInt(spriteRect.y + spriteRect.height - (maskRect.height - localPosition.y));
						}
						else
						{
							y = Mathf.FloorToInt(spriteRect.y + border.y +
												 ((localPosition.y - border.y) /
												 (maskRect.height - border.y - border.w)) *
												 (spriteRect.height - border.y - border.w));
						}
					}
					break;
				case Image.Type.Simple:
				default:
					{
						// conversion to uniform UV space
						x = Mathf.FloorToInt(spriteRect.x + spriteRect.width * localPosition.x / maskRect.width);
						y = Mathf.FloorToInt(spriteRect.y + spriteRect.height * localPosition.y / maskRect.height);
					}
					break;
			}

			try
			{
				bool isValid = sprite.texture.GetPixel(x, y).a > 0;
				return isValid;
			}
			catch (UnityException e)
			{
				Debug.LogError("Something wrong happened. " + e.Message);
				return false;
			}
		}

		private Camera GetCamera()
		{
			Canvas canvas = GetComponentInParent<Canvas>();
			switch (canvas.renderMode)
			{
				case RenderMode.ScreenSpaceOverlay:
					{
						return null;
					}
				case RenderMode.ScreenSpaceCamera:
				case RenderMode.WorldSpace:
					{
						return canvas.worldCamera;
					}
			}

			return null;
		}
	}
}