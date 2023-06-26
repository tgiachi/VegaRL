using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Furniture;
using Vega.Api.Data.Entities.Items;
using Vega.Api.Map.GameObjects.Furniture;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(5)]
public class FurnitureService : BaseDataLoaderVegaService<FurnitureService>, IFurnitureService
{
    private readonly ITileService _tileService;
    private readonly IItemService _itemService;
    private readonly Dictionary<string, FurnitureClassEntity> _furnitureClasses = new();
    private readonly Dictionary<string, FurnitureEntity> _furniture = new();
    private readonly Dictionary<string, FurnitureGroupEntity> _furnitureGroups = new();

    public FurnitureService(
        ILogger<FurnitureService> logger, IDataService dataService, IMessageBusService messageBusService,
        ITileService tileService, IItemService itemService
    ) : base(logger, dataService, messageBusService)
    {
        _tileService = tileService;
        _itemService = itemService;
    }

    public override async Task<bool> LoadAsync()
    {
        await LoadFurnitureClasses();
        await LoadFurniture();
        await LoadFurnitureGroups();

        return true;
    }

    public FurnitureEntity GetFurniture(string id)
    {
        if (_furniture.TryGetValue(id.ToLower(), out var furniture))
        {
            var furnitureEntity = new FurnitureEntity()
            {
                Id = furniture.Id,
                Sym = furniture.Sym,
                Background = furniture.Background,
                Foreground = furniture.Foreground,
                IsWalkable = furniture.IsWalkable,
                IsTransparent = furniture.IsTransparent,
                Category = furniture.Category,
                SubCategory = furniture.SubCategory,
                Weight = furniture.Weight,
                Container = furniture.Container,
                Flags = furniture.Flags,
                Name = furniture.Name,
                Description = furniture.Description,
                Comment = furniture.Comment,
                FurnitureClassId = furniture.FurnitureClassId
            };

            if (furniture.FurnitureClassId != null)
            {
                if (_furnitureClasses.TryGetValue(furniture.FurnitureClassId.ToLower(), out var furnitureClass))
                {
                    furnitureEntity.FurnitureClassId = furnitureClass.Id;
                    furnitureEntity.IsTransparent = furnitureClass.IsTransparent;
                    furnitureEntity.IsWalkable = furnitureClass.IsWalkable;
                    furnitureEntity.Sym = furnitureClass.Sym ?? furniture.Sym;
                    furnitureEntity.Background = furnitureClass.Background ?? furniture.Background;
                    furnitureEntity.Foreground = furnitureClass.Foreground ?? furniture.Foreground;
                    furnitureEntity.Category = furnitureClass.Category ?? furniture.Category;
                    furnitureEntity.SubCategory = furnitureClass.SubCategory ?? furniture.SubCategory;
                    furnitureEntity.Weight = furnitureClass.Weight ?? furniture.Weight;
                    furnitureEntity.Container = furnitureClass.Container ?? furniture.Container;
                    furnitureEntity.Flags = furnitureClass.Flags ?? furniture.Flags;
                    furnitureEntity.Weight = furnitureClass.Weight ?? furniture.Weight;
                }
                else
                {
                    throw new Exception($"Furniture class with id {furniture.FurnitureClassId} not found.");
                }
            }

            return furnitureEntity;
        }

        throw new Exception($"Furniture with id {id} not found.");
    }

    public FurnitureGameObject CreateFurnitureGameObject(string id, Point position)
    {
        if (_furniture.TryGetValue(id.ToLower(), out var furnitureEntity))
        {
            var coloredTile = _tileService.GetTile(furnitureEntity);
            var furnitureGameObject = new FurnitureGameObject(
                id,
                furnitureEntity.Sym,
                coloredTile.Item1,
                position,
                coloredTile.isWalkable,
                coloredTile.isTransparent
            );
            if (furnitureEntity.Container != null)
            {
                var items = _itemService.GetItemsFromDrop(
                    furnitureEntity.Container.Select(
                            s => new ItemDropEntity()
                            {
                                ItemId = s.Key,
                                Count = s.Value.Count,
                                Probability = s.Value.Probability,
                                Dice = s.Value.Dice,
                                Range = s.Value.Range
                            }
                        )
                        .ToList()
                );

                furnitureGameObject.ContainerItems.AddRange(items);
            }

            return furnitureGameObject;
        }

        throw new Exception($"Furniture with id {id} not found.");
    }

    private Task LoadFurnitureClasses()
    {
        foreach (var furnitureClass in LoadData<FurnitureClassEntity>())
        {
            _furnitureClasses.Add(furnitureClass.Id.ToLower(), furnitureClass);
        }

        return Task.CompletedTask;
    }

    private Task LoadFurniture()
    {
        foreach (var furniture in LoadData<FurnitureEntity>())
        {
            _furniture.Add(furniture.Id.ToLower(), furniture);
        }

        return Task.CompletedTask;
    }

    private Task LoadFurnitureGroups()
    {
        foreach (var furnitureGroup in LoadData<FurnitureGroupEntity>())
        {
            try
            {
                foreach (var furnitureId in furnitureGroup.Ids)
                {
                    if (!_furniture.TryGetValue(furnitureId.ToLower(), out var furniture))
                    {
                        throw new Exception($"Furniture with id {furnitureId} not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error loading furniture group {furnitureGroup.Id}: Error: {ex.Message}");
            }
            finally
            {
                _furnitureGroups.Add(furnitureGroup.Id.ToLower(), furnitureGroup);
            }
        }

        return Task.CompletedTask;
    }
}
