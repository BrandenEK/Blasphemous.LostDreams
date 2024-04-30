using Blasphemous.Framework.Items;
using Framework.Managers;
using Gameplay.GameControllers.Entities;
using System.Linq;
using UnityEngine;

namespace Blasphemous.LostDreams.Prayers;

public class PR501(float _range) : ModItemEffectOnPrayerUse
{
    protected override float EffectTime { get; } = 0;

    protected override bool UsePrayerDurationModifier { get; } = false;

    protected override void ApplyEffect()
    {
        PerformSwap();
    }

    protected override void RemoveEffect() { }

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
        Core.Logic.Penitent.transform.position = enemy.transform.position;
        enemy.transform.position = playerPosition;
    }

    private Enemy FindClosestEnemy()
    {
        Vector3 playerPosition = Core.Logic.Penitent.transform.position;

        return Object.FindObjectsOfType<Enemy>()
            .Select(e => new EnemyDistance(e, Vector3.Distance(playerPosition, e.transform.position)))
            .Where(x => x.Distance <= _range)
            .OrderBy(x => x.Distance)
            .FirstOrDefault()?.Enemy;
    }

    class EnemyDistance(Enemy e, float d)
    {
        public Enemy Enemy { get; } = e;
        public float Distance { get; } = d;
    }
}
