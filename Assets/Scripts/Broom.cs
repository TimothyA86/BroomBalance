using UnityEngine;

namespace BroomBalance
{
	[RequireComponent(typeof(HingeJoint))]
	public class Broom : MonoBehaviour
	{
		[SerializeField] private float destroyDelay;

		public HingeJoint Joint { get; private set; }

		private void Awake()
		{
			Joint = GetComponent<HingeJoint>();
		}

		public void Disconnect()
		{
			Joint.breakForce = 0f;
			Joint.breakTorque = 0f;
			Destroy(gameObject, destroyDelay);
		}
	}
}