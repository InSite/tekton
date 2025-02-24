using Microsoft.EntityFrameworkCore;

using Tek.Service.Bus;
using Tek.Service.Contact;
using Tek.Service.Content;
using Tek.Service.Metadata;
using Tek.Service.Security;

namespace Tek.Service;

internal class TableDbContext : DbContext
{
    internal TableDbContext(DbContextOptions options) : base(options) { }

    #region Storage Tables

    // Application: Contact
    internal DbSet<TCountryEntity> TCountry { get; set; }
    internal DbSet<TProvinceEntity> TProvince { get; set; }

    // Application: Content
    internal DbSet<TTranslationEntity> TTranslation { get; set; }

    // Utility: Bus
    internal DbSet<TAggregateEntity> TAggregate { get; set; }
    internal DbSet<TEventEntity> TEvent { get; set; }

    // Utility: Metadata
    internal DbSet<TOriginEntity> TOrigin { get; set; }
    internal DbSet<TVersionEntity> TVersion { get; set; }

    // Utility: Security
    internal DbSet<TOrganizationEntity> TOrganization { get; set; }
    internal DbSet<TPartitionEntity> TPartition { get; set; }
    internal DbSet<TPasswordEntity> TPassword { get; set; }
    internal DbSet<TPermissionEntity> TPermission { get; set; }
    internal DbSet<TResourceEntity> TResource { get; set; }
    internal DbSet<TRoleEntity> TRole { get; set; }



    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ApplyConfigurations(builder);
        ApplyNavigations(builder);

        var decimalProperties = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

        foreach (var property in decimalProperties)
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }
    }

    private void ApplyConfigurations(ModelBuilder builder)
    {
        // Application: Contact
        builder.ApplyConfiguration(new TCountryEntityConfiguration());
        builder.ApplyConfiguration(new TProvinceEntityConfiguration());

        // Application: Content
        builder.ApplyConfiguration(new TTranslationEntityConfiguration());

        // Utility: Bus
        builder.ApplyConfiguration(new TAggregateEntityConfiguration());
        builder.ApplyConfiguration(new TEventEntityConfiguration());

        // Utility: Metadata
        builder.ApplyConfiguration(new TOriginEntityConfiguration());
        builder.ApplyConfiguration(new TVersionEntityConfiguration());

        // Utility: Security
        builder.ApplyConfiguration(new TOrganizationEntityConfiguration());
        builder.ApplyConfiguration(new TPartitionEntityConfiguration());
        builder.ApplyConfiguration(new TPasswordEntityConfiguration());
        builder.ApplyConfiguration(new TPermissionEntityConfiguration());
        builder.ApplyConfiguration(new TResourceEntityConfiguration());
        builder.ApplyConfiguration(new TRoleEntityConfiguration());


    }

    private void ApplyNavigations(ModelBuilder builder)
    {
        // builder.Entity<PrimaryEntity>()
        //     .HasMany(e => e.ForeignProperty)
        //     .WithOne(e => e.PrimaryProperty)
        //     .HasForeignKey(e => e.ForeignIdentifier)
        //     .HasPrincipalKey(e => e.PrimaryIdentifier);
    }
}