using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;



using Gameplay.GameControllers.Penitent.Attack;
using Framework.FrameworkCore.Attributes;
using Framework.FrameworkCore.Attributes.Logic;
using Framework.Managers;
using Framework.Inventory;
using Tools.Playmaker2;

using Blasphemous.Framework.Items;
using Blasphemous.ModdingAPI;
using System.Runtime.CompilerServices;


namespace Blasphemous.LostDreams.Effects;

/// <summary>
/// allows attacks to shoot out a beam when TPO is at full health, with a cooldown.
/// </summary>
public class HolyWater : ModItemEffectOnEquip
{
    private readonly RB504Config _config;

    /// <summary>
    /// checks if TPO has more than 99% of health
    /// </summary>
    private bool _canShootBeam
    {
        get 
        {
            return (Core.Logic.Penitent.Stats.Life.Current > Core.Logic.Penitent.Stats.Life.MaxValue * 0.99f);
        }
    }

    private bool _hasBeam = false;

    private float _beamDamage;

    private float _currentTimeLapse;

    private CloisteredGemBeadEffect _beamEffect;


    /// <summary>
    /// Initializes parameters by reading from `config`
    /// </summary>
    public HolyWater(RB504Config config)
    {
        _config = config;
    }

    /// <summary>
    /// set beam damage (to scale with current attack).
    /// </summary>
    public void SetBeamDamage()
    {
        _beamDamage = _config.BEAM_BASE_DAMAGE + Core.Logic.Penitent.Stats.Strength.Final * _config.BEAM_STRENGTH_SCALING;
    }

    /// <summary>
    /// allow the next attack to fire a beam
    /// </summary>
    public void AddBeamToAttack()
    {
        _hasBeam = true;
        SetBeamDamage();

        // add beam to attack
    }

    /// <summary>
    /// Checks if TPO remains at full health for 3 seconds.
    /// When timer reaches 3 seconds, apply beam effect.
    /// </summary>
    protected override void Update()
    {
        base.Update();
        if (_canShootBeam && !_hasBeam && Main.LostDreams.ItemHandler.IsEquipped("RB504"))
        {
            _currentTimeLapse += Time.deltaTime;
            if (_currentTimeLapse >= _config.BEAM_COOLDOWN)
            {
                AddBeamToAttack();
            }
        }
        else
        {
            _currentTimeLapse = 0f;
        }

    }

    /// <summary>
    /// apply the cooldown timer and register a beam to the attack when timer ticks
    /// </summary>
    protected override void ApplyEffect()
    {
        
    }

    /// <summary>
    /// removes timer and removes existing beam effect
    /// </summary>
    protected override void RemoveEffect()
    {
        Main.LostDreams.TimeHandler.RemoveTimer("RB504-beam");
    }

    
}



public class RB504Config
{
    /// <summary> 
    /// The cooldown before the next attack can send a beam, in seconds.
    /// </summary>
    public float BEAM_COOLDOWN = 3f;

    /// <summary>
    /// the base damage of the beam (not scaling to anything)
    /// </summary>
    public float BEAM_BASE_DAMAGE = 0f;

    /// <summary>
    /// the beam damage's scaling factor based on TPO's current Mea Culpa `Strength` attribute
    /// </summary>
    public float BEAM_STRENGTH_SCALING = 1f;

    // final beam damage = BEAM_BASE_DAMAGE + Strength * BEAM_STRENGTH_SCALING

}