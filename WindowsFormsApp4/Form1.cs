using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        string DBFile = null;
        string CSVFile = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                DBFile = ofd.FileName;
                label1.Enabled = true;
                comboBox1.Enabled = true;

                List<string> TableValues = new List<string>();
                
                foreach (DataRow Item in GetTablesFromDB(DBFile).Rows)
                {
                    if (Item[0].ToString() != "sqlite_sequence")
                    {
                        TableValues.Add(Item[0].ToString());
                    }
                }

                comboBox1.DataSource = TableValues;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                DefaultExt = "*.csv",
                Filter = "Comma Separated Values (*.csv) | *.csv"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CSVFile = ofd.FileName;
                checkBox1.Enabled = true;
                label2.Enabled = true;
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;

                ImportDataFromCSVFile(ofd.FileName);
            }
        }

        private DataTable GetTablesFromDB(string FilePath)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection("Data Source=" + FilePath))
            {
                try
                {
                    dbConnection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string SQL = "SELECT name FROM sqlite_master WHERE type='table'";

                using (SQLiteCommand cmd = new SQLiteCommand(SQL, dbConnection))
                {
                    using (SQLiteDataAdapter da_FillTableSelection = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt_FillTableSelection = new DataTable();

                        try
                        {
                            da_FillTableSelection.Fill(dt_FillTableSelection);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        return dt_FillTableSelection;
                    }
                }
            }
        }

        private DataTable GetColumnNames(string TableName, string FilePath)
        {
            using (SQLiteConnection dbConnection = new SQLiteConnection("Data Source=" + FilePath))
            {
                try
                {
                    dbConnection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string SQL = "PRAGMA table_info (`" + TableName + "`)";

                using (SQLiteCommand cmd = new SQLiteCommand(SQL, dbConnection))
                {
                    using (SQLiteDataAdapter da_FillColumnSelection = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt_FillColumnSelection = new DataTable();
                        try
                        {
                            da_FillColumnSelection.Fill(dt_FillColumnSelection);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        return dt_FillColumnSelection;
                    }
                }
            }
        }

        private void ImportDataFromCSVFile(string FilePath)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> ColumnValues = new List<string>();

            foreach (DataRow Item in GetColumnNames(comboBox1.SelectedItem.ToString(), DBFile).Rows)
            {
                if (Item[1].ToString() != "id")
                {
                    ColumnValues.Add(Item[1].ToString());
                }
            }

            HeaderSelection.DataSource = ColumnValues;
            // Funktioniert mit test-ComboBox
        }
    }
}
