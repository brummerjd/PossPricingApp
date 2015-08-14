using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace PossPricingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string DB_CONN_STRING;

        private PossPricingDatabase _DB;

        public bool EditMode
        {
            get { return this._EditMode; }
            set { this._EditMode = value; OnPropertyChanged("EditMode"); }
        }
        private bool _EditMode;

        public bool ShowAllCharges
        {
            get { return this._ShowAllCharges; }
            set { this._ShowAllCharges = value; OnPropertyChanged("ShowAllCharges"); }
        }
        private bool _ShowAllCharges;

        private int _OldChargeKey;

        #region Lists and Selected variables
        public List<Customer> Customers
        {
            get { return this._Customers; }
            set { this._Customers = value; OnPropertyChanged("Customers"); }
        }
        private List<Customer> _Customers;

        public Customer SelectedCustomer
        {
            get { return this._SelectedCustomer; }
            set { this._SelectedCustomer = value; OnPropertyChanged("SelectedCustomer"); }
        }
        private Customer _SelectedCustomer;

        public WorkDay SelectedWorkDay
        {
            get { return this._SelectedWorkDay; }
            set { this._SelectedWorkDay = value; OnPropertyChanged("SelectedWorkDay"); }
        }
        private WorkDay _SelectedWorkDay = new WorkDay();

        public MiscCharge SelectedMiscCharge
        {
            get { return this._SelectedMiscCharge; }
            set { this._SelectedMiscCharge = value; OnPropertyChanged("SelectedMiscCharge"); }
        }
        private MiscCharge _SelectedMiscCharge = new MiscCharge();

        public DateTime SelectedDate
        {
            get { return this._SelectedDate; }
            set { this._SelectedDate = value; OnPropertyChanged("SelectedDate"); }
        }
        private DateTime _SelectedDate = DateTime.Today;

        public ObservableCollection<Charge> Charges
        {
            get { return this._Charges; }
            set { this._Charges = value; OnPropertyChanged("Charges"); }
        }
        private ObservableCollection<Charge> _Charges;

        public Charge SelectedCharge
        {
            get { return this._SelectedCharge; }
            set { this._SelectedCharge = value; OnPropertyChanged("SelectedCharge"); }
        }
        private Charge _SelectedCharge;
        #endregion

        #region PricePer variables
        public double PricePerTonDistillers
        {
            get { return this._PricePerTonDistillers; }
            set { this._PricePerTonDistillers = value; OnPropertyChanged("PricePerTonDistillers"); }
        }
        private double _PricePerTonDistillers;

        public double PricePerTonHay
        {
            get { return this._PricePerTonHay; }
            set { this._PricePerTonHay = value; OnPropertyChanged("PricePerTonHay"); }
        }
        private double _PricePerTonHay;

        public double PricePerTonCorn
        {
            get { return this._PricePerTonCorn; }
            set { this._PricePerTonCorn = value; OnPropertyChanged("PricePerTonCorn"); }
        }
        private double _PricePerTonCorn;

        public double PricePerTonMinerals
        {
            get { return this._PricePerTonMinerals; }
            set { this._PricePerTonMinerals = value; OnPropertyChanged("PricePerTonMinerals"); }
        }
        private double _PricePerTonMinerals;

        public double PricePerHeadYardage
        {
            get { return this._PricePerHeadYardage; }
            set { this._PricePerHeadYardage = value; OnPropertyChanged("PricePerHeadYardage"); }
        }
        private double _PricePerHeadYardage;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = myWindow;
        }

        private void myWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //JDB.Library.DatabaseFunctions.GenerateDatabaseClassFile(
            //    "PossPricingDatabase",
            //    "C:\\Users\\Josh\\Documents\\PossDatabase.sdf",
            //    "C:\\Users\\Josh\\Documents\\PossDatabase.cs"
            //);
            //return;

            if (string.IsNullOrEmpty(PossPricingApp.Properties.Settings.Default.StorageLocation) || !Directory.Exists(PossPricingApp.Properties.Settings.Default.StorageLocation))
            {
                string storageLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RHD Application Contents");
                if (Directory.Exists(storageLocation))
                {
                    PossPricingApp.Properties.Settings.Default.StorageLocation = storageLocation;
                    PossPricingApp.Properties.Settings.Default.Save();
                }
                else
                {
                    storageLocation = JDB.Library.DatabaseFunctions.GetDatabaseLocation("Select folder containing application contents.");
                    if (!string.IsNullOrEmpty(storageLocation))
                    {
                        PossPricingApp.Properties.Settings.Default.StorageLocation = storageLocation;
                        PossPricingApp.Properties.Settings.Default.Save();
                    }
                    else
                    {
                        this.CloseMenuItem_Click(this, null);
                    }
                }
            }

            if (Directory.GetFiles(PossPricingApp.Properties.Settings.Default.StorageLocation, "PossDatabase.sdf", SearchOption.TopDirectoryOnly).Length == 0)
            {
                PossPricingApp.Properties.Settings.Default.StorageLocation = "";
                PossPricingApp.Properties.Settings.Default.Save();

                MessageBox.Show("Database file not found in specified contents folder. Try re-opening and selecting different contents folder.");
                this.CloseMenuItem_Click(this, null);
            }

            string receiptsFolderLocation = Path.Combine(PossPricingApp.Properties.Settings.Default.StorageLocation, "Receipts");
            if (!Directory.Exists(receiptsFolderLocation)) { Directory.CreateDirectory(receiptsFolderLocation); }

            string databaseBackupsFolderLocation = Path.Combine(PossPricingApp.Properties.Settings.Default.StorageLocation, "Database Backups");
            if (!Directory.Exists(databaseBackupsFolderLocation)) { Directory.CreateDirectory(databaseBackupsFolderLocation); }

            System.Globalization.CultureInfo cul = System.Globalization.CultureInfo.CurrentCulture;
            int week = cul.Calendar.GetWeekOfYear(DateTime.Today, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            string currentDatabaseBackupPath = Path.Combine(databaseBackupsFolderLocation, string.Format("PossDatabaseBackup_{0:00}_{1:yyyy}.sdf", week, DateTime.Today));
            if (!File.Exists(currentDatabaseBackupPath))
            {
                File.Copy(Path.Combine(PossPricingApp.Properties.Settings.Default.StorageLocation, "PossDatabase.sdf"), currentDatabaseBackupPath);
            }

            DB_CONN_STRING = Path.Combine(PossPricingApp.Properties.Settings.Default.StorageLocation, "PossDatabase.sdf");

            this._DB = new PossPricingDatabase
            (
                DB_CONN_STRING
            );

            this.RefreshCustomerList();

            this.PricePerTonDistillers = PossPricingApp.Properties.Settings.Default.PricePerTonDistillers;
            this.PricePerTonCorn = PossPricingApp.Properties.Settings.Default.PricePerTonCorn;
            this.PricePerTonHay = PossPricingApp.Properties.Settings.Default.PricePerTonHay;
            this.PricePerTonMinerals = PossPricingApp.Properties.Settings.Default.PricePerTonMinerals;
            this.PricePerHeadYardage = PossPricingApp.Properties.Settings.Default.PricePerHeadYardage;

            this.BetweenDatePicker1.SelectedDate = DateTime.Today.AddMonths(-1);
            this.BetweenDatePicker2.SelectedDate = DateTime.Today;
        }

        private void myWindow_Closed(object sender, EventArgs e)
        {
            PossPricingApp.Properties.Settings.Default.PricePerTonDistillers = this.PricePerTonDistillers;
            PossPricingApp.Properties.Settings.Default.PricePerTonCorn = this.PricePerTonCorn;
            PossPricingApp.Properties.Settings.Default.PricePerTonHay = this.PricePerTonHay;
            PossPricingApp.Properties.Settings.Default.PricePerTonMinerals = this.PricePerTonMinerals;
            PossPricingApp.Properties.Settings.Default.PricePerHeadYardage = this.PricePerHeadYardage;
            PossPricingApp.Properties.Settings.Default.Save();

            this._DB.SubmitChanges();
        }

        private void RandomChargeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();

            this.PricePerTonDistillers = Math.Round(r.NextDouble(), 2);
            this.PricePerTonHay = Math.Round(r.NextDouble(), 2);
            this.PricePerTonCorn = Math.Round(r.NextDouble(), 2);
            this.PricePerTonMinerals = Math.Round(r.NextDouble(), 2);
            this.PricePerHeadYardage = Math.Round(r.NextDouble(), 2);

            this.SelectedWorkDay = new WorkDay();
            this.SelectedDate = DateTime.Today;
            this.SelectedWorkDay.PoundsDistillers = r.Next(200);
            this.SelectedWorkDay.PoundsHay = r.Next(200);
            this.SelectedWorkDay.PoundsCorn = r.Next(200);
            this.SelectedWorkDay.PoundsMinerals = r.Next(200);
            this.SelectedWorkDay.NumberOfHeadYardage = r.Next(200);

            this.AddChargeButton_Click(null, null);
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender == null) { return; }

            (sender as TextBox).SelectAll();
        }

        private void SelectedCustomer_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectedCustomer == null) { return; }

            var charges  = new ObservableCollection<Charge>();
            foreach (WorkDay day in this._DB.WorkDay.Where(o => (this.ShowAllCharges ? true : o.Printed == null) && o.CustomerID == this.SelectedCustomer.CustomerID))
            {
                charges.Add(new Charge(day));
            }
            foreach (MiscCharge misc in this._DB.MiscCharge.Where(o => (this.ShowAllCharges ? true : o.Printed == null) && o.CustomerID == this.SelectedCustomer.CustomerID))
            {
                charges.Add(new Charge(misc));
            }

            this.Charges = new ObservableCollection<Charge>(charges.OrderByDescending(o => o.Date));
        }

        private void ShowAllChargesCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedCustomer_Changed(this, null);
        }

        private void ChargesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Charge charge = this.SelectedCharge;

            if (charge == null) { return; }

            this.SelectedDate = charge.Date;

            if (charge.Type == ChargeType.WorkDay)
            {
                this.WorkDayTab.IsSelected = true;

                this.SelectedMiscCharge = null;

                this.SelectedWorkDay = Charge.Clone(this._DB.WorkDay.Where(o => o.WorkDayKey == charge.Key).FirstOrDefault());
                
                this.PricePerTonDistillers = this.SelectedWorkDay.PricePerTonDistillers;
                this.PricePerTonHay = this.SelectedWorkDay.PricePerTonHay;
                this.PricePerTonCorn = this.SelectedWorkDay.PricePerTonCorn;
                this.PricePerTonMinerals = this.SelectedWorkDay.PricePerTonMinerals;
                this.PricePerHeadYardage = this.SelectedWorkDay.PricePerHeadYardage;
            }
            else
            {
                this.MiscTab.IsSelected = true;

                this.SelectedWorkDay = null;

                this.SelectedMiscCharge = Charge.Clone(this._DB.MiscCharge.Where(o => o.MiscChargeKey == charge.Key).FirstOrDefault());
            }
        }

        private void EditChargeItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCharge == null) { return; }

            this._OldChargeKey = this.SelectedCharge.Key;
            if (this.SelectedCharge.Type == ChargeType.WorkDay) { WorkDayTab.IsEnabled = true; }
            else { MiscTab.IsEnabled = true; }

            this.EditMode = true;
        }

        private void DeleteChargeItem_Click(object sender, RoutedEventArgs e)
        {
            Charge charge = this.SelectedCharge;

            if (charge == null) { return; }

            if (MessageBox.Show("Are you sure you want to delete this charge? This action cannot be undone.", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (charge.Type == ChargeType.WorkDay)
                {
                    WorkDay day = this._DB.WorkDay.Where(o => o.WorkDayKey == charge.Key).First();
                    this._DB.WorkDay.DeleteOnSubmit(day);
                    this._DB.SubmitChanges();
                }
                else
                {
                    MiscCharge misc = this._DB.MiscCharge.Where(o => o.MiscChargeKey == charge.Key).First();
                    this._DB.MiscCharge.DeleteOnSubmit(misc);
                    this._DB.SubmitChanges();
                }

                this.SelectedCustomer_Changed(null, null);
            }
        }

        private void NewCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            JDB.Library.Controls.CustomInputBox win = new JDB.Library.Controls.CustomInputBox();

            string cust = win.Show(this, "Enter name of customer:", "New Customer", "");

            if (!string.IsNullOrEmpty(cust) && win.WasOkClicked)
            {
                _DB.Customer.InsertOnSubmit(new Customer
                {
                    Name = cust
                });
                _DB.SubmitChanges();

                this.RefreshCustomerList();

                long maxID = this.Customers.Max(o => o.CustomerID);
                this.SelectedCustomer = this.Customers.Where(o => o.CustomerID == maxID).FirstOrDefault();
            }
        }

        private void RefreshCustomerList()
        {
            this.Customers = this._DB.Customer.OrderBy(o => o.Name).ToList();
        }

        private void AddChargeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCustomer == null)
            {
                MessageBox.Show("No customer selected. Please select one before adding charge information.");
                return;
            }

            bool addingWorkDay = this.WorkDayTab.IsSelected;
            int chargeKey;
            if (addingWorkDay)
            {
                chargeKey = this.SelectedWorkDay.WorkDayKey;

                this.SelectedWorkDay.CustomerID = this.SelectedCustomer.CustomerID;
                this.SelectedWorkDay.Date = this.SelectedDate;
                this.SelectedWorkDay.PricePerTonDistillers = this.PricePerTonDistillers;
                this.SelectedWorkDay.PricePerTonHay = this.PricePerTonHay;
                this.SelectedWorkDay.PricePerTonCorn = this.PricePerTonCorn;
                this.SelectedWorkDay.PricePerTonMinerals = this.PricePerTonMinerals;
                this.SelectedWorkDay.PricePerHeadYardage = this.PricePerHeadYardage;
                this.SelectedWorkDay.Printed = null;

                this._DB.WorkDay.InsertOnSubmit(this.SelectedWorkDay);
                this._DB.SubmitChanges();
            }
            else
            {
                chargeKey = this.SelectedMiscCharge.MiscChargeKey;

                this.SelectedMiscCharge.CustomerID = this.SelectedCustomer.CustomerID;
                this.SelectedMiscCharge.Date = this.SelectedDate;
                this.SelectedMiscCharge.Printed = null;

                this._DB.MiscCharge.InsertOnSubmit(this.SelectedMiscCharge);
                this._DB.SubmitChanges();
            }

            this.SelectedCustomer_Changed(null, null);

            this.ChargesGrid.SelectedItem = this.Charges.Where(o =>
                (addingWorkDay ? o.Type == ChargeType.WorkDay : o.Type == ChargeType.Misc)
                    && o.Key == chargeKey);
            this.ChargesGrid.Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WorkDayTab.IsEnabled)
            {
                this.SelectedWorkDay.Date = this.SelectedDate;
                this.SelectedWorkDay.PricePerTonDistillers = this.PricePerTonDistillers;
                this.SelectedWorkDay.PricePerTonHay = this.PricePerTonHay;
                this.SelectedWorkDay.PricePerTonCorn = this.PricePerTonCorn;
                this.SelectedWorkDay.PricePerTonMinerals = this.PricePerTonMinerals;
                this.SelectedWorkDay.PricePerHeadYardage = this.PricePerHeadYardage;

                WorkDay updatedDay = this._DB.WorkDay.Where(o => o.WorkDayKey == this.SelectedWorkDay.WorkDayKey).FirstOrDefault();
                Charge.Clone(this.SelectedWorkDay, updatedDay);

                this._DB.SubmitChanges();

                this.ChargesGrid.SelectedItem = this.Charges.Where(o => o.Type == ChargeType.WorkDay && o.Key == this._OldChargeKey).FirstOrDefault();
            }
            else
            {
                MiscCharge updatedMisc = this._DB.MiscCharge.Where(o => o.MiscChargeKey == this.SelectedMiscCharge.MiscChargeKey).FirstOrDefault();
                Charge.Clone(this.SelectedMiscCharge, updatedMisc);

                this._DB.SubmitChanges();

                this.ChargesGrid.SelectedItem = this.Charges.Where(o => o.Type == ChargeType.Misc && o.Key == this._OldChargeKey).FirstOrDefault();
            }

            this.EditMode = false;
            this.ChargesGrid.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WorkDayTab.IsEnabled)
            {
                this.SelectedWorkDay = this._DB.WorkDay.Where(o => o.WorkDayKey == this._OldChargeKey).FirstOrDefault();
                this.SelectedDate = this.SelectedWorkDay.Date;
                this.PricePerTonDistillers = this.SelectedWorkDay.PricePerTonDistillers;
                this.PricePerTonHay = this.SelectedWorkDay.PricePerTonHay;
                this.PricePerTonCorn = this.SelectedWorkDay.PricePerTonCorn;
                this.PricePerTonMinerals = this.SelectedWorkDay.PricePerTonMinerals;
                this.PricePerHeadYardage = this.SelectedWorkDay.PricePerHeadYardage;

                this.ChargesGrid.SelectedItem = this.Charges.Where(o => o.Type == ChargeType.WorkDay && o.Key == this._OldChargeKey).FirstOrDefault();
            }
            else
            {
                this.SelectedMiscCharge = this._DB.MiscCharge.Where(o => o.MiscChargeKey == this._OldChargeKey).FirstOrDefault();
                this.SelectedDate = this.SelectedMiscCharge.Date;

                this.ChargesGrid.SelectedItem = this.Charges.Where(o => o.Type == ChargeType.Misc && o.Key == this._OldChargeKey).FirstOrDefault();
            }

            this.EditMode = false;
            this.ChargesGrid.Focus();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCustomer == null)
            {
                MessageBox.Show("No customer selected. Please select one before printing receipt.");
                return;
            }

            if (this.Charges.Count(o => o.Include) == 0)
            {
                MessageBox.Show("No charges have been selected to print. Select charges using the 'Include' column to print");
                return;
            }

            string receipt = PossPricingApp.Properties.Resources.Receipt;

            receipt = receipt.Replace("{Customer}", this.SelectedCustomer.Name);
            receipt = receipt.Replace("{Date}", DateTime.Today.ToString("M/d/yyyy (dddd)"));
            receipt = receipt.Replace("{Total}", Charge.GetTotalReceiptLine(this.Charges.ToList()));

            string workPeriod = this.Charges.Where(o => o.Include).LastOrDefault().Date.ToString("M/d/yyyy")
                + " to " + this.Charges.Where(o => o.Include).FirstOrDefault().Date.ToString("M/d/yyyy");
            receipt = receipt.Replace("{Work Period}", workPeriod);

            string receiptInfoLines = "";

            bool hasMiscCharges = (this.Charges.Where(o => o.Include).Count(o => o.Type == ChargeType.Misc) > 0);

            if (this.Charges.Where(o => o.Include).Count(o => o.Type == ChargeType.WorkDay) > 0)
            {
                List<int> workDayKeys = this.Charges.Where(o => o.Type == ChargeType.WorkDay && o.Include).Select(o => o.Key).ToList();
                List<WorkDay> customerWorkDays = this._DB.WorkDay.Where(o => o.CustomerID == this.SelectedCustomer.CustomerID && o.Printed == null).ToList();

                receiptInfoLines += Charge.GetWorkDayLines(customerWorkDays.Where(o => (workDayKeys.IndexOf(o.WorkDayKey) != -1)).ToList());
                if (customerWorkDays.Count(o => o.NumberOfHeadYardage > 0) > 0) { receiptInfoLines += System.Environment.NewLine + System.Environment.NewLine + Charge.GetYardageLine(customerWorkDays); }
                if (hasMiscCharges) { receiptInfoLines += System.Environment.NewLine + System.Environment.NewLine; }

                foreach (WorkDay workDay in customerWorkDays)
                {
                    workDay.Printed = System.DateTime.Today;
                }
                this._DB.SubmitChanges();
            }

            if (hasMiscCharges)
            {
                List<int> miscChargeKeys = this.Charges.Where(o => o.Type == ChargeType.Misc && o.Include).Select(o => o.Key).ToList();
                List<MiscCharge> customerMiscCharges = this._DB.MiscCharge.Where(o => o.CustomerID == this.SelectedCustomer.CustomerID && o.Printed == null).ToList();

                receiptInfoLines += Charge.GetMiscLines(customerMiscCharges.Where(o => (miscChargeKeys.IndexOf(o.MiscChargeKey) != -1)).ToList());

                foreach (MiscCharge miscCharge in customerMiscCharges)
                {
                    miscCharge.Printed = System.DateTime.Today;
                }
                this._DB.SubmitChanges();
            }

            receipt = receipt.Replace("{Receipt Info}", receiptInfoLines);

            string receiptFolder = Path.Combine(PossPricingApp.Properties.Settings.Default.StorageLocation, "Receipts", this.SelectedCustomer.Name);
            if (!Directory.Exists(receiptFolder)) { Directory.CreateDirectory(receiptFolder); }
            string receiptLocation = Path.Combine(receiptFolder, string.Format(@"{0}_{1}.txt", this.SelectedCustomer.Name, DateTime.Now.ToString("M-d-yyyy_HHmmss")));
            File.WriteAllText(receiptLocation, receipt);
            Process.Start(receiptLocation);

            this.SelectedCustomer_Changed(null, null);
        }

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        private void ChargesGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!this.EditMode)
            {
                this.ChargesGrid.SelectedItem = null;
                this.SelectedWorkDay = new WorkDay();
                this.SelectedMiscCharge = new MiscCharge();
            }
        }

        private void ChangeNameCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCustomer == null) { return; }

            JDB.Library.Controls.CustomInputBox win = new JDB.Library.Controls.CustomInputBox();

            string cust = win.Show(this, "Enter name of customer:", "New Customer", this.SelectedCustomer.Name);

            if (win.WasOkClicked && !string.IsNullOrEmpty(cust) && !cust.Equals(this.SelectedCustomer.Name))
            {
                Directory.Move
                (
                    Path.Combine(PossPricingApp.Properties.Settings.Default.StorageLocation, "Receipts", this.SelectedCustomer.Name),
                    Path.Combine(PossPricingApp.Properties.Settings.Default.StorageLocation, "Receipts", cust)
                );

                Customer customer = this._DB.Customer.Where(o => o.CustomerID == this.SelectedCustomer.CustomerID).FirstOrDefault();
                customer.Name = cust;
                this._DB.SubmitChanges();

                this.RefreshCustomerList();

                this.SelectedCustomer = this.Customers.Where(o => o.Name == cust).FirstOrDefault();
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCustomer == null) { return; }

            Customer customer = this.SelectedCustomer;

            if (MessageBox.Show("Are you sure you want to delete this customer? This action cannot be undone.", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Customer cust = this._DB.Customer.Where(o => o.CustomerID == customer.CustomerID).First();
                this._DB.Customer.DeleteOnSubmit(cust);
                this._DB.SubmitChanges();

                this.RefreshCustomerList();
            }
        }

        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Charges == null) { return; }

            foreach (Charge c in this.Charges)
            {
                c.Include = true;
            }
        }

        private void NoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Charges == null) { return; }

            foreach (Charge c in this.Charges)
            {
                c.Include = false;
            }
        }
        private void BetweenButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Charges == null) { return; }

            DateTime date1 = BetweenDatePicker1.SelectedDate ?? DateTime.Today;
            DateTime date2 = BetweenDatePicker2.SelectedDate ?? DateTime.Today;

            DateTime afterDate, beforeDate;
            if (DateTime.Compare(date1, date2) < 0)
            {
                afterDate = date1;
                beforeDate = date2;
            }
            else
            {
                afterDate = date2;
                beforeDate = date1;
            }

            foreach (Charge c in this.Charges)
            {
                if (c.Date >= afterDate && c.Date <= beforeDate)
                {
                    c.Include = true;
                }
                else
                {
                    c.Include = false;
                }
            }
        }

        private void DuplicateButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedCustomer == null)
            {
                MessageBox.Show("No customer selected. Please select one before adding charge information.");
                return;
            }

            WorkDay previousWorkDay = this._DB.WorkDay.Where(o => o.CustomerID == this.SelectedCustomer.CustomerID).OrderByDescending(o => o.Date).First();
            if (previousWorkDay == null) { previousWorkDay = new WorkDay(); }


            this.WorkDayTab.IsSelected = true;
            this.SelectedWorkDay = Charge.Clone(previousWorkDay);
        }
    }
}