using System;
using UnityEngine;

namespace Game.GameSystemServices.DataReferences
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
		protected DataReference() {}
	}

	public abstract class ReadonlyDataReference<T> : BaseDataReference<T>
	{
		public T Value => _value;
		protected ReadonlyDataReference(T defaultValue) => _value = defaultValue;
	}
}