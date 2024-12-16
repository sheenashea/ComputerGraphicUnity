using System;
using System.Collections;
using UnityEngine;

namespace Crazyminds.AwesomeMask.Samples.AisPlanes
{
	public class Projectile : Destroyable
	{
		[Header("References")]
		[SerializeField] private Transform _transform;
		[SerializeField] private SpriteRenderer _renderer;
		[SerializeField] private ParticleSystem _particle;
		[SerializeField] private Collider2D _collider;

		[Header("Size settings")]
		[SerializeField] private float _movingSpeed = 1f;

		public Action Collided;  // collided with candles

		public void Spawn(Camera camera, Vector3 startPosition)
		{
			_transform.gameObject.SetActive(true);
			_camera = camera;
			_transform.position = startPosition;

			StartCoroutine(MoveToLeft());
		}

		protected override void AnimateDestroy()
		{
			StopAllCoroutines();
			StartCoroutine(AsyncAnimateDestroy());

		}
		IEnumerator AsyncAnimateDestroy()
		{
			_collider.enabled = false;
			_renderer.enabled = false;
			yield return null;
			_particle.Play();

			yield return new WaitForSeconds(1.0f);

			Destroy(gameObject);
		}

		IEnumerator MoveToLeft()
		{
			while (true)
			{
				_transform.Translate(Vector3.left * (_movingSpeed * Time.deltaTime), Space.World);

				// test candy collision
				var screenPoint = _camera.WorldToScreenPoint(_transform.position);
				if (screenPoint.x <= 78)
				{
					Hit?.Invoke();
					Collided?.Invoke();
					AnimateDestroy();
				}

				yield return null;
			}
		}
	}
}