using Microsoft.EntityFrameworkCore;
using ProductionApi.Models;

namespace ProductionApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<RecipeVersion> RecipeVersions { get; set; }
        public DbSet<RecipeComponent> RecipeComponents { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<BatchStepExecution> BatchStepExecutions { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<TechnologicalMap> TechnologicalMaps { get; set; }
        public DbSet<TechnologicalMapStep> TechnologicalMapSteps { get; set; }
        public DbSet<RawMaterialLot> RawMaterialLots { get; set; }
        public DbSet<Equipment> Equipment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // users
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.Property(e => e.Login).HasColumnName("login");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.FullName).HasColumnName("full_name");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
            });

            // statuses
            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("statuses");
                entity.Property(e => e.EntityType).HasColumnName("entity_type");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
            });

            // products
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.Form).HasColumnName("form");
                entity.Property(e => e.StatusId).HasColumnName("status_id");
            });

            // raw_materials
            modelBuilder.Entity<RawMaterial>(entity =>
            {
                entity.ToTable("raw_materials");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.MaterialType).HasColumnName("material_type");
                entity.Property(e => e.Unit).HasColumnName("unit");
            });

            // recipe_versions
            modelBuilder.Entity<RecipeVersion>(entity =>
            {
                entity.ToTable("recipe_versions");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.Version).HasColumnName("version");
                entity.Property(e => e.StatusId).HasColumnName("status_id");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.ApprovedAt).HasColumnName("approved_at");
            });

            // recipe_components
            modelBuilder.Entity<RecipeComponent>(entity =>
            {
                entity.ToTable("recipe_components");
                entity.Property(e => e.RecipeVersionId).HasColumnName("recipe_version_id");
                entity.Property(e => e.RawMaterialId).HasColumnName("raw_material_id");
                entity.Property(e => e.Percentage).HasColumnName("percentage");
                entity.Property(e => e.LoadOrder).HasColumnName("load_order");
                entity.Property(e => e.ToleranceMin).HasColumnName("tolerance_min");
                entity.Property(e => e.ToleranceMax).HasColumnName("tolerance_max");
            });

            // production_orders
            modelBuilder.Entity<ProductionOrder>(entity =>
            {
                entity.ToTable("production_orders");
                entity.Property(e => e.OrderNumber).HasColumnName("order_number");
                entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
                entity.Property(e => e.PlannedQuantityKg).HasColumnName("planned_quantity_kg");
                entity.Property(e => e.StatusId).HasColumnName("status_id");
                entity.Property(e => e.PlannedStartDate).HasColumnName("planned_start_date");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            // batches
            modelBuilder.Entity<Batch>(entity =>
            {
                entity.ToTable("batches");
                entity.Property(e => e.BatchNumber).HasColumnName("batch_number");
                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.StartTime).HasColumnName("start_time");
                entity.Property(e => e.EndTime).HasColumnName("end_time");
                entity.Property(e => e.StatusId).HasColumnName("status_id");
                entity.Property(e => e.ActualQuantityKg).HasColumnName("actual_quantity_kg");
            });

            // batch_step_executions
            modelBuilder.Entity<BatchStepExecution>(entity =>
            {
                entity.ToTable("batch_step_executions");
                entity.Property(e => e.BatchId).HasColumnName("batch_id");
                entity.Property(e => e.StepOrder).HasColumnName("step_order");
                entity.Property(e => e.StepName).HasColumnName("step_name");
                entity.Property(e => e.PlannedTempC).HasColumnName("planned_temp_c");
                entity.Property(e => e.ActualTempC).HasColumnName("actual_temp_c");
                entity.Property(e => e.PlannedDurationMin).HasColumnName("planned_duration_min");
                entity.Property(e => e.ActualDurationMin).HasColumnName("actual_duration_min");
                entity.Property(e => e.PlannedPressureBar).HasColumnName("planned_pressure_bar");
                entity.Property(e => e.ActualPressureBar).HasColumnName("actual_pressure_bar");
                entity.Property(e => e.DeviationFlag).HasColumnName("deviation_flag");
                entity.Property(e => e.OperatorComment).HasColumnName("operator_comment");
            });

            // lab_tests
            modelBuilder.Entity<LabTest>(entity =>
            {
                entity.ToTable("lab_tests");
                entity.Property(e => e.BatchId).HasColumnName("batch_id");
                entity.Property(e => e.RawMaterialLotId).HasColumnName("raw_material_lot_id");
                entity.Property(e => e.AnalysisDate).HasColumnName("analysis_date");
                entity.Property(e => e.SampleType).HasColumnName("sample_type");
                entity.Property(e => e.ParameterName).HasColumnName("parameter_name");
                entity.Property(e => e.MeasuredValue).HasColumnName("measured_value");
                entity.Property(e => e.StandardValue).HasColumnName("standard_value");
                entity.Property(e => e.Unit).HasColumnName("unit");
                entity.Property(e => e.Result).HasColumnName("result");
                entity.Property(e => e.Decision).HasColumnName("decision");
                entity.Property(e => e.AnalystComment).HasColumnName("analyst_comment");
            });

            // audit_log
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("audit_log");
                entity.Property(e => e.EntityType).HasColumnName("entity_type");
                entity.Property(e => e.EntityId).HasColumnName("entity_id");
                entity.Property(e => e.Action).HasColumnName("action");
                entity.Property(e => e.OldValue).HasColumnName("old_value");
                entity.Property(e => e.NewValue).HasColumnName("new_value");
                entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
                entity.Property(e => e.ChangedAt).HasColumnName("changed_at");
            });

            // technological_maps
            modelBuilder.Entity<TechnologicalMap>(entity =>
            {
                entity.ToTable("technological_maps");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.Version).HasColumnName("version");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.StatusId).HasColumnName("status_id");
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });

            // technological_map_steps
            modelBuilder.Entity<TechnologicalMapStep>(entity =>
            {
                entity.ToTable("technological_map_steps");
                entity.Property(e => e.MapId).HasColumnName("map_id");
                entity.Property(e => e.StepOrder).HasColumnName("step_order");
                entity.Property(e => e.StepName).HasColumnName("step_name");
                entity.Property(e => e.StepType).HasColumnName("step_type");
                entity.Property(e => e.PlannedTempC).HasColumnName("planned_temp_c");
                entity.Property(e => e.PlannedDurationMin).HasColumnName("planned_duration_min");
                entity.Property(e => e.PlannedPressureBar).HasColumnName("planned_pressure_bar");
                entity.Property(e => e.IsMandatory).HasColumnName("is_mandatory");
                entity.Property(e => e.Instruction).HasColumnName("instruction");
                entity.Property(e => e.ToleranceTemp).HasColumnName("tolerance_temp");
                entity.Property(e => e.ToleranceDuration).HasColumnName("tolerance_duration");
                entity.Property(e => e.TolerancePressure).HasColumnName("tolerance_pressure");
            });

            // raw_material_lots
            modelBuilder.Entity<RawMaterialLot>(entity =>
            {
                entity.ToTable("raw_material_lots");
                entity.Property(e => e.RawMaterialId).HasColumnName("raw_material_id");
                entity.Property(e => e.LotNumber).HasColumnName("lot_number");
                entity.Property(e => e.Supplier).HasColumnName("supplier");
                entity.Property(e => e.ReceiptDate).HasColumnName("receipt_date");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.Unit).HasColumnName("unit");
                entity.Property(e => e.StatusId).HasColumnName("status_id");
            });

            // equipment
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("equipment");
                entity.Property(e => e.Code).HasColumnName("code");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.Line).HasColumnName("line");
                entity.Property(e => e.Location).HasColumnName("location");
                entity.Property(e => e.StatusId).HasColumnName("status_id");
            });

            // Настройка отношений (чтобы избежать автоматической генерации неверных внешних ключей)
            modelBuilder.Entity<RecipeVersion>()
                .HasMany(r => r.RecipeComponents)
                .WithOne()
                .HasForeignKey(c => c.RecipeVersionId);

            modelBuilder.Entity<TechnologicalMap>()
                .HasMany(m => m.Steps)
                .WithOne()
                .HasForeignKey(s => s.MapId);

            base.OnModelCreating(modelBuilder);
        }
    }
}