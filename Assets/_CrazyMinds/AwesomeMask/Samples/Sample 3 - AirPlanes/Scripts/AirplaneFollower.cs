using Crazyminds.AwesomeMask;
using System;
using System.Collections;
using UnityEngine;

namespace Crazyminds.AwesomeMask.Samples.AisPlanes
{
	public class AirplaneFollower : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private RectTransform _rectTransform;
		[SerializeField] private Animation _amimation;

		Transform _target;
		Camera _camera;
		AwesomeMask _awesomeMask;

		public Action AboutToDestroy;

		private void Update()
		{
			if (_target == null)
			{
				return;
			}

			if (_camera == null)
			{
				return;
			}

			_rectTransform.position = _camera.WorldToScreenPoint(_target.position);
		}

		public void Show(GameObject target, Camera camera, AwesomeMask awesomeMask)
		{
			_awesomeMask = awesomeMask;
			_awesomeMask.Add(GetComponent<AwesomeMaskHollow>());
			StopAllCoroutines();
			Destroyable destroyable = target.GetComponent<Destroyable>();
			destroyable.Hit += OnHit;
			destroyable.Config(_awesomeMask);
			_target = target.transform;
			_camera = camera;
			_rectTransform.position = _camera.WorldToScreenPoint(_target.position);
			gameObject.SetActive(true);
			_amimation.Play("AnimationShow");
		}

		public void Hide()
		{
			AboutToDestroy?.Invoke();
			Destroy(gameObject);
		}

		private void OnHit()
		{
			_awesomeMask.Remove(GetComponent<AwesomeMaskHollow>());
			_amimation.Play("AnimationDestroy");
			StartCoroutine(WaitAndHide());
		}

		IEnumerator WaitAndHide()
		{
			yield return new WaitForSeconds(1f);
			Hide();
		}
	}
}