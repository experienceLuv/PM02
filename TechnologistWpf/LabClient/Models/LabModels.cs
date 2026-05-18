using System;
using System.Collections.Generic;

namespace LabClient.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
    }

    public class RawMaterialLot
    {
        public int Id { get; set; }
        public int RawMaterialId { get; set; }
        public string LotNumber { get; set; }
        public string Supplier { get; set; }
        public DateTime ReceiptDate { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public int StatusId { get; set; }
    }

    public class LabTest
    {
        public int Id { get; set; }
        public int? BatchId { get; set; }
        public int? RawMaterialLotId { get; set; }
        public DateTime AnalysisDate { get; set; }
        public string SampleType { get; set; }
        public string ParameterName { get; set; }
        public decimal MeasuredValue { get; set; }
        public string StandardValue { get; set; }
        public string Unit { get; set; }
        public string Result { get; set; }
        public string Decision { get; set; }
        public string AnalystComment { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}