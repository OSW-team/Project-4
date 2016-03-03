using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public UnitManager UnitManager;
        public List<CheckBox> CheckBoxList;
        private bool _redrawCheckbox;
        public Form1()
        {
            InitializeComponent();
            panel1.Hide();
            panel2.Hide();
            CheckBoxList = new List<CheckBox>();
        }



        public void SetUnitsCombobox(List<string> items)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(items.ToArray());
        }

        public void SetUnitFields(List<string> fields, List<string> values)
        {
            _redrawCheckbox = true;
            tableLayoutPanel1.Hide();
            tableLayoutPanel1.Controls.Clear();
            if (fields.Count > 0) tableLayoutPanel1.RowCount = fields.Count;
            foreach (var field in fields)
            {
                var index = fields.IndexOf(field);
                var label = new Label();
                label.AutoSize = true;
                label.Text = field;
                var texbox = new TextBox();
                texbox.Width = 280;
                texbox.Text = values[index];
                texbox.Name = field;
                var buttonUp = new Button();
                buttonUp.Width = 25;
                buttonUp.Text = @"/\";
                buttonUp.Name = field;
                buttonUp.Click += new EventHandler(UnitButtonUp_Click);
                var buttonDown = new Button();
                buttonDown.Width = 25;
                buttonDown.Text = @"\/";
                texbox.TextChanged += new EventHandler(UnitTexbox_TextChanged);
                tableLayoutPanel1.Controls.Add(label,0,index);
                tableLayoutPanel1.Controls.Add(texbox, 1, index);
                tableLayoutPanel1.Controls.Add(buttonUp, 2, index);
                tableLayoutPanel1.Controls.Add(buttonDown, 3, index);
            }
            tableLayoutPanel1.Show();
        }

        public string GetUnitModelPath()
        {            
            var o = @"\";
            var n =  @"/";
            var text = textBox3.Text.Replace(o, n);
            return text;
        }

        public void SetUpgradeFields(List<string> fields, List<string> values)
        {
            tableLayoutPanel3.Hide();
            tableLayoutPanel3.Controls.Clear();
            if (fields.Count > 0) tableLayoutPanel3.RowCount = fields.Count;
            foreach (var field in fields)
            {
                var index = fields.IndexOf(field);
                var label = new Label();
                label.AutoSize = true;
                label.Text = field;
                var texbox = new TextBox();
                texbox.Width = 280;
                texbox.Text = values[index];
                texbox.Name = field;
                var buttonUp = new Button();
                buttonUp.Width = 25;
                buttonUp.Text = @"/\";
                var buttonDown = new Button();
                buttonDown.Width = 25;
                buttonDown.Text = @"\/";
                texbox.TextChanged += new EventHandler(UpgradeTexbox_TextChanged);
                tableLayoutPanel3.Controls.Add(label, 0, index);
                tableLayoutPanel3.Controls.Add(texbox, 1, index);
                tableLayoutPanel3.Controls.Add(buttonUp, 2, index);
                tableLayoutPanel3.Controls.Add(buttonDown, 3, index);
            }
            tableLayoutPanel3.Show();
        } 

        private void button1_Click(object sender, EventArgs e)
        {
            UncheckUpgrades();
            SetUnitFields(UnitManager.GetUnitFieldsList(), UnitManager.GetUnitUpgradedFieldsValues());
            UnitManager.Save();
        }

        private void UncheckUpgrades()
        {
            _redrawCheckbox = false;
            foreach (var ch in CheckBoxList)
            {
                ch.Checked = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UncheckUpgrades();
            UnitManager.LoadUnitFields(comboBox1.SelectedItem.ToString());
            UnitManager.FillUnitTable();
            ShowUpgradesList();
            groupBox1.Text = comboBox1.SelectedItem.ToString();
            tableLayoutPanel3.Controls.Clear();
        }

        public void ShowUpgradesList()
        {
            var list = UnitManager.GetUpgradesList();
            tableLayoutPanel2.Hide();
            tableLayoutPanel2.Controls.Clear();
            if (list.Count > 0) tableLayoutPanel1.RowCount = list.Count;
            foreach (var field in list)
            {
                var index = list.IndexOf(field);
                var linkLabel = new LinkLabel();
                linkLabel.AutoSize = true;
                var link = new LinkLabel.Link();
                link.Name = field;
                linkLabel.Links.Add(link);
                linkLabel.Text = field;
                linkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(ShowUpgrade); 
                var checkbox = new CheckBox();
                checkbox.Name = field;
                checkbox.CheckStateChanged += Checkbox_CheckStateChanged;
                CheckBoxList.Add(checkbox);
                tableLayoutPanel2.Controls.Add(linkLabel, 0, index);
                tableLayoutPanel2.Controls.Add(checkbox, 1, index);
            }
            tableLayoutPanel2.Show();
        }

        private void Checkbox_CheckStateChanged(object sender, EventArgs e)
        {
            var o = (CheckBox)sender;
            UnitManager.AddUpgradeValues(o.Name, o.Checked, _redrawCheckbox);
        }

        private void ShowUpgrade(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var b = false;
            foreach (var ch in CheckBoxList)
            {
                if(ch.Checked)
                {
                    b = true;    
                }
            }
            if (b)
            {
                UncheckUpgrades();
                SetUnitFields(UnitManager.GetUnitFieldsList(), UnitManager.GetUnitUpgradedFieldsValues());
            }
            groupBox3.Text = e.Link.Name;
            UnitManager.LoadUpgradeFields(e.Link.Name);
            UnitManager.FillUpgradeTable();
        }

        private void UnitTexbox_TextChanged(object sender, EventArgs e)
        {
            var o = (TextBox)sender;
            UnitManager.ReplaceUnitValue(o.Name, o.Text);
        }

        private void UpgradeTexbox_TextChanged(object sender, EventArgs e)
        {
            var o = (TextBox)sender;
            UnitManager.ReplaceUpgradeValue(o.Name, o.Text);
        }

        private void UnitButtonUp_Click(object sender, EventArgs e)
        {
            var o = (Button)sender;
            UnitManager.MoveUpUnitField(o.Name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                UnitManager.AddUnit(textBox1.Text);
                panel1.Hide();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel2.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox2.Text!="")
            {
                UnitManager.AddUpgrade(textBox2.Text);
                panel2.Hide();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel2.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UncheckUpgrades();
            UnitManager.Start();
        }
    }
}
