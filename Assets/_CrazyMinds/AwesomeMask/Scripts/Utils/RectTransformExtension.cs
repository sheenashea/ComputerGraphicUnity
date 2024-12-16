using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crazyminds.AwesomeMask.Utils
{
	public static class RectTransformExtension
	{
		static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
		{
			Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
			Vector3[] objectCorners = new Vector3[4];
			rectTransform.GetWorldCorners(objectCorners);

			int visibleCorners = 0;
			// Cached
			for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
			{
				// Transform world space position of corner to screen space
				if (screenBounds.Contains(objectCorners[i])) // If the corner is inside the screen
				{
					visibleCorners++;
				}
			}
			return visibleCorners;
		}

		public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
		{
			return CountCornersVisibleFrom(rectTransform, camera) == 4; // True if all 4 corners are visible
		}

		public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
		{
			return CountCornersVisibleFrom(rectTransform, camera) > 0; // True if any corner is visible
		}
	}
}