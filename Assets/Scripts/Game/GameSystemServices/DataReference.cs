using System;
using UnityEngine;

namespace Game.GameSystemServices
{
	[Serializable]
	public abstract class BaseDataReference<T>
	{
		[SerializeField] protected T _value;
	}

	public abstract class DataReference<T> : BaseDataReference<T>
	{
		public event EventHandler ValueChanged;

		public T Value
		{
			get => _value;
			set
			{
				_value = value;
				ValueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		protected DataReference(T defaultValue) => _value = defaultValue;
	}

	public abstract class ReadonlyDataReference<T> : BaseDataReference<T>
	{
		public T Value => _value;
		protected ReadonlyDataReference(T defaultValue) => _value = defaultValue;
	}

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

	[Serializable]
	public sealed class ComponentReference : ReadonlyDataReference<BaseComponent>
	{
		public ComponentReference(BaseComponent defaultValue) : base(defaultValue) {}
	}
}