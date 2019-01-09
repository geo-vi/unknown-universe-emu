using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using QuestBuilder.mysql;
using QuestBuilder.quests;
using QuestBuilder.quests.serializables;
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
    public partial class QuestList : Form
    {
        public QuestList()
        {
            InitializeComponent();
            Name = "Loading...";
        }

        private void QuestList_Load(object sender, EventArgs e)
        {
            questView.DataSource = GetQuests();
            Name = "Connected, " + questView.Rows.Count + " quests loaded";
            questView.UserAddedRow += RowAdded;
            questView.CellEndEdit += EdittedCell;
            questView.SelectionChanged += QuestSelected;
            questView.Columns[0].ReadOnly = true;
        }

        private void QuestSelected(object sender, EventArgs e)
        {
            var selection = questView.SelectedCells;
            if (selection.Count > 0)
            {
                foreach (DataGridViewCell selectedCell in selection)
                {
                    var cell = questView.Rows[selectedCell.RowIndex].Cells["ID"].Value;
                    if (cell == DBNull.Value) continue;
                    Program.Quest = LoadQuest(selectedCell.RowIndex);
                    Program.MainForm.Reload();
                }
            }
        }

        private void EdittedCell(object sender, DataGridViewCellEventArgs e)
        {
            var row = questView.Rows[e.RowIndex];
            var selectedCell = row.Cells[e.ColumnIndex];
            var cell = row.Cells["ID"];
            if (cell.Value == DBNull.Value) return;
            var id = Convert.ToInt32(cell.Value);

            switch (e.ColumnIndex)
            {
                case 1:
                    using (var mySqlClient = SqlDatabaseManager.GetClient())
                    {
                        mySqlClient.ExecuteNonQuery($"UPDATE server_quests SET NAME='{selectedCell.Value}' WHERE ID={id}");
                    }
                    break;
                case 2:
                    using (var mySqlClient = SqlDatabaseManager.GetClient())
                    {
                        mySqlClient.ExecuteNonQuery($"UPDATE server_quests SET DESC='{selectedCell.Value}' WHERE ID={id}");
                    }
                    break;
                default:
                    MessageBox.Show("Editting here is not possible!");
                    break;
            }
        }
        
        private void RowAdded(object sender, DataGridViewRowEventArgs e)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                mySqlClient.ExecuteNonQuery("INSERT INTO server_quests(TYPE, ICON, EXPIRY_DATE, DAY_OF_WEEK) VALUES (0, 0, NOW(), 0)");
            }

            questView.DataSource = GetQuests();
            questView.Update();
        }

        private DataTable GetQuests()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                return mySqlClient.ExecuteQueryTable("SELECT * FROM server_quests");
            }
        }

        private QuestLoader LoadQuest(int rowIndex)
        {
            var row = questView.Rows[rowIndex];
            var id = Convert.ToInt32(row.Cells["ID"].Value);
            var rootJson = row.Cells["ROOT"].Value.ToString();
            var rewardsJson = row.Cells["REWARDS"].Value.ToString();
            var type = Convert.ToInt32(row.Cells["TYPE"].Value.ToString());
            var icon = Convert.ToInt32(row.Cells["ICON"].Value.ToString());

            return new QuestLoader() { Id = id, Root = JsonConvert.DeserializeObject<QuestRoot>(rootJson), Rewards = JsonConvert.DeserializeObject<QuestSerializableReward>(rewardsJson), Icon = (QuestIcons)icon, QuestType = (QuestTypes)type };
        }
    }
}
