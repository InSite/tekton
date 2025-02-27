namespace Tek.Service.Metadata;

public class TEntityAdapter : IEntityAdapter
{
    public void Copy(ModifyEntity modify, TEntityEntity entity)
    {
        entity.ComponentType = modify.ComponentType;
        entity.ComponentName = modify.ComponentName;
        entity.ComponentFeature = modify.ComponentFeature;
        entity.EntityName = modify.EntityName;
        entity.CollectionSlug = modify.CollectionSlug;
        entity.CollectionKey = modify.CollectionKey;
        entity.StorageStructure = modify.StorageStructure;
        entity.StorageSchema = modify.StorageSchema;
        entity.StorageTable = modify.StorageTable;
        entity.StorageKey = modify.StorageKey;
        entity.FutureStorageTable = modify.FutureStorageTable;

    }

    public TEntityEntity ToEntity(CreateEntity create)
    {
        var entity = new TEntityEntity
        {
            ComponentType = create.ComponentType,
            ComponentName = create.ComponentName,
            ComponentFeature = create.ComponentFeature,
            EntityId = create.EntityId,
            EntityName = create.EntityName,
            CollectionSlug = create.CollectionSlug,
            CollectionKey = create.CollectionKey,
            StorageStructure = create.StorageStructure,
            StorageSchema = create.StorageSchema,
            StorageTable = create.StorageTable,
            StorageKey = create.StorageKey,
            FutureStorageTable = create.FutureStorageTable
        };
        return entity;
    }

    public IEnumerable<EntityModel> ToModel(IEnumerable<TEntityEntity> entities)
    {
        return entities.Select(ToModel);
    }

    public EntityModel ToModel(TEntityEntity entity)
    {
        var model = new EntityModel
        {
            ComponentType = entity.ComponentType,
            ComponentName = entity.ComponentName,
            ComponentFeature = entity.ComponentFeature,
            EntityId = entity.EntityId,
            EntityName = entity.EntityName,
            CollectionSlug = entity.CollectionSlug,
            CollectionKey = entity.CollectionKey,
            StorageStructure = entity.StorageStructure,
            StorageSchema = entity.StorageSchema,
            StorageTable = entity.StorageTable,
            StorageKey = entity.StorageKey,
            FutureStorageTable = entity.FutureStorageTable
        };

        return model;
    }

    public IEnumerable<EntityMatch> ToMatch(IEnumerable<TEntityEntity> entities)
    {
        return entities.Select(ToMatch);
    }

    public EntityMatch ToMatch(TEntityEntity entity)
    {
        var match = new EntityMatch
        {
            EntityId = entity.EntityId

        };

        return match;
    }
}