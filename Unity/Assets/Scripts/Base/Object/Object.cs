﻿using System.ComponentModel;

namespace ETModel
{
	public abstract class Object: EventDispatcherInterface,ISupportInitialize
	{
		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
		}

		public override string ToString()
		{
			return JsonHelper.ToJson(this);
		}
	}
}