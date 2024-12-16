using Crazyminds.AwesomeMask;
using System.Collections.Generic;
using UnityEngine;

namespace Crazyminds.AwesomeMask.Samples.AisPlanes
{
	public class MasksUI : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private AwesomeMask _awesomeMask;
		[SerializeField] private Camera _camera;
		[SerializeField] private Transform _instanceParent;

		[Header("Masks")]
		[SerializeField] private AirplaneFollower _airplaneFollower;
		[SerializeField] private AirplaneFollower _projectileFollower;

		List<AirplaneFollower> _followers = new List<AirplaneFollower>();


		public void ShowAirplaneMask(GameObject airplane)
		{
			var newInstance = Instantiate(_airplaneFollower, _instanceParent);
			newInstance.transform.SetAsFirstSibling();
			newInstance.Show(airplane, _camera, _awesomeMask);
			newInstance.AboutToDestroy += () =>
			{
				_followers.Remove(newInstance);
			};
			_followers.Add(newInstance);
		}

		public void ShowProjectileMask(GameObject projectile)
		{
			var newInstance = Instantiate(_projectileFollower, _instanceParent);
			newInstance.transform.SetAsFirstSibling();
			newInstance.Show(projectile, _camera, _awesomeMask);
			newInstance.AboutToDestroy += () =>
			{
				_followers.Remove(newInstance);
			};
			_followers.Add(newInstance);
		}

		public void GameOver()
		{
			foreach (var follower in _followers)
			{
				_awesomeMask.Remove(follower.GetComponent<AwesomeMaskHollow>());
				Destroy(follower.gameObject);
			}

			_followers.Clear();
		}
	}
}