using MyStoreWinApp.services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyStoreWinApp
{
    public partial class frmMemberManagement : Form
    {
        IMemberService _service = new MemberService();
        BindingSource source;

        private TextBox[] boxes;
        private bool isUpdated = true;
     

        public frmMemberManagement()
        {
            InitializeComponent();
            boxes = new TextBox[] { txtMemberID, txtMemberName, txtEmail, txtPassword, txtCity, txtCountry };
        }

        private void frmMemberManagement_Load(object sender, EventArgs e)
        {
            new frmLogin().ShowDialog();
            LoadMemberList();
        }

        private void LoadMemberList()
        {
            var members = _service.FetchList();
            try
            {
                source = new BindingSource();
                source.DataSource = members;

                ClearBindingsForTextBoxes(boxes);
                AddBindingsForTextBoxes(source, "Text", boxes);
                dgvMemberList.DataSource = source;

                if (members.Count() == 0)
                {
                    ClearText(boxes);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ClearText(params TextBox[] boxes)
        {
            foreach (var box in boxes)
            {
                box.Text = string.Empty;
            }
        }

        private void ClearBindingsForTextBoxes(params TextBox[] boxes)
        {
            foreach (var box in boxes)
            {
                box.DataBindings.Clear();
            }
        }

        private void AddBindingsForTextBoxes(BindingSource source, string sourceType, params TextBox[] boxes)
        {
            foreach (var box in boxes)
            {
                box.DataBindings.Add(sourceType, source, GetControlNameWithoutPrefix(box, "txt"));
            }
        }

        private string GetControlNameWithoutPrefix(Control control, string toSplit)
        {
            return control.Name.Split(toSplit)[1];
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearText(boxes);
            ClearBindingsForTextBoxes(boxes);
            isUpdated = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isUpdated)
                {
                    _service.ModifyMember(new()
                    {
                        MemberID = txtMemberID.Text,
                        MemberName = txtMemberName.Text,
                        Email = txtEmail.Text,
                        Password = txtPassword.Text,
                        City = txtCity.Text,
                        Country = txtCountry.Text
                    }, ModifidationType.UPDATE);
                } 
                else
                {
                    _service.ModifyMember(new()
                    {
                        MemberID = Guid.NewGuid().ToString(),
                        MemberName = txtMemberName.Text,
                        Email = txtEmail.Text,
                        Password = txtPassword.Text,
                        City = txtCity.Text,
                        Country = txtCountry.Text
                    }, ModifidationType.INSERT);
                }
                
                LoadMemberList();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _service.DeleteMember(txtMemberID.Text);
                LoadMemberList();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvMemberList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            source.Position = source.Count;
        }
    }
}
