using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;



namespace SVU_Bestellungen
{
    public partial class TrikotOrderForm : Form
    {
        private DataTable ordersTable;
        private string connectionString;
        private SQLiteDataAdapter dataAdapter;
        private DataTable dataTable;

        public TrikotOrderForm()
        {
            InitializeComponent();
            InitializeOrdersTable();
            InitializeControls();
            InitializeDatabase();
            //LoadOrdersFromDatabase();
            btnSaveSummary.Click += (s, e) => EvaluateOrderQuantities();
            this.AcceptButton = btnAddOrder;
            // this.BackgroundImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("SVU_Bestellungen.background.jpg"));
            dataGridViewOrders.BorderStyle = BorderStyle.None;
            dataGridViewOrders.VirtualMode = true;
            dataGridViewOrders.ScrollBars = ScrollBars.Vertical;
            foreach (DataGridViewColumn column in dataGridViewOrders.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            this.BackColor = System.Drawing.Color.White;
            this.ForeColor = System.Drawing.Color.DarkRed;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Load += TrikotOrderForm_Load;  // Registering the Load event
        }

        private void TrikotOrderForm_Load(object sender, EventArgs e)
        {
            txtNachname.Focus();
            connectionString = $"Data Source=Bestellungen.db;Version=3;";
            string currentUserName = Environment.UserName;
            LogMessage($"Willkommen, {currentUserName}!");
            numericUpDownQuantity.Value = 1;

            // Find the index of the "ID" column
            int idColumnIndex = dataGridViewOrders.Columns["ID"].Index;

            // Move the "ID" column to the beginning
            dataGridViewOrders.Columns["ID"].DisplayIndex = 0;

            // Adjust the display index of the other columns
            foreach (DataGridViewColumn column in dataGridViewOrders.Columns)
            {
                if (column.Name != "ID")
                {
                    if (column.DisplayIndex <= idColumnIndex)
                    {
                        column.DisplayIndex += 1;
                    }
                }
            }



        }

        private void LogMessage(string message)
        {
            // Erstellen des Zeitstempels
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Füge den Zeitstempel und die Meldung zur richInfoTextBox hinzu
            richInfoTextBox.AppendText($"[{timestamp}] {message}\n");

            // Automatisches Scrollen zum Ende der RichTextBox, damit die neueste Nachricht sichtbar ist
            richInfoTextBox.ScrollToCaret();
        }

        private void ShowErrorMessage(string error)
        {
            // Titel und Symbol der MessageBox anpassen
            MessageBox.Show(error, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void InitializeDatabase()
        {
            string databasePath = "Bestellungen.db";
            string connectionString = $"Data Source={databasePath};Version=3;";

            // Ensure database structures are in place
            SetupDatabase(connectionString);

            // Load data and bind to DataGridView
            BindDataToDataGridView(connectionString);
        }

        private void SetupDatabase(string connectionString)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString))
            {
                m_dbConnection.Open();

                // Create table if it doesn't exist
                string createTableSql = @"CREATE TABLE IF NOT EXISTS orders (
                        ID INTEGER PRIMARY KEY,
                        Nachname TEXT,
                        Vorname TEXT,
                        Initialen TEXT,
                        Größe TEXT,
                        Menge INTEGER)";
                using (SQLiteCommand command = new SQLiteCommand(createTableSql, m_dbConnection))
                {
                    command.ExecuteNonQuery();
                }

                // Create unique index
                string createIndexSql = "CREATE UNIQUE INDEX IF NOT EXISTS idx_unique_order ON orders (Nachname, Vorname, Größe, Initialen)";
                using (SQLiteCommand command = new SQLiteCommand(createIndexSql, m_dbConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void SyncDataBase(object sender, EventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Änderungen zurück in die Datenbank schreiben
                new SQLiteCommandBuilder(dataAdapter);
                dataAdapter.Update(dataTable);
            }

        }

        private void BindDataToDataGridView(string connectionString)
        {
            SQLiteDataAdapter dataAdapter;
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Load data from database into DataTable
                dataAdapter = new SQLiteDataAdapter("SELECT * FROM orders", connection);
                dataAdapter.Fill(dataTable);

                // Bind DataTable to DataGridView
                dataGridViewOrders.DataSource = dataTable;
            }
        }





        private void LoadOrdersFromDatabase()
        {
            string databasePath = "Bestellungen.db";
            using (SQLiteConnection m_dbConnection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
            {
                m_dbConnection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM orders", m_dbConnection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataRow row = ordersTable.NewRow();
                        row["Nachname"] = reader["Nachname"];
                        row["Vorname"] = reader["Vorname"];
                        row["Größe"] = reader["Größe"];
                        row["Initialen"] = reader["Initialen"];
                        row["Menge"] = int.Parse(reader["Menge"].ToString());
                        ordersTable.Rows.Add(row);
                    }
                }
            }
        }



        private void InitializeOrdersTable()
        {
            ordersTable = new DataTable();
            ordersTable.Columns.Add("Nachname");
            ordersTable.Columns.Add("Vorname");
            ordersTable.Columns.Add("Größe");  // Adding the Größe column
            ordersTable.Columns.Add("Initialen");
            ordersTable.Columns.Add("Menge", typeof(int));

            dataGridViewOrders.DataSource = ordersTable;

            // Konfigurieren Sie die Darstellung der Status-Spalte im DataGridView
            if (dataGridViewOrders.Columns["Status"] is DataGridViewCheckBoxColumn statusCol)
            {
                statusCol.HeaderText = "Gespeichert";
            }
        }


        private void InitializeControls()
        {
            comboBoxSize.Items.Add("S");
            comboBoxSize.Items.Add("M");
            comboBoxSize.Items.Add("L");
            comboBoxSize.Items.Add("XL");

            // Styling
            foreach (Control ctrl in this.Controls)
            {
                // Default Control Styling
                ctrl.ForeColor = System.Drawing.Color.DarkRed;
                ctrl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);

                // Specific Control Styling
                if (ctrl is Button)
                {
                    var button = ctrl as Button;
                    button.BackColor = System.Drawing.Color.DarkRed;
                    button.ForeColor = System.Drawing.Color.White;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = System.Drawing.Color.DarkRed;
                    button.FlatAppearance.BorderSize = 1;
                }
                else if (ctrl is ComboBox)
                {
                    var comboBox = ctrl as ComboBox;
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                else if (ctrl is DataGridView)
                {
                    var dgv = ctrl as DataGridView;
                    dgv.BackgroundColor = System.Drawing.Color.White;
                    dgv.BorderStyle = BorderStyle.None;
                    dgv.GridColor = System.Drawing.Color.DarkRed;
                    dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightCoral;
                    dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    dgv.RowHeadersVisible = false;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.DarkRed;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                    dgv.EnableHeadersVisualStyles = false;
                }
                else if (ctrl is TextBox)
                {
                    var txt = ctrl as TextBox;
                }
            }

            // Button Hover Effects
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button)
                {
                    var button = ctrl as Button;
                    button.MouseEnter += (s, e) =>
                    {
                        button.BackColor = System.Drawing.Color.White;
                        button.ForeColor = System.Drawing.Color.DarkRed;
                        button.FlatAppearance.BorderColor = System.Drawing.Color.White;
                    };
                    button.MouseLeave += (s, e) =>
                    {
                        button.BackColor = System.Drawing.Color.DarkRed;
                        button.ForeColor = System.Drawing.Color.White;
                    };
                }
            }
        }

        private void BtnAddOrder_Click(object sender, EventArgs e)
        {
            DataRow row = ordersTable.NewRow();
            row["Nachname"] = txtNachname.Text;
            row["Vorname"] = txtVorname.Text;
            row["Initialen"] = txtInitialen.Text;
            row["Größe"] = comboBoxSize.SelectedItem.ToString();
            row["Menge"] = numericUpDownQuantity.Value;
            ordersTable.Rows.Add(row);

            // Clear the input controls
            txtNachname.Clear();
            txtVorname.Clear();
            txtInitialen.Clear();
            comboBoxSize.SelectedIndex = -1; // deselect any selected item
            numericUpDownQuantity.Value = numericUpDownQuantity.Minimum; // or set it to some default value if you have
        }

        private void BtnSaveOrders_Click(object sender, EventArgs e)
        {
            try
            {
                string databasePath = "Bestellungen.db";
                using (SQLiteConnection m_dbConnection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    m_dbConnection.Open();

                    // Beginnen Sie die Transaktion
                    using (SQLiteTransaction transaction = m_dbConnection.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataRow row in ordersTable.Rows)
                            {

                                string insertSQL = $"INSERT INTO orders (Nachname, Vorname, Größe, Initialen, Menge) VALUES (@Nachname, @Vorname, @Größe, @Initialen, @Menge)";
                                using (SQLiteCommand cmd = new SQLiteCommand(insertSQL, m_dbConnection))
                                {
                                    cmd.Parameters.AddWithValue("@Nachname", row["Nachname"].ToString());
                                    cmd.Parameters.AddWithValue("@Vorname", row["Vorname"].ToString());
                                    cmd.Parameters.AddWithValue("@Größe", row["Größe"].ToString());
                                    cmd.Parameters.AddWithValue("@Initialen", row["Initialen"].ToString());
                                    cmd.Parameters.AddWithValue("@Menge", row["Menge"].ToString());
                                    cmd.ExecuteNonQuery();

                                    // Status der Zeile in ordersTable auf gespeichert setzen
                                    row["Status"] = true;
                                }
                            }

                            // Transaktion abschließen
                            transaction.Commit();
                        }
                        catch
                        {
                            // Bei einem Fehler wird die Transaktion zurückgesetzt
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                // Überprüfen Sie, ob es sich um einen Konflikt wegen eines eindeutigen Indexes handelt
                if (ex.ResultCode == SQLiteErrorCode.Constraint)
                {
                    LogMessage("Ein oder mehrere Einträge sind bereits in der Datenbank vorhanden und wurden nicht erneut hinzugefügt.");
                }
                else
                {
                    ShowErrorMessage($"Ein Fehler ist aufgetreten: {ex.Message}");
                }
            }

            LogMessage("Bestellungen gespeichert!");
        }


        private void EvaluateOrderQuantities()
        {
            // Key: Größe, Value: Gesamtmenge
            Dictionary<string, int> summary = new Dictionary<string, int>();

            foreach (DataRow row in ordersTable.Rows)
            {
                string size = row["Größe"].ToString();
                int quantity = int.Parse(row["Menge"].ToString());

                if (summary.ContainsKey(size))
                {
                    summary[size] += quantity;
                }
                else
                {
                    summary[size] = quantity;
                }
            }

            // Speichern der zusammengefassten Daten in einer CSV-Datei
            using (StreamWriter sw = new StreamWriter("bestellzusammenfassung.csv", false, Encoding.UTF8))
            {
                // Header
                sw.WriteLine("Größe,Menge");

                foreach (var entry in summary)
                {
                    sw.WriteLine($"{entry.Key},{entry.Value}");
                }
            }

            // Anzeige der zusammengefassten Daten
            StringBuilder orderSummary = new StringBuilder("Bestellzusammenfassung:\n\n");
            foreach (var entry in summary)
            {
                orderSummary.AppendLine($"Größe {entry.Key}: {entry.Value} Stück");
            }

            LogMessage(orderSummary.ToString());
            LogMessage("Bestellzusammenfassung erfolgreich im CSV-Format gespeichert!");
        }


        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the directory of the current executable
                string directoryPath = Path.GetDirectoryName(Application.ExecutablePath);

                // Check if directoryPath is not null, otherwise provide a fallback value
                string safeDirectoryPath = directoryPath ?? string.Empty;

                // Open the directory in the default file explorer
                if (!string.IsNullOrEmpty(safeDirectoryPath))
                {
                    Process.Start("explorer.exe", safeDirectoryPath);
                }
                else
                {
                    LogMessage("Konnte den Ordner nicht finden.");
                }
            }
            catch (Exception ex)
            {
                // Display a user-friendly message
                ShowErrorMessage($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }

        private void DataGridViewOrders_Paint(object sender, PaintEventArgs e)
        {
            Rectangle borderRectangle = dataGridViewOrders.Bounds;
            borderRectangle.Inflate(-1, -1);  // Optionally shrink the rectangle slightly if you want the border inside the DGV's bounds
            ControlPaint.DrawBorder(e.Graphics, borderRectangle, Color.Red, ButtonBorderStyle.Solid);
        }
    }
}
