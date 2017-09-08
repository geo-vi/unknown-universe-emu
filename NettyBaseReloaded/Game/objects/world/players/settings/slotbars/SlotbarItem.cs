using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.players.settings.slotbars
{
    abstract class SlotbarItem
    {
        public string ItemId { get; }
        public string ClickedId { get; set; }
        public double CounterValue { get; set; }
        public int MaxCounter { get; set; }
        public List<ClientUITooltipTextFormat> ToolTipItemBar;
        public short CounterType { get; set; } //2 default == BAR
        public short CounterStyle { get; set; }
        public bool Activable { get; set; }
        public bool Selected { get; set; }
        public bool Visible { get; set; }
        public bool Buyable { get; set; }
        public bool Available { get; set; }
        public bool Blocked { get; set; }

        public SlotbarCategoryItemModule Object { get; private set; }

        protected SlotbarItem(string itemId, int counterValue, int maxCounter, List<ClientUITooltipTextFormat> toolTipItemBar = null, short counterType = 2, bool selected = false,
            bool visible = true, bool buyable = false, bool available = true, bool blocked = false)
        {
            ItemId = itemId;
            ClickedId = itemId;
            CounterValue = counterValue;

            CounterStyle = SlotbarItemStatus.BLUE;

            MaxCounter = maxCounter;

            if (toolTipItemBar == null)
            {
                ToolTipItemBar = new List<ClientUITooltipTextFormat>
                {
                    new ClientUITooltipTextFormat(ClientUITooltipTextFormat.STANDARD, itemId, new commandWw(3),
                        new List<commandF5>()),
                    new ClientUITooltipTextFormat(ClientUITooltipTextFormat.STANDARD, itemId, new commandWw(7),
                        new List<commandF5>())
                };
            }
            else
            {
                ToolTipItemBar = toolTipItemBar;
            }

            CounterType = counterType;
            Activable = true;
            Selected = selected;
            Visible = visible;
            Buyable = buyable;
            Available = available;
            Blocked = blocked;
        }

        /// <summary>
        /// Creates the slotbar object button (Called in constructor)
        /// </summary>
        public void Create()
        {
            var itemStatus = new SlotbarItemStatus(
                ItemId,
                Selected, //selected
                Activable, //activable
                Buyable, //buyable
                ItemId, //clickId
                MaxCounter, //maxCounter
                Blocked, //blocked
                new ClientUITooltip(new List<ClientUITooltipTextFormat>()),
                Visible, //visible
                CounterStyle, //counterStyle
                Available, //available
                new ClientUITooltip(ToolTipItemBar),
                CounterValue //counterValue 
            );

            var timerModule = new SlotbarCategoryItemTimerModule(ItemId, new TimerState(TimerState.READY), 0, 0, true);

            Object = new SlotbarCategoryItemModule(1, itemStatus, timerModule, new CooldownTypeModule(CooldownTypeModule.NONE),
                CounterType, SlotbarCategoryItemModule.SELECTION);
        }

        public byte[] ChangeStatus()
        {
            var itemStatus = new SlotbarItemStatus(
                ItemId,
                Selected, //selected
                Activable, //activable
                Buyable, //buyable
                ItemId, //clickId
                MaxCounter, //maxCounter
                Blocked, //blocked
                new ClientUITooltip(new List<ClientUITooltipTextFormat>()),
                Visible, //visible
                CounterStyle, //counterStyle
                Available, //available
                new ClientUITooltip(ToolTipItemBar),
                CounterValue //counterValue
            );

            return itemStatus.write2();
        }

        /// <summary>
        /// This method will execute the action if the button gets clicked
        /// </summary>
        public abstract void Execute(Player player);
    }
}

