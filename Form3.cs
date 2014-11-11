using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StudentsTest
{
    public partial class Form3 : Form
    {
        public Form3(int _city_id)
        {
            InitializeComponent();
            m_LoadCity(_city_id);
        }

        void m_LoadCity(int city_id)
        {
            using(DataClasses1DataContext db=new DataClasses1DataContext())
            {
                var tbl = from if_CitySelect in db.if_CitySelect()
                          where if_CitySelect.City_id!=null
                          orderby if_CitySelect.City_Name
                          select if_CitySelect;
                comboBox1.DataSource = tbl;
                comboBox1.ValueMember = "City_id";
                comboBox1.DisplayMember = "City_Name";
                comboBox1.SelectedValue = city_id;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text) ||
                Convert.ToInt32(comboBox1.SelectedValue) <= 0)
            {
                MessageBox.Show("შეავსეთ აუცილებელი ველები!");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
