using UnityEngine;

namespace Crazyminds.AwesomeMask.Samples.AisPlanes
{
	public class Game : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private Airplanes _airplanes;
		[SerializeField] private MasksUI _masks;
		[SerializeField] private HudUI _hud;

		[Header("Settings")]
		[SerializeField] private int _startingLifeCount = 10;

		private int _lifesRemaining = 0;

		void Awake()
		{
			_airplanes.AirplaneCreated += OnAirplaneCreated;
			_airplanes.ProjectileCreated += OnProjectileCreated;
			_airplanes.ProjectileCollided += OnProjectileCollided;

			_lifesRemaining = _startingLifeCount;
			_hud.Configure(_startingLifeCount);
		}

		private void OnAirplaneCreated(GameObject newAirplane)
		{
			_masks.ShowAirplaneMask(newAirplane);
		}
		private void OnProjectileCreated(GameObject newAirplane)
		{
			_masks.ShowProjectileMask(newAirplane);
		}

		private void OnProjectileCollided()
		{
			_hud.AddCollision();
			_lifesRemaining -= 1;
			if (_lifesRemaining == 0)
			{
				// game over
				_hud.ShowGameOver();
				_airplanes.GameOver();
				_masks.GameOver();
			}

		}

	}
}