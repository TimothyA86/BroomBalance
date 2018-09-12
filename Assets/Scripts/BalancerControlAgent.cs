using UnityEngine;
using MH = Helpers.MathHelper;

namespace BroomBalance
{
	public class BalancerControlAgent : Agent
	{
		#region Fields and Properties
		private const float FieldWidth = 16f;
		private const float BroomAngleThreshold = 80f;
		private const float DistanceThreshold = 1f;
		private const float DeltaToDestinationNormalizer = FieldWidth + 5f;
		private const float VelocityNormalizer = 30f;
		private const float BroomAngularVelocityNormalizer = 375f;

		[SerializeField] private Balancer balancerOriginal;
		[SerializeField] private Transform destination;
		[SerializeField] private float acceleration;
		private Balancer balancer;
		#endregion

		#region Agent
		public override void AgentReset()
		{
			InstantiateNewBalancer();
			ResetDestination();
		}

		public override void CollectObservations()
		{
			float deltaToDestination = destination.transform.position.x - balancer.transform.position.x;
			float velocity = balancer.Rigidbody.velocity.x;
			float broomAngle = balancer.Broom.Joint.angle;
			float broomAngleVelocity = balancer.Broom.Joint.velocity;
			
			AddVectorObs(deltaToDestination / DeltaToDestinationNormalizer);
			AddVectorObs(velocity / VelocityNormalizer);
			AddVectorObs(broomAngle / BroomAngleThreshold);
			AddVectorObs(broomAngleVelocity / BroomAngularVelocityNormalizer);
		}

		public override void AgentAction(float[] vectorAction, string textAction)
		{
			ApplyReward();			

			float controlSignal = Mathf.Clamp(vectorAction[0], -1f, 1f);
			balancer.Rigidbody.AddForce(controlSignal * acceleration * Vector3.right, ForceMode.VelocityChange);
		}
		#endregion

		#region Reward Functions
		private void ApplyReward()
		{
			float delta, distance;
			CalculateDestinationData(out delta, out distance);

			float angleSign, velocitySign;
			bool leavingBroomBehind = LeavingBroomBehind(out angleSign, out velocitySign);

			float penalty = velocitySign == MH.Sign(delta)
				? leavingBroomBehind
					? -0.0005f
					: -0.00005f
				: leavingBroomBehind
					? -0.00005f
					: -0.005f;

			AddReward(penalty);

			if (distance < DistanceThreshold)
			{
				ResetDestination();
				SetReward(1f);
			}

			if (CheckFailure())
			{
				Done();
				SetReward(-1f);
			}
		}
		#endregion

		#region Other
		private bool LeavingBroomBehind(out float angleSign, out float velocitySign)
		{
			angleSign = MH.Sign(balancer.Broom.Joint.angle);
			velocitySign = MH.Sign(balancer.Rigidbody.velocity.x);
			return angleSign == velocitySign;
		}

		private void CalculateDestinationData(out float delta, out float distance)
		{
			delta = destination.transform.position.x - balancer.transform.position.x;
			distance =  Mathf.Abs(delta);
		}

		private bool CheckFailure()
		{
			float angle = balancer.Broom.Joint.angle;
			return (BroomAngleThreshold < angle || angle < -BroomAngleThreshold);
		}

		private void InstantiateNewBalancer()
		{
			balancer?.Dispose();
			balancer = Instantiate(balancerOriginal, transform.position, Quaternion.identity);
		}

		private void ResetDestination()
		{
			var position = transform.position;
			position.x += FieldWidth * Random.value - FieldWidth / 2f;
			destination.transform.position = position;
		}
		#endregion
	}
}