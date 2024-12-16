using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Crazyminds.AwesomeMask.Samples.AisPlanes
{
	public class HudUI : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private GameObject _gameOver;
		[SerializeField] private Image _whiteBlink;
		[SerializeField] private Slider _lifeBar;
		[SerializeField] private GameObject _candy;

		[Header("Settings")]
		[SerializeField] private int _blinkAnimationSpeed = 2;


		public bool blink = false;

		internal void Update()
		{
			if (blink)
			{
				StartCoroutine(AnimateCollision());
				blink = false;
			}
		}

		internal void Configure(int _startingLifeCount)
		{
			var color = Color.white;
			color.a = 0f;
			_whiteBlink.color = color;

			_lifeBar.maxValue = _startingLifeCount;
			_lifeBar.value = _startingLifeCount;
		}

		internal void AddCollision()
		{
			StartCoroutine(AnimateCollision());
			_lifeBar.value = (_lifeBar.value > 0) ? _lifeBar.value - 1 : 0;
		}

		IEnumerator AnimateCollision()
		{
			var color = Color.white;
			color.a = 1f;
			while (color.a > 0)
			{
				color.a -= Time.deltaTime * _blinkAnimationSpeed;
				_whiteBlink.color = color;
				yield return null;
			}
		}

		internal void ShowGameOver()
		{
			_lifeBar.gameObject.SetActive(false);
			_gameOver.SetActive(true);
			_candy.SetActive(false);
		}
	}
}