using GoRogue.DiceNotation;
using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Items;
using Vega.Framework.Map.GameObjects.Items;
using Vega.Framework.Utils.Random;

namespace Vega.Engine.Services;

[VegaService(4)]
public class ItemService : BaseDataLoaderVegaService<ItemService>, IItemService
{
    private readonly Dictionary<string, ItemClassEntity> _itemClasses = new();
    private readonly Dictionary<string, ItemEntity> _items = new();
    private readonly Dictionary<string, ItemGroupEntity> _itemGroups = new();

    private readonly ITileService _tileService;

    public ItemService(
        ILogger<ItemService> logger, IDataService dataService, IMessageBusService messageBusService, ITileService tileService
    ) : base(logger, dataService, messageBusService)
    {
        _tileService = tileService;
    }

    public override async Task<bool> LoadAsync()
    {
        await LoadItemClasses();
        await LoadItems();
        await LoadItemGroups();

        return true;
    }

    public ItemGameObject CreateItemGameObject(string itemId, Point position)
    {
        if (_items.TryGetValue(itemId.ToLower(), out var itemEntity))
        {
            var item = GetItemWithClass(itemId);
            var colorAppearance = _tileService.GetTile(item);
            return new ItemGameObject(
                itemEntity.Id,
                itemEntity.Sym,
                itemEntity.Name,
                itemEntity.Description,
                colorAppearance.Item1,
                position,
                colorAppearance.isWalkable,
                colorAppearance.isTransparent
            );
        }

        throw new Exception($" Item with id {itemId} not found.");
    }

    public IEnumerable<ItemGameObject> CreateItemGameObjects(IEnumerable<ItemEntity> items) => items.Select(item => CreateItemGameObject(item.Id, Point.None));

    public IEnumerable<ItemEntity> GetItemsFromDrop(List<ItemDropEntity> itemDrops)
    {
        var resultBag = new List<ItemEntity>();


        return resultBag;
    }

    public IEnumerable<ItemEntity> GetItemsFromItemGroupId(string itemGroupId)
    {
        if (_itemGroups.TryGetValue(itemGroupId.ToLower(), out var itemGroup))
        {
            return itemGroup.Items.BuildPropEntries().Select(GetItemWithClass);
        }

        throw new Exception("Item group not found.");
    }

    public ItemEntity GetItem(string itemId) => GetItemWithClass(itemId);

    public IEnumerable<ItemEntity> FindItemsByCategory(string category, string? subCategory = null)
    {
        return _items.Values.Where(
                item => item.Category.ToLower() == category.ToLower() &&
                        (subCategory == null || item.SubCategory.ToLower() == subCategory.ToLower())
            )
            .ToList();
    }

    private Task LoadItemClasses()
    {
        foreach (var classEntity in LoadData<ItemClassEntity>())
        {
            _itemClasses.Add(classEntity.Id.ToLower(), classEntity);
        }

        return Task.CompletedTask;
    }

    private Task LoadItemGroups()
    {
        foreach (var groupEntity in LoadData<ItemGroupEntity>())
        {
            try
            {
                foreach (var itemId in groupEntity.Items.Items)
                {
                    if (_items.ContainsKey(itemId.Key.ToLower()))
                    {
                        continue;
                    }

                    throw new Exception($" Item with id {itemId} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading item group {groupEntity.Id}", ex);
            }
            finally
            {
                _itemGroups.Add(groupEntity.Id.ToLower(), groupEntity);
            }
        }

        return Task.CompletedTask;
    }

    private Task LoadItems()
    {
        foreach (var itemEntity in LoadData<ItemEntity>())
        {
            _items.Add(itemEntity.Id.ToLower(), itemEntity);
        }

        return Task.CompletedTask;
    }

    private ItemEntity GetItemWithClass(string itemId)
    {
        var item = _items[itemId.ToLower()];
        var itemEntity = new ItemEntity
        {
            Id = item.Id,
            ItemClassId = item.ItemClassId,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            Weight = item.Weight,
            Sym = item.Sym,
            Background = item.Background,
            Foreground = item.Foreground,
            IsWalkable = item.IsWalkable,
            IsTransparent = item.IsTransparent,
            Category = item.Category,
            SubCategory = item.SubCategory,
            Comment = item.Comment,
            Flags = item.Flags
        };
        if (item.ItemClassId != null)
        {
            // Override item properties with class properties
            var itemClass = _itemClasses[item.ItemClassId.ToLower()];

            itemEntity.Background = itemClass.Background ?? item.Background;
            itemEntity.Foreground = itemClass.Foreground ?? item.Foreground;
            itemEntity.Sym = itemClass.Sym ?? item.Sym;
            itemEntity.IsWalkable = itemClass.IsWalkable;
            itemEntity.IsTransparent = itemClass.IsTransparent;
            itemEntity.Category = itemClass.Category ?? item.Category;
            itemEntity.SubCategory = itemClass.SubCategory ?? item.SubCategory;
        }


        return itemEntity;
    }
}
