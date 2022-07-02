using DataAccess.extensions;
using Microsoft.Extensions.Configuration;
using MyStoreWinApp.services;
using System.Text.RegularExpressions;

namespace MyStoreWinApp
{
    public partial class frmLogin : Form
    {

        IMemberService service = new MemberService();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (service.IsLogined(email, password) || IsDefaultAccount(email, password))
            {
                ShowInvalidMessage("Login successfully");
            }
            else
            {
                ShowInvalidMessage("Incorrect email or password!!");
            }

        }

        private bool IsFormattedEmail(string email)
        {
            return Regex.IsMatch(email, Validation.EMAIL_REGEX);
        }

        private void ShowInvalidMessage(string message)
        {
            txtValid.ForeColor = Color.Red;
            txtValid.Text = message;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (txtEmail.Text.Length == 0)
            {
                txtValid.Text = "";
            }
            else if (!IsFormattedEmail(txtEmail.Text))
            {
                ShowInvalidMessage("Invalid email format!!");
            }
        }

        private bool IsDefaultAccount(string email, string password)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            return email.Equals(config["DefaultEmail"]) && password.Equals(config["DefaultPassword"]);
        }
    }
}
