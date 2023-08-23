# 🚀 SVU-Bestellungen Dokumentation

## 🎯 Overview

This project focuses on managing jersey orders using a CSV-based approach. It includes functionalities ranging from loading data from a CSV, managing data through a DataTable, to providing a user-friendly interface for operations.

## 📌 Table of Contents

1. [Initialization](#initialization)
2. [Loading CSV](#loading-csv)
3. [DataTable](#datatable)
4. [Controls Initialization](#controls-initialization)
5. [Event Handlers](#event-handlers)
6. [Appearance & UI Interactions](#appearance--ui-interactions)
7. [Miscellaneous](#miscellaneous)

* * *

## 🎨 Initialization

* The form kicks off with a custom look achieved through `InitializeControls()`.
* A `DataTable` is set up to mirror jersey orders using the `InitializeOrdersTable()` function.
* The system retrieves orders from a `bestellungen.csv` file via `LoadOrdersFromCSV()`.

## 📂 Loading CSV

* The `LoadOrdersFromCSV()` function reads orders from a CSV file, seamlessly populating the `DataTable` with this valuable data.

## 📊 DataTable

* The table, named `ordersTable`, houses columns like:
    * `Nachname` (🧑‍🦰 Last Name)
    * `Vorname` (👩 First Name)
    * `Größe` (📏 Size)
    * `Initialen` (🔤 Initials)
    * `Menge` (🛍️ Quantity)
* The visual representation of this table is facilitated by `dataGridViewOrders`.

## 🔨 Controls Initialization

* With the power of `InitializeControls()`, the size combo box (`comboBoxSize`) gets populated.
* Styling for controls such as buttons, text boxes, combo boxes, and the data grid are also defined here.

## 🖱️ Event Handlers

* `BtnAddOrder_Click` ➡️ An order, based on user input, gets a place in the `DataTable`.
* `BtnSaveOrders_Click` ➡️ What's in the `DataTable` gets written to the CSV file.
* `btnOpenFolder_Click` ➡️ Ever wanted to quickly check where the app's operating? This opens that exact folder in your default file explorer.
* `DataGridViewOrders_Paint` ➡️ This grants the DataGridView a tailor-made paint job for its borders.
* `EvaluateOrderQuantities` ➡️ A keen eye that assesses ordered quantities based on jersey size. This analysis finds its way to a new CSV named `bestellzusammenfassung.csv` and subsequently, a messagebox spells out the summary.

## ✨ Appearance & UI Interactions

* The interface dazzles with specified UI colors and fonts.
* Buttons come alive with hover effects.
* As the form springs into action, the spotlight falls on the `txtNachname` text box.
* The data grid wears a custom border, thanks to the paint job.

## 🔍 Miscellaneous

* Hidden in comments is a gem – the code to endow the form with a background image, drawing from an embedded resource (maybe it's a captivating jpg image? 😉).

* * *
