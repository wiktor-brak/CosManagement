﻿using AutoMapper.QueryableExtensions;
using CosManagement.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.Mappings;

public static class MappingExtensions
{
	public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
		=> PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
}