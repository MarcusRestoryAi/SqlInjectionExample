using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlInjectionExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoggaIn();
        }

        private void LoggaIn()
        {

            //Definiera min Connection String
            string server = "localhost";
            string database = "sqlinkectexample";

            string dbUsername = "injectuser";
            string dbPassword = "ABC123";

            string MySqlConnectionString = $"SERVER={server};DATABASE={database};UID={dbUsername};PASSWORD={dbPassword};";

            string username = txtUsername.Text;
            string password = txtPassword.Text;

            //string sqlQuerry = $"SELECT * from users WHERE users_username = '{username}' and users_password = '{password}';";
            string sqlQuerry = "SELECT * from users WHERE users_username = @username and users_password = @password;";

            //Skapa MySQL connection
            MySqlConnection conn = new MySqlConnection(MySqlConnectionString);

            //Skapa en MySQL command
            MySqlCommand cmd = new MySqlCommand(sqlQuerry, conn);

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            try
            {
                //Öppna connection
                conn.Open();

                //Exekvera command
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read() )
                {
                    lblOutput.Text = "Du har loggat in!";
                } else
                {
                    lblOutput.Text = "Du är inte inloggad!";
                }
            } catch (Exception e)
            {
                lblOutput.Text = e.Message;
            }
            finally
            {
                conn.Close();
            }

            //Kryptera lösenordet, bara för att
            SHA512 sha = SHA512.Create();
            Byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hex = BitConverter.ToString(hash).Replace("-", "");

            lblCrypt.Text = hex;
            
        }
    }
}
