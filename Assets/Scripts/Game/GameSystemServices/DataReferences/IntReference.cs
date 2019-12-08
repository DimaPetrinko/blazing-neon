using System;

namespace Game.GameSystemServices.DataReferences
{
	[Serializable]
	public sealed class IntReference : DataReference<float>
	{
		public IntReference(float defaultValue) : base(defaultValue) {}
	}

	[Serializable]
	public sealed class ReadonlyIntReference : ReadonlyDataReference<float>
	{
		public ReadonlyIntReference(float defaultValue) : base(defaultValue) {}
	}
}