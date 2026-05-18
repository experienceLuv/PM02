using System;
using System.Collections.Generic;

namespace TechnologistWpf.Models
{
    public class LoginResponse { public string Token { get; set; } public string FullName { get; set; } public int RoleId { get; set; } }
    public class Product { public int Id { get; set; } public string Code { get; set; } public string Name { get; set; } public string Type { get; set; } public string Form { get; set; } public int? StatusId { get; set; } }
    public class RecipeVersion { public int Id { get; set; } public int ProductId { get; set; } public int Version { get; set; } public int StatusId { get; set; } public bool IsActive { get; set; } public DateTime CreatedAt { get; set; } public DateTime? ApprovedAt { get; set; } }
    public class TechnologicalMap { public int Id { get; set; } public int ProductId { get; set; } public int Version { get; set; } public string Name { get; set; } public int StatusId { get; set; } public bool IsActive { get; set; } public DateTime CreatedAt { get; set; } }
    public class ProductionOrder { public int Id { get; set; } public string OrderNumber { get; set; } public int RecipeId { get; set; } public decimal PlannedQuantityKg { get; set; } public int StatusId { get; set; } public DateTime? PlannedStartDate { get; set; } }
    public class Batch { public int Id { get; set; } public string BatchNumber { get; set; } public int OrderId { get; set; } public DateTime? StartTime { get; set; } public DateTime? EndTime { get; set; } public int StatusId { get; set; } public decimal? ActualQuantityKg { get; set; } }
    public class AuditLog { public int Id { get; set; } public string EntityType { get; set; } public int EntityId { get; set; } public string Action { get; set; } public DateTime ChangedAt { get; set; } public string NewValue { get; set; } }
    public class PaginatedResponse<T> { public List<T> Items { get; set; } public int TotalCount { get; set; } public int Page { get; set; } public int PageSize { get; set; } }
}