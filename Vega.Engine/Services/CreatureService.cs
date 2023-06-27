using Microsoft.Extensions.Logging;
using SadRogue.Primitives;
using Vega.Engine.Components.Creatures;
using Vega.Engine.Interfaces;
using Vega.Engine.Services.Base;
using Vega.Framework.Attributes;
using Vega.Framework.Data.Entities.Creatures;
using Vega.Framework.Data.Entities.Names;
using Vega.Framework.Map.GameObjects.Creatures;
using Vega.Framework.Utils.Random;

namespace Vega.Engine.Services;

[VegaService(5)]
public class CreatureService : BaseDataLoaderVegaService<CreatureService>, ICreatureService
{
    private readonly Dictionary<string, CreatureClassEntity> _creatureClassEntities = new();
    private readonly Dictionary<string, CreatureEntity> _creatureEntities = new();
    private readonly Dictionary<string, CreatureGroupEntity> _creatureGroups = new();

    private readonly ITileService _tileService;
    private readonly IItemService _itemService;
    private readonly INameService _nameService;

    public CreatureService(
        ILogger<CreatureService> logger, IDataService dataService, IMessageBusService messageBusService,
        IItemService itemService, ITileService tileService, INameService nameService
    ) :
        base(logger, dataService, messageBusService)
    {
        _itemService = itemService;
        _tileService = tileService;
        _nameService = nameService;
    }

    public override Task<bool> LoadAsync()
    {
        LoadCreatureClassesAsync();
        LoadCreatureGroupsAsync();
        LoadCreaturesAsync();


        return Task.FromResult(true);
    }

    public CreatureGameObject CreateCreatureGameObject(string creatureId, Point position)
    {
        if (_creatureEntities.TryGetValue(creatureId.ToLower(), out var _))
        {
            // TODO: Add creature equipment and behavior
            var creatureEntity = GetCreature(creatureId);

            var creatureGlyph = _tileService.GetTile(creatureEntity);
            var creatureGameObject = new CreatureGameObject(
                creatureEntity.Id,
                creatureEntity.Sym,
                creatureEntity.Name,
                creatureEntity.Description,
                creatureGlyph.coloredGlyph,
                position,
                false,
                false
            )
            {
                Name = _nameService.RandomName(NameTypeEnum.Given, creatureEntity.GenderType)
            };
            if (creatureEntity.ItemGroupId != null)
            {
                var itemGroup = _itemService.GetItemsFromItemGroupId(creatureEntity.ItemGroupId);
                creatureGameObject.GoRogueComponents.Add(new InventoryComponent(itemGroup.ToList()));
            }

            creatureGameObject.Stats = creatureEntity.Stats.BuildRandomCreatureStats();


            return creatureGameObject;
        }

        throw new Exception($"Creature not found with id {creatureId}");
    }


    public IEnumerable<CreatureEntity> GetCreaturesFromGroup(string creatureGroupId)
    {
        if (_creatureGroups.TryGetValue(creatureGroupId.ToLower(), out var creatureGroupEntity))
        {
            var creatureIds = creatureGroupEntity.Creatures.BuildPropEntries();
            return creatureIds.Select(GetCreature);
        }

        throw new Exception($"Creature group not found with id {creatureGroupId}");
    }

    private Task LoadCreatureClassesAsync()
    {
        foreach (var creatureClass in LoadData<CreatureClassEntity>())
        {
            _creatureClassEntities.Add(creatureClass.Id.ToLower(), creatureClass);
        }

        return Task.CompletedTask;
    }

    private Task LoadCreatureGroupsAsync()
    {
        foreach (var creatureGroup in LoadData<CreatureGroupEntity>())
        {
            _creatureGroups.Add(creatureGroup.Id.ToLower(), creatureGroup);
        }

        return Task.CompletedTask;
    }

    private Task LoadCreaturesAsync()
    {
        foreach (var creature in LoadData<CreatureEntity>())
        {
            _creatureEntities.Add(creature.Id.ToLower(), creature);
        }

        return Task.CompletedTask;
    }

    public CreatureEntity GetCreature(string id)
    {
        if (_creatureEntities.TryGetValue(id.ToLower(), out var creatureEntity))
        {
            var creature = new CreatureEntity()
            {
                Id = creatureEntity.Id,
                Sym = creatureEntity.Sym,
                Background = creatureEntity.Background,
                Foreground = creatureEntity.Foreground,
                IsWalkable = creatureEntity.IsWalkable,
                IsTransparent = creatureEntity.IsTransparent,
                GenderType = creatureEntity.GenderType,
                Stats = creatureEntity.Stats,
                ItemGroupId = creatureEntity.ItemGroupId,
                BehaviorTreeId = creatureEntity.BehaviorTreeId,
                Name = creatureEntity.Name,
                Description = creatureEntity.Description,
                Flags = creatureEntity.Flags,
                Comment = creatureEntity.Comment,
            };

            if (creatureEntity.CreatureClassId != null)
            {
                var creatureClass = _creatureClassEntities[creatureEntity.CreatureClassId.ToLower()];
                creature.Sym = creatureClass.Sym ?? creatureEntity.Sym;
                creature.Background = creatureClass.Background ?? creatureEntity.Background;
                creature.Foreground = creatureClass.Foreground ?? creatureEntity.Foreground;
                creature.IsWalkable = creatureClass.IsWalkable;
                creature.IsTransparent = creatureClass.IsTransparent;
                creature.GenderType = creatureClass.GenderTypeEnum;
                creature.Stats = creatureClass.Stats;
                creature.BehaviorTreeId = creatureClass.BehaviorTreeId;
                creature.Name = creatureClass.Name ?? creatureEntity.Name;
                creature.Flags = creatureClass.Flags;
                creature.Comment = creatureClass.Comment;
            }

            return creature;
        }

        throw new Exception($"Creature not found with id {id}");
    }
}
