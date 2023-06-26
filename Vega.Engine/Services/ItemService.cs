using GoRogue.DiceNotation;
using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using Vega.Api.Attributes;
using Vega.Api.Data.Entities.Items;
using Vega.Api.Map.GameObjects.Items;
using Vega.Api.Utils.Random;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;

namespace Vega.Engine.Services;

[VegaService(5)]
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
                colorAppearance.Item1,
                position,
                colorAppearance.isWalkable,
                colorAppearance.isTransparent
            );
        }

        throw new Exception($" Item with id {itemId} not found.");
    }

    public IEnumerable<ItemEntity> GetItemsFromDrop(List<ItemDropEntity> itemDrops)
    {
        var resultBag = new List<ItemEntity>();

        foreach (var itemDrop in itemDrops.Where(s => s.Probability == null))
        {
            if (itemDrop.Count != null)
            {
                resultBag.AddRange(Enumerable.Range(0, itemDrop.Count.Value).Select(_ => GetItemWithClass(itemDrop.ItemId)));
            }
            else if (itemDrop.Range != null)
            {
                resultBag.AddRange(
                    Enumerable.Range(0, RandomUtils.Range(itemDrop.Range.Min, itemDrop.Range.Max))
                        .Select(_ => GetItemWithClass(itemDrop.ItemId))
                );
            }
            else if (itemDrop.Dice != null)
            {
                resultBag.AddRange(
                    Enumerable.Range(0, Dice.DiceParser.Parse(itemDrop.Dice).Roll())
                        .Select(_ => GetItemWithClass(itemDrop.ItemId))
                );
            }
        }

        var probabilityBag = RandomUtils.RandomWeightedElements(1, itemDrops.Where(s => s.Probability != null).ToArray());

        resultBag.AddRange(probabilityBag.Select(s => GetItemWithClass(s.ItemId)));

        return resultBag;
    }

    public IEnumerable<ItemEntity> GetItemsFromItemGroupId(string itemGroupId)
    {
        if (_itemGroups.TryGetValue(itemGroupId.ToLower(), out var itemGroup))
        {
            return itemGroup.Ids.Select(GetItemWithClass).ToList();
        }

        throw new Exception("Item group not found.");
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
                foreach (var itemId in groupEntity.Ids)
                {
                    if (_items.ContainsKey(itemId.ToLower()))
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
