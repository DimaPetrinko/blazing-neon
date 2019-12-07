using System;
using UnityEngine;

namespace Game.GameSystemServices
{
	public abstract class BaseDataReference<T>
	{
		[SerializeField] protected T value;
	}

	public abstract class DataReference<T> : BaseDataReference<T>
	{
		public event EventHandler ValueChanged;
		public T Value
		{
			get => value;
			set
			{
				this.value = value;
				ValueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		protected DataReference(T defaultValue) => value = defaultValue;
	}
	
	public abstract class ReadonlyDataReference<T> : BaseDataReference<T>
	{
		public T Value => value;
		protected ReadonlyDataReference(T defaultValue) => value = defaultValue;
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