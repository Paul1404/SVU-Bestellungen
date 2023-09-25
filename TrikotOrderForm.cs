using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace SVU_Bestellungen
{
    public partial class TrikotOrderForm : Form
    {
        private DataTable ordersTable;
        private const string ConnectionString = "data source=bestellungen.db";
        const string SIZE_COLUMN = "Größe";
        const string QUANTITY_COLUMN = "Menge";

        public TrikotOrderForm()
        {
            InitializeComponent();
            InitializeDatabase();
            InitializeOrdersTable();
            InitializeControls();
            LoadOrdersFromSQLite();
            //LoadOrdersFromCSV();
            btnSaveSummary.Click += (s, e) => EvaluateOrderQuantities();
            this.FormClosing += OrderManagementForm_FormClosing;
            this.AcceptButton = btnAddOrder;
            this.BackgroundImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("SVU_Bestellungen.RedAbstractShapesSmall.jpg"));
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


        /// <summary>
        /// Handles the "Load" event of the TrikotOrderForm. Sets initial control states and focus.
        /// </summary>
        /// <param name="sender">The source of the event. Typically, the instance of TrikotOrderForm that triggered the event.</param>
        /// <param name="e">Event data. Contains no specific data for this event.</param>
        /// <remarks>
        /// This method performs the following initializations:
        ///
        /// 1. Sets the focus to the 'txtNachname' textbox, allowing users to start inputting data immediately upon form load.
        ///
        /// 2. Initializes the 'numericUpDownQuantity' control value to 1.
        ///
        /// It ensures a smooth user experience by priming controls for immediate use once the form loads.
        /// </remarks>
        private void TrikotOrderForm_Load(object sender, EventArgs e)
        {
            txtNachname.Focus();
            numericUpDownQuantity.Value = 1;
        }


        /// <summary>
        /// Logs an informational message to the 'richTextBoxLogs' with a timestamp and an 'INFO' indication.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <remarks>
        /// This method performs the following operations:
        ///
        /// 1. Generates a timestamp based on the current date and time in the format "dd/MM/yyyy HH:mm:ss".
        ///
        /// 2. Appends the message with a preceding 'INFO' indication and timestamp to the 'richTextBoxLogs'.
        ///
        /// 3. Automatically scrolls to the end of the 'richTextBoxLogs' to ensure the latest log is visible.
        ///
        /// This method provides a consistent way of logging informational messages in the application.
        /// </remarks>
        private void LogMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            richTextBoxLogs.AppendText($"[INFO {timestamp}] {message}\n");
            richTextBoxLogs.ScrollToCaret(); // Scroll to the end of the logs
        }


        /// <summary>
        /// Logs an error message to the 'richTextBoxLogs' with a timestamp and error indication.
        /// </summary>
        /// <param name="message">The error message to be logged.</param>
        /// <remarks>
        /// This method performs the following operations:
        ///
        /// 1. Generates a timestamp based on the current date and time in the format "dd/MM/yyyy HH:mm:ss".
        ///
        /// 2. Adjusts the selection in the 'richTextBoxLogs' to the end, ensuring that new logs are appended.
        ///
        /// 3. Changes the text color to red to indicate an error message.
        ///
        /// 4. Appends the error message with a preceding error indication and timestamp.
        ///
        /// 5. Resets the text color back to the default for subsequent logs.
        ///
        /// 6. Automatically scrolls to the end of the 'richTextBoxLogs' to ensure the latest log is visible.
        ///
        /// The method is useful for providing consistent error logging in the application with visual distinction for errors.
        /// </remarks>
        private void ErrorMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            richTextBoxLogs.SelectionStart = richTextBoxLogs.TextLength;
            richTextBoxLogs.SelectionLength = 0;

            // Changing color to red for errors
            richTextBoxLogs.SelectionColor = Color.Red;
            richTextBoxLogs.AppendText($"[ERROR {timestamp}] {message}\n");
            richTextBoxLogs.SelectionColor = richTextBoxLogs.ForeColor;

            richTextBoxLogs.ScrollToCaret(); // Scroll to the end of the logs
        }

        private void OrderManagementForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Überprüfen Sie, ob der Benutzer das Formular wirklich schließen möchte.
            if (MessageBox.Show("Möchten Sie die Anwendung wirklich schließen und die Datenbank sichern?", "Bestätigen", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Rufen Sie hier Ihre Backup-Funktion auf.
                BackupDatabase("bestellungen.db");
            }
            else
            {
                // Abbruch des Schließvorgangs.
                e.Cancel = true;
            }
        }


        //private void LoadOrdersFromCSV()
        //{
        //    if (File.Exists("bestellungen.csv"))
        //    {
        //        using (StreamReader sr = new StreamReader("bestellungen.csv", Encoding.UTF8))
        //        {
        //            string line;
        //            while ((line = sr.ReadLine()) != null)
        //            {
        //                string[] values = line.Split(',');
        //                if (values.Length == 5)
        //                {
        //                    DataRow row = ordersTable.NewRow();
        //                    row["Nachname"] = values[0];
        //                    row["Vorname"] = values[1];
        //                    row["Größe"] = values[2];
        //                    row["Initialen"] = values[3];
        //                    row["Menge"] = int.Parse(values[4]); // Add this line
        //                    ordersTable.Rows.Add(row);
        //                }
        //            }
        //        }
        //    }
        //}


        private bool BackupDatabase(string sourceFilename)
        {
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string sourcePath = Path.Combine(currentDirectory, sourceFilename);

                if (!File.Exists(sourcePath))
                {
                    LogMessage($"Quelldatei {sourcePath} existiert nicht!");
                    return false;
                }

                string backupPath = PrepareBackupDirectoryAndPath(currentDirectory);
                BackupUsingSQLite(sourcePath, backupPath);

                LogMessage($"Backup gespeichert unter {backupPath}!");
                return true;
            }
            catch (Exception ex)
            {
                LogMessage($"Fehler beim Prozess: {ex.Message}");
                return false;
            }
        }

        private string PrepareBackupDirectoryAndPath(string currentDirectory)
        {
            string backupDirectory = Path.Combine(currentDirectory, "Backup");
            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            return Path.Combine(backupDirectory, $"database_backup_{timestamp}.db");
        }

        private void BackupUsingSQLite(string sourcePath, string backupPath)
        {
            using (var sourceConnection = new SQLiteConnection($"Data Source={sourcePath}"))
            using (var destinationConnection = new SQLiteConnection($"Data Source={backupPath}"))
            {
                sourceConnection.Open();
                destinationConnection.Open();  // Explicitly open destination connection

                sourceConnection.BackupDatabase(destinationConnection, "main", "main", -1, null, -1);
            }
        }



        /// <summary>
        /// Loads order records from the SQLite database and populates the ordersTable.
        /// </summary>
        /// <remarks>
        /// This method connects to the SQLite database 'bestellungen.db' and fetches all the records from the 'Orders' table.
        /// Each record is then converted into a DataRow and added to the ordersTable.
        /// 
        /// Assumptions:
        /// - The SQLite database has been properly initialized and contains a table named 'Orders'.
        /// - The 'Orders' table contains columns: 'Nachname', 'Vorname', 'Größe', 'Initialen', and 'Menge'.
        /// - The ordersTable DataTable has corresponding columns to hold the data.
        /// - The 'Menge' column in the SQLite table contains integers.
        /// 
        /// Note: Exception handling for database access or data conversion errors has not been implemented in this method.
        /// </remarks>
        private void LoadOrdersFromSQLite()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT Nachname, Vorname, Größe, Initialen, Menge FROM Orders", conn))
                {
                    conn.Open();
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataRow row = ordersTable.NewRow();
                            row["Nachname"] = reader["Nachname"];
                            row["Vorname"] = reader["Vorname"];
                            row["Größe"] = reader["Größe"];
                            row["Initialen"] = reader["Initialen"];

                            if (int.TryParse(reader["Menge"].ToString(), out int mengeValue))
                            {
                                row["Menge"] = mengeValue;
                            }
                            else
                            {
                                // Handle invalid integer data
                                // Maybe set a default value or log an error
                                row["Menge"] = 0;
                            }

                            ordersTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here, for example, log the error or show a user-friendly message
                LogMessage($"Error loading orders: {ex.Message}");
            }
        }


        /// <summary>
        /// Initializes the ordersTable with appropriate columns and binds it to the dataGridViewOrders control.
        /// </summary>
        /// <remarks>
        /// This method sets up the structure for the ordersTable DataTable by defining the required columns:
        /// - 'Nachname'
        /// - 'Vorname'
        /// - 'Größe'
        /// - 'Initialen'
        /// - 'Menge' (with a data type of integer)
        /// 
        /// After defining the columns, the method then sets the dataGridViewOrders control's DataSource property to this ordersTable, allowing the control to display the table's data.
        /// 
        /// Note: It's assumed that the dataGridViewOrders control has been properly initialized prior to calling this method.
        /// </remarks>
        private void InitializeOrdersTable()
        {
            ordersTable = new DataTable();
            ordersTable.Columns.Add("Nachname");
            ordersTable.Columns.Add("Vorname");
            ordersTable.Columns.Add("Größe");
            ordersTable.Columns.Add("Initialen");
            ordersTable.Columns.Add("Menge", typeof(int));

            dataGridViewOrders.DataSource = ordersTable;
        }


        /// <summary>
        /// Initializes the SQLite database for storing orders. If the database file does not exist, it is created.
        /// </summary>
        /// <remarks>
        /// This method checks if the 'bestellungen.db' file exists in the application's directory.
        /// If it doesn't exist, the method creates a new SQLite database file with that name.
        ///
        /// Once the database file is ensured to exist, the method establishes a connection to the database 
        /// and checks for the existence of the 'Orders' table. If the table doesn't exist, it is created 
        /// with the following columns:
        /// - 'Nachname' (Text type)
        /// - 'Vorname' (Text type)
        /// - 'Größe' (Text type)
        /// - 'Initialen' (Text type)
        /// - 'Menge' (Integer type)
        ///
        /// This method should be called during the initialization phase of the application or before 
        /// any database operations to ensure the database structure is in place.
        /// </remarks>
        private void InitializeDatabase()
        {
            if (!File.Exists("bestellungen.db"))
            {
                SQLiteConnection.CreateFile("bestellungen.db");
            }

            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                conn.Open();

                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Orders 
                            (Nachname TEXT, Vorname TEXT, Größe TEXT, Initialen TEXT, Menge INT)";
                cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Initializes and styles the controls present on the form.
        /// </summary>
        /// <remarks>
        /// This method performs the following operations:
        /// 
        /// 1. Populates the 'comboBoxSize' control with the size options ("S", "M", "L", "XL").
        ///
        /// 2. Applies default styling (color and font) to all controls on the form.
        /// 
        /// 3. Applies specific styling to controls based on their type:
        ///    - For Button controls: Sets the background, foreground color, border style, etc.
        ///    - For ComboBox controls: Sets the dropdown style.
        ///    - For DataGridView controls: Sets the background, border style, grid color, row headers, column header styles, etc.
        ///    - Additional controls can have specific styling added in this section.
        ///
        /// 4. Applies hover effects to Button controls, changing their appearance when the mouse cursor enters and leaves the button area.
        /// 
        /// Ensure that this method is called during form initialization, after control properties have been set and before the form is displayed.
        /// </remarks>
        private void InitializeControls()
        {
            comboBoxSize.Items.Add("S");
            comboBoxSize.Items.Add("M");
            comboBoxSize.Items.Add("L");
            comboBoxSize.Items.Add("XL");
            comboBoxSize.Items.Add("2XL");
            comboBoxSize.Items.Add("3XL");

            // Styling
            foreach (Control ctrl in this.Controls)
            {
                // Default Control Styling
                ctrl.ForeColor = System.Drawing.Color.DarkRed;
                ctrl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);

                // Specific Control Styling
                switch (ctrl)
                {
                    case System.Windows.Forms.Button button:
                        button.BackColor = System.Drawing.Color.DarkRed;
                        button.ForeColor = System.Drawing.Color.White;
                        button.FlatStyle = FlatStyle.Flat;
                        button.FlatAppearance.BorderColor = System.Drawing.Color.White;
                        button.FlatAppearance.BorderSize = 1;

                        // Button Hover Effects
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
                        break;

                    case System.Windows.Forms.ComboBox comboBox:
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        break;

                    case System.Windows.Forms.DataGridView dgv:
                        dgv.BackgroundColor = System.Drawing.Color.White;
                        dgv.BorderStyle = BorderStyle.None;
                        dgv.GridColor = System.Drawing.Color.DarkRed;
                        dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightCoral;
                        dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                        dgv.RowHeadersVisible = false;
                        dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.DarkRed;
                        dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                        dgv.EnableHeadersVisualStyles = false;
                        break;

                    // Add any TextBox styling here if necessary
                    case System.Windows.Forms.TextBox txt:
                        break;

                    case System.Windows.Forms.Label label:
                        label.BackColor = Color.DarkRed;
                        label.ForeColor = Color.White;
                        label.Paint += (s, e) =>
                        {
                            ControlPaint.DrawBorder(e.Graphics, label.DisplayRectangle, Color.White, ButtonBorderStyle.Solid);
                        };
                        break;
                }
            }
        }



        /// <summary>
        /// Saves the provided order data to the SQLite database.
        /// </summary>
        /// <param name="order">The DataRow representing the order to be saved.</param>
        /// <remarks>
        /// This method performs the following operations:
        /// 
        /// 1. Establishes a connection with the SQLite database 'bestellungen.db'.
        ///
        /// 2. Constructs an INSERT SQL statement to add the order data to the 'Orders' table in the database.
        /// 
        /// 3. Binds the values from the provided DataRow 'order' to the SQL statement parameters.
        ///
        /// 4. Executes the SQL statement, thereby saving the order to the database.
        ///
        /// Ensure that the DataRow 'order' passed to this method contains the necessary columns 
        /// (Nachname, Vorname, Größe, Initialen, Menge) and that the database connection string is correctly configured.
        /// </remarks>
        private void SaveOrderToSQLite(DataRow order)
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source=bestellungen.db"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = @"INSERT INTO Orders (Nachname, Vorname, Größe, Initialen, Menge) 
                               VALUES (@Nachname, @Vorname, @Größe, @Initialen, @Menge)";
                    cmd.Parameters.AddWithValue("@Nachname", order["Nachname"]);
                    cmd.Parameters.AddWithValue("@Vorname", order["Vorname"]);
                    cmd.Parameters.AddWithValue("@Größe", order["Größe"]);
                    cmd.Parameters.AddWithValue("@Initialen", order["Initialen"]);
                    cmd.Parameters.AddWithValue("@Menge", order["Menge"]);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }



        /// <summary>
        /// Event handler for the "Add Order" button click.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button itself.</param>
        /// <param name="e">Details about the click event.</param>
        /// <remarks>
        /// This method performs the following operations upon button click:
        /// 
        /// 1. Creates a new DataRow 'row' and populates it with data from the UI controls (textboxes, combobox, etc.).
        ///
        /// 2. Adds the populated 'row' to the orders DataTable 'ordersTable'.
        ///
        /// 3. Invokes the <see cref="SaveOrderToSQLite"/> method to save the order details to the SQLite database.
        ///
        /// 4. Clears all the input controls in preparation for a new entry.
        ///
        /// Ensure that all the required UI controls are appropriately initialized and accessible. 
        /// This method assumes that 'ordersTable' is properly instantiated.
        /// </remarks>
        private void BtnAddOrder_Click(object sender, EventArgs e)
        {
            DataRow row = ordersTable.NewRow();
            row["Nachname"] = txtNachname.Text;
            row["Vorname"] = txtVorname.Text;
            row["Initialen"] = txtInitialen.Text;
            row["Größe"] = comboBoxSize.SelectedItem.ToString();
            row["Menge"] = numericUpDownQuantity.Value;
            ordersTable.Rows.Add(row);

            // Save to SQLite
            SaveOrderToSQLite(row);

            // Clear the input controls
            txtNachname.Clear();
            txtVorname.Clear();
            txtInitialen.Clear();
            comboBoxSize.SelectedIndex = -1; // deselect any selected item
            numericUpDownQuantity.Value = 1;
        }

        private void BtnBackupDatabase_Click(object sender, EventArgs e)
        {
            BackupDatabase("bestellungen.db");
        }


        /// <summary>
        /// Event handler for the "Save Orders" button click.
        /// </summary>
        /// <param name="sender">The source of the event, typically the button itself.</param>
        /// <param name="e">Details about the click event.</param>
        /// <remarks>
        /// This method performs the following operations upon button click:
        /// 
        /// 1. Initiates a StreamWriter to write to the 'bestellungen.csv' file with UTF8 encoding.
        ///
        /// 2. Iterates through each DataRow in 'ordersTable' and writes the order details as comma-separated values to the CSV file.
        ///
        /// 3. Closes the StreamWriter upon completion.
        ///
        /// 4. Calls the <see cref="LogMessage"/> method to log the success message.
        ///
        /// Note: It's advisable to handle potential exceptions that might arise, especially when working with file I/O.
        /// </remarks>
        private void BtnSaveOrders_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("bestellungen.csv", false, Encoding.UTF8))
            {
                foreach (DataRow row in ordersTable.Rows)
                {
                    sw.WriteLine($"{row["Nachname"]},{row["Vorname"]},{row["Größe"]},{row["Initialen"]},{row["Menge"]}");
                }

            }
            LogMessage("Bestellungen gespeichert!");
        }


        /// <summary>
        /// Evaluates and summarizes order quantities based on sizes.
        /// </summary>
        /// <remarks>
        /// This method performs the following operations:
        /// 
        /// 1. Iterates through each DataRow in 'ordersTable' and constructs a summary of total quantities ordered for each size.
        ///
        /// 2. Writes the summarized order data to a CSV file named 'bestellzusammenfassung.csv' with UTF8 encoding.
        ///
        /// 3. The CSV file will contain two columns: 'Größe' (Size) and 'Menge' (Quantity).
        ///
        /// 4. Constructs a string representation of the order summary and logs it using the <see cref="LogMessage"/> method.
        ///
        /// 5. Logs a success message indicating that the order summary has been saved successfully in CSV format.
        ///
        /// Note: It's advisable to handle potential exceptions that might arise, especially when working with file I/O or data manipulation.
        /// </remarks>
        private void EvaluateOrderQuantities()
        {
            var summary = SummarizeOrders();
            SaveSummaryToCsv(summary);
            LogSummary(summary);
        }

        private Dictionary<string, int> SummarizeOrders()
        {
            Dictionary<string, int> summary = new Dictionary<string, int>();

            foreach (DataRow row in ordersTable.Rows)
            {
                string size = row[SIZE_COLUMN]?.ToString();
                if (size == null || !int.TryParse(row[QUANTITY_COLUMN]?.ToString(), out int quantity))
                    continue;

                if (summary.TryGetValue(size, out int currentQuantity))
                {
                    summary[size] = currentQuantity + quantity;
                }
                else
                {
                    summary[size] = quantity;
                }
            }

            return summary;
        }

        private void SaveSummaryToCsv(Dictionary<string, int> summary)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("bestellzusammenfassung.csv", false, Encoding.UTF8))
                {
                    sw.WriteLine($"{SIZE_COLUMN},{QUANTITY_COLUMN}");
                    foreach (var entry in summary)
                    {
                        sw.WriteLine($"{entry.Key},{entry.Value}");
                    }
                }
                LogMessage("Bestellzusammenfassung erfolgreich im CSV-Format gespeichert!");
            }
            catch (Exception ex)
            {
                LogMessage($"Fehler beim Speichern der CSV: {ex.Message}");
            }
        }

        private void LogSummary(Dictionary<string, int> summary)
        {
            StringBuilder orderSummary = new StringBuilder("Bestellzusammenfassung:\n\n");
            foreach (var entry in summary)
            {
                orderSummary.AppendLine($"Größe {entry.Key}: {entry.Value} Stück");
            }
            LogMessage(orderSummary.ToString());
        }

        //private void InitializeTooltips()
        //{
        //    toolStrip1.SetToolTip(this.label1, "Beschreibung für label1"); // Ersetzen Sie durch eine genauere Nachricht basierend auf dem Zweck des Labels
        //    toolStrip1.SetToolTip(this.label2, "Beschreibung für label2");
        //    // ... Für andere Labels wiederholen ...

        //    toolStrip1.SetToolTip(this.numericUpDownQuantity, "Setzen Sie die Menge mit den Auf- und Ab-Pfeilen.");

        //    toolStrip1.SetToolTip(this.txtNachname, "Geben Sie hier den Nachnamen ein.");
        //    toolStrip1.SetToolTip(this.txtVorname, "Geben Sie hier den Vornamen ein.");
        //    toolStrip1.SetToolTip(this.txtInitialen, "Geben Sie die Initialen hier ein.");

        //    toolStrip1.SetToolTip(this.comboBoxSize, "Wählen Sie die gewünschte Größe aus dem Dropdown-Menü aus.");

        //    toolStrip1.SetToolTip(this.btnAddOrder, "Klicken Sie hier, um eine neue Bestellung mit den angegebenen Details hinzuzufügen.");
        //    toolStrip1.SetToolTip(this.dataGridViewOrders, "Hier können Sie alle Bestellungen ansehen und verwalten.");
        //    toolStrip1.SetToolTip(this.btnSaveSummary, "Speichern Sie eine Zusammenfassung aller Bestellungen.");
        //    toolStrip1.SetToolTip(this.btnOpenFolder, "Öffnen Sie das Verzeichnis, in dem die Bestellungen gespeichert sind.");
        //    toolStrip1.SetToolTip(this.BtnSaveOrders, "Speichern Sie alle aktuellen Bestellungen in der Datenbank.");
        //    toolStrip1.SetToolTip(this.buttonBackupDatabase, "Erstellen Sie eine Sicherungskopie der aktuellen Datenbank.");

        //    toolStrip1.SetToolTip(this.richTextBoxLogs, "Hier können Sie Protokollnachrichten und Vorgänge ansehen.");

        //    // Wenn die ToolStrip spezifische Funktionen oder Tools hat, können für einzelne Elemente innerhalb von ihr Tooltips hinzugefügt werden
        //}


        /// <summary>
        /// Handles the click event of the 'btnOpenFolder' button. Opens the folder containing the application executable in the default file explorer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// This method performs the following operations:
        ///
        /// 1. Tries to determine the directory in which the current executable resides using the <see cref="Application.ExecutablePath"/>.
        ///
        /// 2. If successful, opens the determined directory in the default file explorer using the explorer.exe command.
        ///
        /// 3. In case of any issues (like a null directory path or an exception during the process launch), provides feedback to the user through log messages or message boxes.
        ///
        /// It is recommended to handle potential exceptions that might arise when dealing with file paths and launching external processes.
        /// </remarks>
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
                MessageBox.Show($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }


        /// <summary>
        /// Handles the Paint event of the 'dataGridViewOrders' control. Draws a custom border around the DataGridView.
        /// </summary>
        /// <param name="sender">The source of the event, typically the DataGridView itself.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data, including a reference to the graphics used for painting.</param>
        /// <remarks>
        /// This method performs the following operations:
        ///
        /// 1. Determines the boundary of the DataGridView using its Bounds property.
        ///
        /// 2. Optionally, it adjusts (shrinks) the boundary rectangle slightly for visual purposes using the Inflate method.
        ///
        /// 3. Uses the ControlPaint utility's DrawBorder method to paint a solid red border around the adjusted boundary of the DataGridView.
        ///
        /// This is particularly useful when custom visual effects are required for the DataGridView that aren't provided by default properties and methods.
        /// </remarks>
        private void DataGridViewOrders_Paint(object sender, PaintEventArgs e)
        {
            Rectangle borderRectangle = dataGridViewOrders.Bounds;
            borderRectangle.Inflate(-1, -1);  // Optionally shrink the rectangle slightly if you want the border inside the DGV's bounds
            ControlPaint.DrawBorder(e.Graphics, borderRectangle, Color.Red, ButtonBorderStyle.Solid);
        }

        private void comboBoxSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Placeholder
        }
    }
}
