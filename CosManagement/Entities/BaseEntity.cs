﻿using CosManagement.Interfaces;

namespace CosMeToo.Domain.Common;

public abstract class BaseEntity : IOwned, IResource
{
	public string? OwnerId { get; set; }

	public Guid Id { get; set; }

	public DateTime Created { get; set; }

	public string? CreatedBy { get; set; }

	public DateTime? LastModified { get; set; }

	public string? LastModifiedBy { get; set; }
}