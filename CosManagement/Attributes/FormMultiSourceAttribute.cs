using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CosManagement.Attributes
{
	public class FormMultiSourceAttribute : Attribute, IBindingSourceMetadata
	{
		public BindingSource? BindingSource { get; } = CompositeBindingSource.Create(
			new[] { BindingSource.Path, BindingSource.Query },
			nameof(FormMultiSourceAttribute));
	}
}