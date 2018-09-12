using UnityEngine;

namespace BroomBalance
{
	[RequireComponent(typeof(Rigidbody))]
	public class Balancer : MonoBehaviour
	{
		[SerializeField] private Broom broom;

		public Broom Broom => broom;
		public Rigidbody Rigidbody { get; private set; }

		private void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
			InstantiateBroom();
		}

		private void InstantiateBroom()
		{
			broom = Instantiate(broom, transform.position, Quaternion.identity);
			broom.Joint.connectedBody = Rigidbody;
		}

		public void Dispose()
		{
			broom?.Disconnect();
			Destroy(gameObject);
		}
	}
}