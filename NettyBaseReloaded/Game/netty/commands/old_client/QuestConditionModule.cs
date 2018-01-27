using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class QuestConditionModule
    {
        public const short QUESTCASE = 0;

        public const short TIMER = 1;

        public const short HASTE = 2;

        public const short ENDURANCE = 3;

        public const short COUNTDOWN = 4;

        public const short COLLECT = 5;

        public const short KILL_NPC = 6;

        public const short DAMAGE = 7;

        public const short AVOID_DAMAGE = 8;

        public const short TAKE_DAMAGE = 9;

        public const short AVOID_DEATH = 10;

        public const short COORDINATES = 11;

        public const short DISTANCE = 12;

        public const short TRAVEL = 13;

        public const short FUEL_SHORTAGE = 14;

        public const short PROXIMITY = 15;

        public const short MAP = 16;

        public const short MAP_DIVERSE = 17;

        public const short EMPTY = 18;

        public const short MISCELLANEOUS = 19;

        public const short AMMUNITION = 20;

        public const short SAVE_AMMUNITION = 21;

        public const short SPEND_AMMUNITION = 22;

        public const short SALVAGE = 23;

        public const short STEAL = 24;

        public const short KILL = 25;

        public const short DEAL_DAMAGE = 26;

        public const short KILL_NPCS = 27;

        public const short KILL_PLAYERS = 28;

        public const short DAMAGE_NPCS = 29;

        public const short DAMAGE_PLAYERS = 30;

        public const short VISIT_MAP = 31;

        public const short DIE = 32;

        public const short AVOID_KILL_NPC = 33;

        public const short AVOID_KILL_NPCS = 34;

        public const short AVOID_KILL_PLAYERS = 35;

        public const short AVOID_DAMAGE_NPCS = 36;

        public const short AVOID_DAMAGE_PLAYERS = 37;

        public const short PREVENT = 38;

        public const short JUMP = 39;

        public const short AVOID_JUMP = 40;

        public const short STEADINESS = 41;

        public const short MULTIPLIER = 42;

        public const short STAY_AWAY = 43;

        public const short IN_GROUP = 44;

        public const short KILL_ANY = 45;

        public const short WEB = 46;

        public const short CLIENT = 47;

        public const short CARGO = 48;

        public const short SELL_ORE = 49;

        public const short LEVEL = 50;

        public const short USER_DEFINED = 51;

        public const short COLLECT_BONUS_BOX = 52;

        public const short UPGRADE = 53;

        public const short FINISH_QUEST = 54;

        public const short QUICK_BUY = 55;

        public const short ENTER_GROUP = 56;

        public const short RESTRICT_AMMUNITION_KILL_NPC = 57;

        public const short RESTRICT_AMMUNITION_KILL_PLAYER = 58;

        public const short KILL_PLAYER = 59;

        public const short VISIT_QUEST_GIVER = 60;

        public const short REAL_TIME_HASTE = 61;

        public const short SPECIAL_HONOUR = 62;

        public const short ID = 750;

        public int id;
        public List<int> matches;
        public short type;
        public short displayType;
        public double targetValue;
        public bool mandatory;
        public QuestConditionStateModule state;
        public List<QuestConditionModule> subConditions;

        public QuestConditionModule(int id, List<int> matches, short type, short displayType, double targetValue, bool mandatory, QuestConditionStateModule state, List<QuestConditionModule> subConditions)
        {
            this.id = id;
            this.matches = matches;
            this.type = type;
            this.displayType = displayType;
            this.targetValue = targetValue;
            this.mandatory = mandatory;
            this.state = state;
            this.subConditions = subConditions;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(id);
            cmd.Integer(matches.Count);
            foreach (var loc0 in matches)
            {
                cmd.Integer(loc0);
            }
            cmd.Short(type);
            cmd.Short(displayType);
            cmd.Double(targetValue);
            cmd.Boolean(mandatory);
            cmd.AddBytes(state.write());
            cmd.Integer(subConditions.Count);
            foreach (var loc1 in subConditions)
            {
                cmd.AddBytes(loc1.write());
            }
            return cmd.Message.ToArray();
        }
    }
}
