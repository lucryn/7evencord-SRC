using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000019 RID: 25
	internal abstract class JsonSerializerInternalBase
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00007C02 File Offset: 0x00005E02
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00007C0A File Offset: 0x00005E0A
		internal JsonSerializer Serializer { get; private set; }

		// Token: 0x06000149 RID: 329 RVA: 0x00007C13 File Offset: 0x00005E13
		protected JsonSerializerInternalBase(JsonSerializer serializer)
		{
			ValidationUtils.ArgumentNotNull(serializer, "serializer");
			this.Serializer = serializer;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00007C2D File Offset: 0x00005E2D
		internal BidirectionalDictionary<string, object> DefaultReferenceMappings
		{
			get
			{
				if (this._mappings == null)
				{
					this._mappings = new BidirectionalDictionary<string, object>(EqualityComparer<string>.Default, new JsonSerializerInternalBase.ReferenceEqualsEqualityComparer());
				}
				return this._mappings;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007C52 File Offset: 0x00005E52
		protected ErrorContext GetErrorContext(object currentObject, object member, Exception error)
		{
			if (this._currentErrorContext == null)
			{
				this._currentErrorContext = new ErrorContext(currentObject, member, error);
			}
			if (this._currentErrorContext.Error != error)
			{
				throw new InvalidOperationException("Current error context error is different to requested error.");
			}
			return this._currentErrorContext;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00007C89 File Offset: 0x00005E89
		protected void ClearErrorContext()
		{
			if (this._currentErrorContext == null)
			{
				throw new InvalidOperationException("Could not clear error context. Error context is already null.");
			}
			this._currentErrorContext = null;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007CA8 File Offset: 0x00005EA8
		protected bool IsErrorHandled(object currentObject, JsonContract contract, object keyValue, Exception ex)
		{
			ErrorContext errorContext = this.GetErrorContext(currentObject, keyValue, ex);
			contract.InvokeOnError(currentObject, this.Serializer.Context, errorContext);
			if (!errorContext.Handled)
			{
				this.Serializer.OnError(new ErrorEventArgs(currentObject, errorContext));
			}
			return errorContext.Handled;
		}

		// Token: 0x04000097 RID: 151
		private ErrorContext _currentErrorContext;

		// Token: 0x04000098 RID: 152
		private BidirectionalDictionary<string, object> _mappings;

		// Token: 0x0200001A RID: 26
		private class ReferenceEqualsEqualityComparer : IEqualityComparer<object>
		{
			// Token: 0x0600014E RID: 334 RVA: 0x00007CF3 File Offset: 0x00005EF3
			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}

			// Token: 0x0600014F RID: 335 RVA: 0x00007CFC File Offset: 0x00005EFC
			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				return RuntimeHelpers.GetHashCode(obj);
			}
		}
	}
}
