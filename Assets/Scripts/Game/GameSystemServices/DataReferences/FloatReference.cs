using System;

namespace Game.GameSystemServices.DataReferences
{
	[Serializable]
	public sealed class FloatReference : DataReference<float>
	{
		public FloatReference(float defaultValue) : base(defaultValue) {}
	}

	[Serializable]
	public sealed class ReadonlyFloatReference : ReadonlyDataReference<float>
	{
		public ReadonlyFloatReference(float defaultValue) : base(defaultValue) {}
	}
}