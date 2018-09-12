using UnityEngine;

namespace Helpers
{
	public class RunInBackground : MonoBehaviour
	{
		[SerializeField] private bool runInBackground;

		private void Awake()
		{
			Application.runInBackground = runInBackground;
		}
	}
}