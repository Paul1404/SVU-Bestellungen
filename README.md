# SVU-Bestellungen Dokumentation

1. **Initialization** : 
- The form is initialized with a specific appearance (`InitializeControls()`). 
- A `DataTable` is created to represent jersey orders (`InitializeOrdersTable()`). 
- Orders are loaded from a `bestellungen.csv` file (`LoadOrdersFromCSV()`). 
2. **Loading CSV** : 
- Reads orders from a CSV file and populates the `DataTable` with this data (`LoadOrdersFromCSV()`). 
3. **DataTable** : 
- This table (`ordersTable`) contains the following columns: `Nachname` (last name), `Vorname` (first name), `Größe` (size), `Initialen` (initials), and `Menge` (quantity). The data grid (`dataGridViewOrders`) displays this table. 
4. **Controls Initialization** : 
- The `InitializeControls()` function adds items to the size combo box (`comboBoxSize`), sets styles for controls like buttons, text boxes, combo boxes, and the data grid. 
5. **Event Handlers** : 
- `BtnAddOrder_Click`: Adds an order to the `DataTable` based on the provided user input. 
- `BtnSaveOrders_Click`: Writes the orders from the `DataTable` to the CSV file. 
- `btnOpenFolder_Click`: Opens the folder where the application is running in the default file explorer. 
- `DataGridViewOrders_Paint`: Provides a custom paint job for the borders of the DataGridView. 
- `EvaluateOrderQuantities`: Evaluates the quantities ordered per jersey size and saves this summary to a new CSV (`bestellzusammenfassung.csv`). It then displays the summary in a message box. 
6. **Appearance & UI Interactions** :
- The UI colors and fonts are specified. There are hover effects for buttons. 
- When the form loads, the focus is set on the `txtNachname` (last name) text box.
- A custom border is drawn around the data grid when it's painted. 
7. **Miscellaneous** :
- There's commented code to set a background image for the form, which uses an embedded resource (possibly a jpg image).
