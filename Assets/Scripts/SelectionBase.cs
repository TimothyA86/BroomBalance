using UnityEngine;

namespace Helpers
{
	[SelectionBase]
	public class SelectionBase : MonoBehaviour
	{
		private void OnEnable()
		{
			var baseClass = new BaseClass();
			var derivedClass = new DerivedClass();

			print("Enabling base");
			baseClass.OnEnable(derivedClass);
			print("Enabling derived");
			derivedClass.OnEnable();
		}
	}

	public class BaseClass
	{
		//public DerivedClass super = new DerivedClass();

		public virtual void OnEnable(DerivedClass super = null)
		{
			Debug.Log("Base -> super.info =  " + super.info);
			Debug.Log("Calling derived OnEnable");
			super.OnEnable();
		}
	}

	public class DerivedClass : BaseClass
	{
		public int info = 1;

		public override void OnEnable(DerivedClass super = null)
		{
			Debug.Log("Derived");
		}
	}
}