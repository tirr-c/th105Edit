using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace th105Edit
{
    public partial class cv1RecordEditor : Form
    {
        private cv1DataLine m_record;
        public cv1DataLine Record
        {
            get { return m_record; }
        }
        private int m_field_index;
        private int FieldIndex
        {
            get { return m_field_index; }
            set
            {
                m_record.Fields[m_field_index] = txtData.Text.Replace("\n", "").Replace("\r", "");

                m_field_index = value;
                if (m_record.Fields.Length <= m_field_index) m_field_index = m_record.Fields.Length - 1;
                if (m_field_index < 0) m_field_index = 0;

                btnNext.Enabled = btnPrevious.Enabled = true;
                if (m_record.Fields.Length - 1 == m_field_index) btnNext.Enabled = false;
                if (m_field_index == 0) btnPrevious.Enabled = false;

                txtData.Text = m_record.Fields[m_field_index];
            }
        }

        public cv1RecordEditor(cv1DataLine Record)
        {
            InitializeComponent();
            m_record = Record;
            m_field_index = 0;
            txtData.Text = m_record.Fields[m_field_index];
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            FieldIndex--;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            FieldIndex++;
        }

        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
            }
        }

        private void cv1RecordEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_record.Fields[FieldIndex] = txtData.Text;
        }
    }
}
