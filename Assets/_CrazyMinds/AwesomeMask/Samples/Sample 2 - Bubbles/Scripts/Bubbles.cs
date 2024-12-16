using UnityEngine;
using UnityEngine.UI;

namespace Crazyminds.AwesomeMask.Samples.Bubbles
{
	public class Bubbles : MonoBehaviour
	{
		[Header("Bubbles")]
		[SerializeField] private RectTransform _bubbleParent;
		[SerializeField] private GameObject _circlePrefab;
		[SerializeField] private GameObject _starPrefab;
		[SerializeField] private GameObject _bonePrefab;

		[Header("Left Buttons")]
		[SerializeField] private GameObject _circleButton;
		[SerializeField] private GameObject _starButton;
		[SerializeField] private GameObject _boneButton;


		[Header("Other")]
		[SerializeField] private GameObject _tuto;
		[SerializeField] private RectTransform _bubbleButton;

		float _tutoTimer = 0;
		int _bubbleType = 0; // 0 == circle, 1 == star, 2 == bone

		void Update()
		{
			TutorialTimer();
		}

		private void TutorialTimer()
		{
			_tutoTimer += Time.deltaTime;
			_tuto.SetActive(_tutoTimer > 5);
		}

		private void GenerateBubble()
		{
			GameObject prefab = _circlePrefab;
			switch (_bubbleType)
			{
				case 0:
					{
						prefab = _circlePrefab;
						break;
					}
				case 1:
					{
						prefab = _starPrefab;
						break;
					}
				case 2:
					{
						prefab = _bonePrefab;
						break;
					}
			}

			var newBubble = Instantiate(prefab, _bubbleButton.position, Quaternion.identity, _bubbleParent);
			newBubble.name = "bubble";
			newBubble.SetActive(true);
		}

		public void OnBubbleButtonPressed()
		{
			_tutoTimer = 0;
			_tuto.SetActive(false);
			GenerateBubble();
		}

		public void OnCircleButtonPressed()
		{
			_bubbleType = 0;
			_circleButton.GetComponent<Image>().enabled = true;
			_starButton.GetComponent<Image>().enabled = false;
			_boneButton.GetComponent<Image>().enabled = false;
		}

		public void OnStarButtonPressed()
		{
			_bubbleType = 1;
			_circleButton.GetComponent<Image>().enabled = false;
			_starButton.GetComponent<Image>().enabled = true;
			_boneButton.GetComponent<Image>().enabled = false;
		}

		public void OnBoneButtonPressed()
		{
			_bubbleType = 2;
			_circleButton.GetComponent<Image>().enabled = false;
			_starButton.GetComponent<Image>().enabled = false;
			_boneButton.GetComponent<Image>().enabled = true;
		}
	}
}