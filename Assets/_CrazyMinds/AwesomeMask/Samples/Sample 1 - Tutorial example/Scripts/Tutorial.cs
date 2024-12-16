
using Crazyminds.AwesomeMask;
using System;
using System.Reflection;
using UnityEngine;

namespace Crazyminds.AwesomeMask.Samples.Tutorial
{
	public class Tutorial : MonoBehaviour
	{
		[SerializeField] private GameObject[] transparentElementList;
		[SerializeField] private GameObject[] tutorialElementList;

		private void OnEnable()
		{
			ConfigTutorialElements(0);
			ConfigTransparentElements(0);
			//AwesomeMask 
		}


		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				ConfigTransparentElements(1);
				ConfigTutorialElements(1);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				ConfigTransparentElements(2);
				ConfigTutorialElements(2);
			}
		}

		private void ConfigTransparentElements(int index)
		{
			if (transparentElementList == null)
			{
				return;
			}

			if (transparentElementList.Length >= 0)
			{
				transparentElementList[0].SetActive(index == 0);
			}

			if (transparentElementList.Length >= 1)
			{
				transparentElementList[1].SetActive(index == 1);
			}

			if (transparentElementList.Length >= 2)
			{
				transparentElementList[2].SetActive(index == 2);
			}
		}

		private void ConfigTutorialElements(int index)
		{
			if (tutorialElementList == null)
			{
				return;
			}

			if (tutorialElementList.Length >= 0)
			{
				tutorialElementList[0].SetActive(index == 0);
			}

			if (tutorialElementList.Length >= 1)
			{
				tutorialElementList[1].SetActive(index == 1);
			}

			if (tutorialElementList.Length >= 2)
			{
				tutorialElementList[2].SetActive(index == 2);
			}
		}

		public void OnPriceButtonPressed()
		{
			ConfigTransparentElements(0);
			ConfigTutorialElements(0);
		}
	}
}