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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace th105Edit
{
    public partial class frmEntryName : Form
    {
        private string m_entry;
        private bool m_decided;
        public string Entry
        {
            get { return m_entry; }
            set
            {
                lblEntry.Text = Path.GetFileName(value);
                txtEntry.Text = value.Replace('\\', '/').Substring((value.IndexOf("data") == -1) ? 0 : value.IndexOf("data"));
            }
        }

        public frmEntryName()
        {
            m_entry = string.Empty;
            m_decided = false;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_entry = txtEntry.Text;
            m_decided = true;
            Close();
        }

        private void frmEntryName_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_decided) m_entry = string.Empty;
        }
    }
}
