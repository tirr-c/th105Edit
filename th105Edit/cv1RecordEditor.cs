/*
Copyright VBChunguk  2012-2013

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
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
