using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crazyminds.AwesomeMask.Samples.AisPlanes
{
	public class Airplanes : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Transform _airplaneParent;
		[SerializeField] private Camera _camera;

		[Header("Prefabs")]
		[SerializeField] private GameObject _airplanePrefab;
		[SerializeField] private GameObject _airplaneProjectile;

		public Action<GameObject> AirplaneCreated;
		public Action<GameObject> ProjectileCreated;
		public Action ProjectileCollided;

		List<GameObject> _liveAirPlanes = new List<GameObject>();
		List<GameObject> _liveProjectiles = new List<GameObject>();

		private bool _isGameOver = false;

		private void Update()
		{
			if (!_isGameOver && CanCreateAirPlanes())
			{
				CreateAirPlane();
			}
		}

		public void GameOver()
		{
			_isGameOver = true;

			foreach (var airPlane in _liveAirPlanes)
			{
				Destroy(airPlane);
			}

			foreach (var projectile in _liveProjectiles)
			{
				Destroy(projectile);
			}
		}

		private void CreateAirPlane()
		{
			var newAirPlane = Instantiate(_airplanePrefab, _airplaneParent);
			_liveAirPlanes.Add(newAirPlane);
			var airPlane = newAirPlane.GetComponent<Airplane>();
			airPlane.Spawn(_camera);
			airPlane.JustShoot += CreateProjectile;
			AirplaneCreated?.Invoke(newAirPlane);
			airPlane.Destroyed += () =>
			{
				_liveAirPlanes.Remove(newAirPlane);
			};
		}

		private bool CanCreateAirPlanes()
		{
			return _liveAirPlanes.Count == 0;
		}

		private void CreateProjectile(Transform shooter)
		{
			var newProjectile = Instantiate(_airplaneProjectile, _airplaneParent);
			_liveProjectiles.Add(newProjectile);
			var projectile = newProjectile.GetComponent<Projectile>();
			projectile.Spawn(_camera, shooter.position);
			projectile.Collided += ProjectileCollided;
			ProjectileCreated?.Invoke(newProjectile);
		}
	}
}