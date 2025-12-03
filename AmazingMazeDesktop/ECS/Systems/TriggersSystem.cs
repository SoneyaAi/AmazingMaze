using System;
using System.Diagnostics;
using System.Linq;
using AmazingMazeDesktop.ECS.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace AmazingMazeDesktop.Systems;

public class TriggersSystem(GameContext context) : EntityUpdateSystem(Aspect.All(typeof(TriggerComponent)))
{
    ComponentMapper<TriggerComponent> _triggerMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _triggerMapper = mapperService.GetMapper<TriggerComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var trigger = _triggerMapper.Get(entity);
            if (trigger.IsArmed && trigger.Type == TriggerType.OnEnter)
            {
                trigger.IsArmed = false;
                trigger.IsTriggered = true;
            }

            if (!trigger.IsTriggered)
                continue;
            Debug.WriteLine(trigger.Action);
            if (trigger.Action == TriggerAction.LoadNextLevel)
            {
                GetEntity(trigger.ActivatingEntity).Get<Transform2>().Position =
                    context.Services.CoordinatesTranslator.CellToWorld(
                        context.CurrentLevel.SpawnPoints.First(x => x.IsPlayerSpawnFromHigher).TileCoordinates);
                context.ChangeLevelBy(1);
            }

            if (trigger.Action == TriggerAction.LoadPreviousLevel)
            {
                if (context._currentLevelIndex == 0)
                    continue;
                GetEntity(trigger.ActivatingEntity).Get<Transform2>().Position =
                    context.Services.CoordinatesTranslator.CellToWorld(
                        context.CurrentLevel.SpawnPoints.First(x => x.IsPlayerSpawnFromLower).TileCoordinates);
                context.ChangeLevelBy(-1);
            }

            trigger.IsTriggered = false;
        }
    }
}