using Newtonsoft.Json;
using QuestBuilder.mysql;
using QuestBuilder.quests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var questType in Enum.GetValues(typeof(QuestTypes)))
            {
                questTypesBox.Items.Add(questType);
            }
            foreach (var icon in Enum.GetValues(typeof(QuestIcons)))
            {
                questIconBox.Items.Add(icon);
            }
            foreach (var conditionType in Enum.GetValues(typeof(QuestConditions)))
            {
                conditionTypesBox.Items.Add(conditionType);
            }

            if (Program.Quest == null)
            {
                HideAll();
                return;
            }
        }
        
        private void UpdateQuestTree()
        {
            questTree.Nodes.Clear();
            var q = Program.Quest;
            questTree.Nodes.Add("ID: " + q.Root.Id);
            questTree.Nodes.Add("Active: " + q.Root.Active);
            questTree.Nodes.Add("Ordered: " + q.Root.Ordered);
            questTree.Nodes.Add("Mandatory: " + q.Root.Mandatory);
            questTree.Nodes.Add("MandatoryCount: " + q.Root.MandatoryCount);
            var elements = questTree.Nodes.Add("Elements");
            var i = 0;
            foreach (var qElement in q.Root.Elements)
            {
                var e = elements.Nodes.Add("[" + i + "]");
                LoadQuestElement(qElement, e);
                i++;
            }
        }

        private void LoadQuestElement(QuestElement qElement, TreeNode element)
        {
            var elementCase = element.Nodes.Add("Case");
            elementCase.Nodes.Add("ID: " + qElement.Case.Id);
            elementCase.Nodes.Add("Active: " + qElement.Case.Active);
            elementCase.Nodes.Add("Ordered: " + qElement.Case.Ordered);
            elementCase.Nodes.Add("Mandatory: " + qElement.Case.Mandatory);
            elementCase.Nodes.Add("MandatoryCount: " + qElement.Case.MandatoryCount);
            var subElement = elementCase.Nodes.Add("Elements");
            var i = 0;
            foreach (var _element in qElement.Case.Elements)
            {
                var e = subElement.Nodes.Add("[" + i + "]");
                LoadQuestElement(_element, e);
                i++;
            }
            var elementCondition = element.Nodes.Add("Condition");
            LoadQuestCondition(qElement.Condition, elementCondition);
        }

        private void LoadQuestCondition(QuestCondition questCond, TreeNode e)
        {
            e.Nodes.Add("Id: " + questCond.Id);
            e.Nodes.Add("Type: " + questCond.Type);
            e.Nodes.Add("Matches: " + string.Join(", ", questCond.Matches));
            e.Nodes.Add("Mandatory: " + questCond.Mandatory);
            e.Nodes.Add("TargetValue: " + questCond.TargetValue);
            var state = e.Nodes.Add("State");
            state.Nodes.Add("Active: " + questCond.State.Active);
            state.Nodes.Add("Completed: " + questCond.State.Completed);
            state.Nodes.Add("CurrentValue: " + questCond.State.CurrentValue);
            var sub = e.Nodes.Add("SubConditions");
            var i = 0;
            foreach (var subCond in questCond.SubConditions)
            {
                var c = sub.Nodes.Add("[" + i + "]");
                LoadQuestCondition(subCond, c);
                i++;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void questsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new QuestList().Show();
        }

        public void Reload()
        {
            var quest = Program.Quest;
            if (quest.Root == null)
            {
                Program.Quest.Root = new QuestRoot() { Id = quest.Id, Elements = new List<QuestElement>() };
            }
            if (quest.Rewards == null)
            {
                Program.Quest.Rewards = new quests.serializables.QuestSerializableReward();
            }
            var q = Program.Quest;
            idLabel.Text = q.Id.ToString();
            questIconBox.SelectedIndex = (int)q.Icon;
            expRewardBox.Text = q.Rewards.Exp.ToString();
            honRewardBox.Text = q.Rewards.Honor.ToString();
            creRewardBox.Text = q.Rewards.Credits.ToString();
            uriRewardBox.Text = q.Rewards.Uridium.ToString();
            UpdateQuestTree();
            ShowAll();
            Refresh();
        }

        private void HideAll()
        {
            foreach (Control ctrl in Controls)
            {
                if (ctrl == menuStrip1) continue;
                ctrl.Hide();
            }
        }
        private void ShowAll()
        {
            foreach (Control ctrl in Controls)
            {
                ctrl.Show();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var selectedNode = questTree.SelectedNode;
            if (selectedNode != null && selectedNode.Parent == null)
            {
                Program.Quest.Root.Elements.Add(new QuestElement() { Case = new QuestRoot() { Elements = new List<QuestElement>() },
                Condition = new QuestCondition() { SubConditions = new List<QuestCondition>(), Matches = new List<int>(), State = new QuestState() }
                });
            }
            UpdateQuestTree();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void saveBttn_Click(object sender, EventArgs e)
        {
            var quest = Program.Quest;
            // todo: form condition ids
            foreach (var element in quest.Root.Elements)
            {
                LoadConditions(element);
            }

            var i = 0;
            foreach (var condition in _conds)
            {
                condition.Id = 100 + (i * 100) + quest.Id * 1000;
                i++;
            }
            UpdateQuestTree();
            _conds.Clear();

            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                mySqlClient.ExecuteNonQuery($"UPDATE server_quests SET ROOT='{JsonConvert.SerializeObject(Program.Quest.Root)}', REWARDS='{JsonConvert.SerializeObject(Program.Quest.Rewards)}', ICON='{(int)Program.Quest.Icon}', TYPE='{(int)Program.Quest.QuestType}' WHERE ID='{Program.Quest.Id}'");
            }
        }

        private List<QuestCondition> _conds = new List<QuestCondition>();
        private void LoadConditions(QuestElement element)
        {
            foreach (var subElement in element.Case.Elements)
            {
                LoadConditions(subElement);
            }
            _conds.Add(element.Condition);
            foreach (var sub in element.Condition.SubConditions)
            {
                _conds.Add(sub);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = questTree.SelectedNode;
            if (selected != null && selected.Level == 1)
            {
                var dogshit = Convert.ToInt32(selected.Text.Replace("[", "").Replace("]", ""));
                LeashTheDog(dogshit);
            }
        }

        private void LeashTheDog(int dogId)
        {
            var rex = Program.Quest.Root.Elements[dogId];
            selectedElement = rex;
            activeCase.Checked = rex.Case.Active;
            mandCase.Checked = rex.Case.Mandatory;
            orderCase.Checked = rex.Case.Ordered;
            manCount.Text = rex.Case.MandatoryCount.ToString();

            // cond
            conditionTypesBox.SelectedIndex = (int)rex.Condition.Type;
            matchesBox.Text = string.Join(",", rex.Condition.Matches);
            targetVal.Text = rex.Condition.TargetValue.ToString();
            mandCondition.Checked = rex.Condition.Mandatory;
        }

        QuestElement selectedElement;
        private void matchesBox_TextChanged(object sender, EventArgs e)
        {
            if (selectedElement == null) return;
            try
            {
                List<int> matches = new List<int>();
                var split = matchesBox.Text.Split(',');
                foreach (var splitted in split)
                {
                    var num = 0;
                    if (int.TryParse(splitted, out num))
                    {
                        matches.Add(num);
                    }
                    else
                    {
                        MessageBox.Show("Match failed to parse! USE ONLY NUMBERS [0-9] AND ',' SEPERATOR\nExample: 2,8,10");
                    }
                }
                selectedElement.Condition.Matches = matches;
                UpdateQuestTree();
            }
            catch (Exception)
            {
                MessageBox.Show("error");
            }
        }

        private void conditionTypesBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (selectedElement == null) return;

            selectedElement.Condition.Type = (QuestConditions) conditionTypesBox.SelectedIndex;
            UpdateQuestTree();
        }

        private void targetVal_TextChanged(object sender, EventArgs e)
        {
            if (selectedElement == null) return;

            var targetValue = 0;
            if (int.TryParse(targetVal.Text, out targetValue))
            {
                selectedElement.Condition.TargetValue = targetValue;
                UpdateQuestTree();
            }
        }

        private void mandCondition_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedElement == null) return;
            selectedElement.Condition.Mandatory = mandCondition.Checked;
        }

        private void activeCase_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedElement == null) return;
            selectedElement.Case.Active = activeCase.Checked;
        }

        private void mandCase_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedElement == null) return;
            selectedElement.Case.Mandatory = mandCase.Checked;
        }

        private void orderCase_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedElement == null) return;
            selectedElement.Case.Ordered = orderCase.Checked;
        }

        private void manCount_TextChanged(object sender, EventArgs e)
        {
            if (selectedElement == null) return;
            try
            {
                selectedElement.Case.MandatoryCount = int.Parse(manCount.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("NUMBERS ONLY!");
            }
        }

        private void questTypesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.Quest == null) return;
            Program.Quest.QuestType = (QuestTypes)questTypesBox.SelectedIndex;
        }

        private void expRewardBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Program.Quest.Rewards.Exp = int.Parse(expRewardBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("NUMBERS ONLY!");
            }
        }

        private void honRewardBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Program.Quest.Rewards.Honor = int.Parse(honRewardBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("NUMBERS ONLY!");
            }
        }

        private void creRewardBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Program.Quest.Rewards.Credits = int.Parse(creRewardBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("NUMBERS ONLY!");
            }
        }

        private void uriRewardBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Program.Quest.Rewards.Uridium = int.Parse(uriRewardBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("NUMBERS ONLY!");
            }
        }

        private void generalHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CheatSheet().Show();
        }
    }
}
