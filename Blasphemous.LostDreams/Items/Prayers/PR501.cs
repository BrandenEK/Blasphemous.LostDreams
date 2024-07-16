using CreativeSpore.SmartColliders;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using System.Linq;
using UnityEngine;

namespace Blasphemous.LostDreams.Items.Prayers;

internal class PR501 : EffectOnPrayerUse
{
    // Do somthing here
    protected override float EffectTime { get; } = 0;

    protected override bool UsePrayerDurationModifier { get; } = false;

    private readonly PR501Config _config;

    public PR501(PR501Config cfg)
    {
        _config = cfg;
    }

    protected override void OnActivate()
    {
        PerformSwap();
    }

    private void PerformSwap()
    {
        Enemy enemy = FindClosestEnemy();

        if (enemy == null)
        {
            Main.LostDreams.LogWarning("PR501: No enemy was in range");
            return;
        }

        Main.LostDreams.Log($"PR501: Swapping places with {enemy.name}");
        Vector3 playerPosition = Core.Logic.Penitent.transform.position;
        Vector3 enemyPosition = enemy.transform.position;

        MoveEntity(enemy, Vector3.zero);
        MoveEntity(Core.Logic.Penitent, enemyPosition);
        MoveEntity(enemy, playerPosition);
    }

    private void MoveEntity(Entity entity, Vector3 position)
    {
        SmartPlatformCollider collider = entity.GetComponentInChildren<SmartPlatformCollider>();

        if (collider != null)
            collider.enabled = false;

        entity.transform.position = position;

        if (collider != null)
            collider.enabled = true;
    }

    private Enemy FindClosestEnemy()
    {
        Vector3 playerPosition = Core.Logic.Penitent.transform.position;

        return Object.FindObjectsOfType<Enemy>()
            .Select(e => new EnemyDistance(e, Vector3.Distance(playerPosition, e.transform.position)))
            .Where(x => x.Distance <= _config.MAX_RANGE && !_bannedEnemyIds.Contains(x.Enemy.Id))
            .OrderBy(x => x.Distance)
            .FirstOrDefault()?.Enemy;
    }

    class EnemyDistance(Enemy e, float d)
    {
        public Enemy Enemy { get; } = e;
        public float Distance { get; } = d;
    }

    private readonly string[] _bannedEnemyIds =
    {
        "BS16"
    };
}

/// <summary> Properties for PR501 </summary>
public class PR501Config
{
    /// <summary> Fervour cost to use this prayer </summary>
    public int FERVOUR_COST = 25;
    /// <summary> Maximum range for enemies that you can swap with </summary>
    public float MAX_RANGE = 10;
}
