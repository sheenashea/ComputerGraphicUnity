using Crazyminds.AwesomeMask.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

namespace Crazyminds.AwesomeMask.Samples.Bubbles
{
	public class Bubble : MonoBehaviour
	{
		[SerializeField] private RectTransform _rect;

		[Header("Size settings")]
		[SerializeField] private float _minSize = 50f;
		[SerializeField] private float _maxSize = 200f;

		[Header("Speed settigns")]
		[SerializeField] private float _minYSpeed = -200f;
		[SerializeField] private float _maxYSpeed = 200f;
		[SerializeField] private float _minStartXSpeed = 2000f;
		[SerializeField] private float _maxStartXSpeed = 4000f;
		[SerializeField] private float _horizontalDeceleration = 8000f;
		[SerializeField] private float _decelerationDrag = 10f;

		Vector2 _startSpeed = new Vector2();
		bool _xDirection = false; // false == left

		private void OnEnable()
		{
			_startSpeed.x = Random.Range(_minStartXSpeed, _maxStartXSpeed);
			_startSpeed.y = Random.Range(_minYSpeed, _maxYSpeed);

			var size = Random.Range(_minSize, _maxSize);
			_rect.sizeDelta = new Vector2(size, size);

			_xDirection = Random.Range(0, 2) < 1;
		}


		void Update()
		{
			//Debug.Log("_startSpeed: " + _startSpeed);
			var position = _rect.position;
			position.x += (!_xDirection) ? (-_startSpeed.x * Time.deltaTime) : (_startSpeed.x * Time.deltaTime);
			position.y += (_startSpeed.y >= _startSpeed.x) ? (_startSpeed.y * Time.deltaTime) : (_startSpeed.x * Time.deltaTime);
			_rect.position = position;

			// deceleration
			if (_startSpeed.x != 0)
			{
				var newSpeed = Mathf.Max(_startSpeed.x - _horizontalDeceleration, 0);
				_startSpeed.x = newSpeed;

				// decrease deceleration
				var delta = Time.deltaTime * _decelerationDrag;
				_horizontalDeceleration -= Mathf.Max(delta, 0);
			}

			if (!_rect.IsVisibleFrom(Camera.main))
			{
				Destroy(gameObject);
			}
		}
	}
}