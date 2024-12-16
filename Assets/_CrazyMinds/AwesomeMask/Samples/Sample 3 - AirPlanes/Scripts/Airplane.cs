using System;
using System.Collections;
using UnityEngine;

namespace Crazyminds.AwesomeMask.Samples.AisPlanes
{
	public class Airplane : Destroyable
	{
		[Header("References")]
		[SerializeField] private Transform _transform;
		[SerializeField] private SpriteRenderer _renderer;
		[SerializeField] private ParticleSystem _particle;
		[SerializeField] private Collider2D _collider;

		[Header("Size settings")]
		[SerializeField] private float _movingSpeed = 1f;
		[SerializeField] private float _delayToShoot = 2f;

		public Action<Transform> JustShoot;

		public void Spawn(Camera camera)
		{
			_transform.gameObject.SetActive(true);
			_camera = camera;
			Vector2 startPosition = GetRandom3DStartPosition();
			_transform.position = startPosition;

			var targetScreenPoint = _camera.WorldToScreenPoint(startPosition);
			targetScreenPoint.x = Screen.width * .8f;
			var targetPosition = _camera.ScreenToWorldPoint(targetScreenPoint);
			StartCoroutine(MoveToTargetPosition(targetPosition));
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
			_particle.Play();

			yield return new WaitForSeconds(1.0f);

			Destroy(gameObject);
		}

		IEnumerator MoveToTargetPosition(Vector2 targetPosition)
		{
			var startPosition = _transform.position;
			var progress = 0f;
			while (Vector2.Distance(_transform.position, targetPosition) > .1f)
			{
				_transform.position = Vector2.Lerp(startPosition, targetPosition, progress);
				progress += _movingSpeed * Time.deltaTime;
				yield return null;
			}

			StartCoroutine(WaitAndShoot());
		}

		IEnumerator WaitAndShoot()
		{
			yield return new WaitForSeconds(_delayToShoot);
			Shoot();
			StartCoroutine(WaitAndShoot());
		}

		private void Shoot()
		{
			JustShoot?.Invoke(_transform);
		}

		private Vector2 GetRandom3DStartPosition()
		{
			var randomHeight = UnityEngine.Random.Range(Screen.height * .1f, Screen.height * .9f);
			var newStartPos = _camera.ScreenToWorldPoint(new Vector2(Screen.width, randomHeight));
			return newStartPos;
		}
	}
}