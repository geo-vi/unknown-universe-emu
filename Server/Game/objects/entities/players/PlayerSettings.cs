using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Server.Game.managers;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.players
{
    class PlayerSettings : PlayerImplementedClass
    {
        private readonly ConcurrentBag<AbstractSettings> _settings = new ConcurrentBag<AbstractSettings>();

        public PlayerSettings(Player player) : base(player)
        {
        }

        public void CreateSettings(AbstractSettings[] settings = null)
        {
            if (settings == null)
            {
                _settings.Add(new AudioSettings { Unset = false, Music = false, Sound = false});
                _settings.Add(new GameplaySettings
                {
                    Unset = true, AutoBoost = false, AutoRefinement = false,
                    AutoStart = false, DoubleclickAttack = true, AutoChangeAmmo = false,
                    AutoBuyGreenBootyKeys = false, QuickslotStopAttack = true, DisplayConfigurationChanges = true
                });
                _settings.Add(new DisplaySettings
                {
                    Unset = true, DisplayCargoboxes = true, DisplayChat = true, DisplayPlayerName = true,
                    DisplayHitpointBubbles = true, AlwaysDraggableWindows = true, ShipHovering = true,
                    ShowSecondQuickslotBar = true, UseAutoQuality = true
                });
                _settings.Add(new KeySettings { Unset = true });
                _settings.Add(new QualitySettings
                {
                    Unset = true, QualityBackground = 3, QualityAttack = 3, QualityCollectables = 3,
                    QualityCustomized = false, QualityEffect = 3, QualityEngine = 3, QualityExplosion = 3,
                    QualityPOIzone = 3, QualityPresetting = 3, QualityShip = 3
                });
                _settings.Add(new WindowSettings
                {
                    Unset = true, BarStatus = "23,0,24,0,25,1,26,0,27,0", MainmenuPosition = "313,480", ClientResolutionId = 0,
                    MinimapScale = 11, 
                    ResizableWindowsString = "5,240,150,20,300,150,36,260,175,", 
                    SlotMenuOrder = "0", 
                    SlotmenuPosition = "313,451", 
                    SlotmenuPremiumPosition = "313,500", 
                    SlotMenuPremiumOrder = "0", 
                    WindowSettingsString = 
                        "0,444,-1,0,1,1057,329,1,20,39,530,0,3,1021,528,1,5,-10,-6,0,24,463,15,0,10,101,307,0,36,100,400,0,13,315,122,0,23,1067,132,0"
                });
                _settings.Add(new SlotbarSettings
                {
                    Unset = true, 
                    QuickbarSlots = "",
                    QuickbarSlotsPremium = "",
                    SelectedHellstormRocketAmmo = "ammunition_rocketlauncher_eco-10", SelectedLaserAmmo = "ammunition_laser_lcb-10", SelectedRocketAmmo = "ammunition_rocket_r-310"
                });
            }
            else
            { 
                foreach (var setting in settings)
                {
                    _settings.Add(setting);
                }

                if (_settings.Count < 7)
                {
                    if (!settings.Any(x => x is AudioSettings))
                    {
                        _settings.Add(new AudioSettings());
                    }

                    if (!settings.Any(x => x is GameplaySettings))
                    {
                        _settings.Add(new GameplaySettings());
                    }

                    if (!settings.Any(x => x is DisplaySettings))
                    {
                        _settings.Add(new DisplaySettings());
                    }

                    if (!settings.Any(x => x is KeySettings))
                    {
                        _settings.Add(new KeySettings());
                    }

                    if (!settings.Any(x => x is QualitySettings))
                    {
                        _settings.Add(new QualitySettings());
                    }

                    if (!settings.Any(x => x is WindowSettings))
                    {
                        _settings.Add(new WindowSettings());
                    }
                    
                    if (!settings.Any(x => x is SlotbarSettings))
                    {
                        _settings.Add(new SlotbarSettings());
                    }
                }
            }
        }

        public void WipeSettings()
        {
            _settings.Clear();
            CreateSettings();
        }

        public AbstractSettings[] PackSettings()
        {
            return _settings.ToArray();
        }
        
        public T GetSettings<T>() where T : AbstractSettings
        {
            var setting = _settings.FirstOrDefault(x => x is T);
            return setting as T;
        }

        public void SaveSettings()
        {
            GameDatabaseManager.Instance.SavePlayerSettings(Player);
        }
    }
}