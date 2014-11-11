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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loadCity(0);
            //dataGridView1.Columns["colCity_Name"].Width = splitContainer1.SplitterDistance - 50;
        }

        void loadCity(int? _cid)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var tbl = from if_CitySelect in db.if_CitySelect()
                          orderby if_CitySelect.City_Name
                          select if_CitySelect;

                dataGridView1.DataSource = tbl;

                if (_cid != null)
                {
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        if (Convert.ToInt32(dataGridView1["colCity_id", i].Value) == _cid)
                        {
                            dataGridView1.Rows[i].Selected = true;
                            dataGridView1.CurrentCell = dataGridView1["colCity_Name", i];
                        }
                        else
                            dataGridView1.Rows[i].Selected = false;
                    }
                }
            }
        }
        private void დამატებაToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_AddCity();
            m_LoadStudents(null);
        }

        private void m_AddCity()
        {
            using (Form2 frm = new Form2() { Text = "ქალაქის დამატება" })
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int? _city_id = null;
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {

                            db.tsp_iudCity(0, ref _city_id, frm.textBox1.Text);
                        }
                        loadCity(_city_id);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void m_EditCity()
        {
            int? _city_id = Convert.ToInt32(dataGridView1["colCity_id",dataGridView1.CurrentRow.Index].Value);
            if(_city_id<=0) return;

            using (Form2 frm = new Form2() { Text = "ქალაქის რედაქტირება" })
            {
                frm.button1.Text = "რედაქტირება";
                frm.textBox1.Text = dataGridView1["colCity_Name", dataGridView1.CurrentRow.Index].Value.ToString();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {

                            db.tsp_iudCity(1, ref _city_id, frm.textBox1.Text);
                        }
                        loadCity(_city_id);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            m_LoadStudents(null);
            m_CityMenuEnable();
            m_StudentMenuEnable();
        }

        private void m_LoadStudents(int? _cid)
        {
            int _city_id = 0;
            try
            {
                _city_id = Convert.ToInt32(dataGridView1["colCity_id", dataGridView1.CurrentCell.RowIndex].Value);
            }
            catch { }
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var tbl = from if_StudentsSelect in db.if_StudentsSelect(_city_id)
                          orderby if_StudentsSelect.FullName
                          select if_StudentsSelect;

                dataGridView2.DataSource = tbl;

                if (_cid != null)
                {
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        if (Convert.ToInt32(dataGridView2["colStudent_id", i].Value) == _cid)
                        {
                            dataGridView2.Rows[i].Selected = true;
                            dataGridView2.CurrentCell = dataGridView1["colFullName", i];
                        }
                        else
                            dataGridView2.Rows[i].Selected = false;
                    }
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow Myrow in dataGridView1.Rows)
            {           
                if (Convert.ToInt32(Myrow.Cells["colCity_id"].Value) ==0)
                {
                    Myrow.DefaultCellStyle.BackColor = Color.Gold;
                }
                else
                {
                    Myrow.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void რედაქტირებაToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_EditCity();
            m_LoadStudents(null);
        }

        private void წაშლაToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_DelCity();
        }
        void m_DelCity()
        {
            int? _city_id = Convert.ToInt32(dataGridView1["colCity_id", dataGridView1.CurrentRow.Index].Value);
            if (_city_id <= 0) return;

            if (MessageBox.Show(this, "დარწმუნებული ხართ, რომ გსურთ მიმდინარე ქალაქის წაშლა?", "ყურადღება!", 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        db.tsp_iudCity(2,ref  _city_id, null);
                    }
                    loadCity(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void m_CityMenuEnable()
        {
            int? _city_id =0;
            try
            {
                _city_id = Convert.ToInt32(dataGridView1["colCity_id", dataGridView1.CurrentRow.Index].Value);
            }
            catch { }
            if (_city_id > 0)
                რედაქტირებაToolStripMenuItem.Enabled = წაშლაToolStripMenuItem.Enabled =
                    რედაქტირებაToolStripMenuItem2.Enabled = წაშლაToolStripMenuItem2.Enabled = true;
            else
                რედაქტირებაToolStripMenuItem.Enabled = წაშლაToolStripMenuItem.Enabled =
                    რედაქტირებაToolStripMenuItem2.Enabled = წაშლაToolStripMenuItem2.Enabled = false;
        }

        private void დამატებაToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            m_AddStudent();
        }

        private void m_AddStudent()
        {
            int _city_id = Convert.ToInt32(dataGridView1["colCity_id", dataGridView1.CurrentRow.Index].Value);
            using (Form3 frm = new Form3(_city_id) { Text = "სტუდენტის დამატება" })
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    int? _student_id = null;
                    try
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            db.tsp_iudStudents(0, ref _student_id, Convert.ToInt32(frm.comboBox1.SelectedValue),
                                frm.textBox1.Text, frm.textBox2.Text, frm.textBox3.Text, frm.textBox4.Text);
                            m_LoadStudents(_student_id);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void რედაქტირებაToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            m_EditStudent();
        }

        private void m_EditStudent()
        {
            int? _student_id = null;
            if (dataGridView2.RowCount > 0)
                _student_id = Convert.ToInt32(dataGridView2["colStudent_id", dataGridView2.CurrentRow.Index].Value);

            if (_student_id == null) return;

            int _city_id = Convert.ToInt32(dataGridView2["colCity_id1", dataGridView2.CurrentRow.Index].Value);
            using (Form3 frm = new Form3(_city_id) { Text = "სტუდენტის რედაქტირება" })
            {
                frm.button1.Text = "რედაქტირება";
                frm.textBox1.Text=string.Format("{0}",dataGridView2["colFirstName", dataGridView2.CurrentRow.Index].Value);
                frm.textBox2.Text=string.Format("{0}",dataGridView2["colLastName", dataGridView2.CurrentRow.Index].Value);
                frm.textBox3.Text=string.Format("{0}",dataGridView2["colPersonalID", dataGridView2.CurrentRow.Index].Value);
                frm.textBox4.Text = string.Format("{0}", dataGridView2["colPhoneNumber", dataGridView2.CurrentRow.Index].Value);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (DataClasses1DataContext db = new DataClasses1DataContext())
                        {
                            db.tsp_iudStudents(1, ref _student_id, Convert.ToInt32(frm.comboBox1.SelectedValue),
                                frm.textBox1.Text, frm.textBox2.Text, frm.textBox3.Text, frm.textBox4.Text);
                            m_LoadStudents(_student_id);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void დამატებაToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            m_AddCity();
        }

        private void რედაქტირებაToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            m_EditCity();
        }

        private void წაშლაToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            m_DelCity();
        }

        private void წაშლაToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            m_DelStudent();
        }
        void m_DelStudent()
        {
            int? _student_id = Convert.ToInt32(dataGridView2["colStudent_id", dataGridView2.CurrentRow.Index].Value);
            if (_student_id <= 0) return;

            if (MessageBox.Show(this, "დარწმუნებული ხართ, რომ გსურთ მიმდინარე სტუდენტის წაშლა?", "ყურადღება!",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                try
                {
                    using (DataClasses1DataContext db = new DataClasses1DataContext())
                    {
                        db.tsp_iudStudents(2, ref  _student_id, null, null, null, null, null);
                    }
                    m_LoadStudents(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void m_StudentMenuEnable()
        {
            int? _student_id = null;
            if (dataGridView2.RowCount > 0)
                _student_id = Convert.ToInt32(dataGridView2["colStudent_id", dataGridView2.CurrentRow.Index].Value);

            if (_student_id > 0)
                რედაქტირებაToolStripMenuItem1.Enabled = წაშლაToolStripMenuItem1.Enabled =
                    რედაქტირებაToolStripMenuItem3.Enabled = წაშლაToolStripMenuItem3.Enabled = true;
            else
                რედაქტირებაToolStripMenuItem1.Enabled = წაშლაToolStripMenuItem1.Enabled =
                    რედაქტირებაToolStripMenuItem3.Enabled = წაშლაToolStripMenuItem3.Enabled = false;
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            m_StudentMenuEnable();
        }

        private void დამატებაToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            m_AddStudent();
        }

        private void რედაქტირებაToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            m_EditStudent();
        }

        private void წაშლაToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            m_DelStudent();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            m_EditCity();
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            m_EditStudent();
        }
    }
}
