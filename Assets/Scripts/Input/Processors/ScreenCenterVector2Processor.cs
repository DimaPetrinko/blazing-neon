using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input.Processors
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public sealed class ScreenCenterVector2Processor : InputProcessor<Vector2>
	{
#if UNITY_EDITOR
		static ScreenCenterVector2Processor()
		{
			Initialize();
		}
#endif

		[RuntimeInitializeOnLoadMethod]
		static void Initialize() => InputSystem.RegisterProcessor<ScreenCenterVector2Processor>();

		public override Vector2 Process(Vector2 value, InputControl control)
		{
			return new Vector2(value.x - Screen.width/2, value.y - Screen.height/2);
		}
	}
}