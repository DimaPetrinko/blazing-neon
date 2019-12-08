using System;
using Game.GameSystemServices.DataReferences;

namespace Game.GameSystemServices.TransformProviders
{
	[Serializable]
	public sealed class TransformProviderReference : DataReference<BaseComponent>
	{
		private ITransformProvider _transformProvider;

		public new ITransformProvider Value
		{
			get => _value != null ? (_value?.BaseInstance as ISceneObject)?.TransformProvider : _transformProvider;
			set
			{
				if (_value != null) (_value?.BaseInstance as ISceneObject).TransformProvider = value;
				_transformProvider = value;
			}
		}

		public TransformProviderReference(BaseComponent defaultValue) : base(defaultValue) {}
		public TransformProviderReference(ITransformProvider defaultValue) => _transformProvider = defaultValue;
	}
}