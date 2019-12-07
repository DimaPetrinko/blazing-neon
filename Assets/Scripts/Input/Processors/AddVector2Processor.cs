using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Processors
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public sealed class AddVector2Processor : InputProcessor<Vector2>
	{
		[Tooltip("A value to add to the incoming Vector2's X component.")]
		public float _x = 0;

		[Tooltip("A value to add to the incoming Vector2's Y component.")]
		public float _y = 0;

#if UNITY_EDITOR
		static AddVector2Processor()
		{
			Initialize();
		}
#endif

		[RuntimeInitializeOnLoadMethod]
		static void Initialize() => InputSystem.RegisterProcessor<AddVector2Processor>();

		public override Vector2 Process(Vector2 value, InputControl control)
		{
			return new Vector2(value.x + _x, value.y + _y);
		}
	}
}