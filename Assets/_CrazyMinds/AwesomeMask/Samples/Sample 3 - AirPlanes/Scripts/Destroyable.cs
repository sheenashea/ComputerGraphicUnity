using Crazyminds.AwesomeMask;
using System;
using UnityEngine;

namespace Crazyminds.AwesomeMask.Samples.AisPlanes
{
	public class Destroyable : MonoBehaviour
	{
		public Action Destroyed; // gameobject destroyed
		public Action Hit;       // hit by mouse

		protected Camera _camera;
		private AwesomeMask _awesomeMask;

		private void OnDestroy()
		{
			Destroyed?.Invoke();
		}

		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (_awesomeMask.CanRayCast(Input.mousePosition))
				{
					RaycastHit2D hitInfo = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Input.mousePosition));

					if (hitInfo.collider != null)
					{
						if (gameObject == hitInfo.collider.gameObject)
						{
							Hit?.Invoke();
							AnimateDestroy();
						}
					}
				}
			}
		}

		public void Config(AwesomeMask awesomeMask)
		{
			_awesomeMask = awesomeMask;
		}

		protected virtual void AnimateDestroy() { }
	}
}