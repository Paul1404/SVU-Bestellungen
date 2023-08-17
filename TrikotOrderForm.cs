using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


namespace SVU_Bestellungen
{
    public partial class TrikotOrderForm : Form
    {
        private DataTable ordersTable;

        public TrikotOrderForm()
        {
            InitializeComponent();
            InitializeOrdersTable();
            InitializeControls();
            LoadOrdersFromCSV();
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
        }

        private void LoadOrdersFromCSV()
        {
            if (File.Exists("bestellungen.csv"))
            {
                using (StreamReader sr = new StreamReader("bestellungen.csv", Encoding.UTF8))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        if (values.Length == 4) // Ensure that there are 4 columns in the CSV
                        {
                            DataRow row = ordersTable.NewRow();
                            row["Artikel"] = values[0];
                            row["Größe"] = values[1];
                            row["Farbe"] = values[2];
                            row["Menge"] = values[3];
                            ordersTable.Rows.Add(row);
                        }
                    }
                }
            }
        }


        private void InitializeOrdersTable()
        {
            ordersTable = new DataTable();
            ordersTable.Columns.Add("Artikel");
            ordersTable.Columns.Add("Größe");
            ordersTable.Columns.Add("Farbe");
            ordersTable.Columns.Add("Menge");

            dataGridViewOrders.DataSource = ordersTable;
        }

        private void InitializeControls()
        {
            comboBoxArticle.Items.Add("Trikot");
            comboBoxArticle.Items.Add("Polo");

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
            row["Artikel"] = comboBoxArticle.SelectedItem?.ToString() ?? string.Empty;
            row["Größe"] = comboBoxSize.SelectedItem?.ToString() ?? string.Empty;
            row["Farbe"] = txtColor.Text;
            row["Menge"] = numericUpDownQuantity.Value.ToString();
            ordersTable.Rows.Add(row);
        }

        private void BtnSaveOrders_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("bestellungen.csv", false, Encoding.UTF8))
            {
                foreach (DataRow row in ordersTable.Rows)
                {
                    sw.WriteLine(string.Join(",", row.ItemArray));
                }
            }
            MessageBox.Show("Bestellungen gespeichert!");
        }

        private void EvaluateOrderQuantities()
        {
            // Key: Artikel|Größe|Farbe, Value: Gesamtmenge
            Dictionary<string, int> summary = new Dictionary<string, int>();

            foreach (DataRow row in ordersTable.Rows)
            {
                string article = row["Artikel"].ToString();
                string size = row["Größe"].ToString();
                string color = row["Farbe"].ToString();
                int quantity = int.Parse(row["Menge"].ToString());

                string key = $"{article}|{size}|{color}";

                if (summary.ContainsKey(key))
                {
                    summary[key] += quantity;
                }
                else
                {
                    summary[key] = quantity;
                }
            }

            // Speichern der zusammengefassten Daten in einer CSV-Datei
            using (StreamWriter sw = new StreamWriter("bestellzusammenfassung.csv", false, Encoding.UTF8))
            {
                // Header
                sw.WriteLine("Artikel,Größe,Farbe,Menge");

                foreach (var entry in summary)
                {
                    string[] parts = entry.Key.Split('|');
                    sw.WriteLine($"{parts[0]},{parts[1]},{parts[2]},{entry.Value}");
                }
            }

            // Anzeige der zusammengefassten Daten
            StringBuilder orderSummary = new StringBuilder("Bestellzusammenfassung:\n\n");
            foreach (var entry in summary)
            {
                orderSummary.AppendLine($"{entry.Key.Replace("|", " - ")}: {entry.Value} Stück");
            }

            MessageBox.Show(orderSummary.ToString());
            MessageBox.Show("Bestellzusammenfassung erfolgreich im CSV-Format gespeichert!");
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
                    MessageBox.Show("Konnte den Ordner nicht finden.");
                }
            }
            catch (Exception ex)
            {
                // Display a user-friendly message
                MessageBox.Show($"Ein Fehler ist aufgetreten: {ex.Message}");
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
