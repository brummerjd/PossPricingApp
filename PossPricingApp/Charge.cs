using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace PossPricingApp
{
    public class Charge : INotifyPropertyChanged
    {
        private const int RECEIPT_WIDTH = 60;

        public int Key;
        public ChargeType Type;

        public bool Include
        {
            get { return this._Include; }
            set { this._Include = value; OnPropertyChanged("Include"); }
        }
        private bool _Include;

        public DateTime Date
        {
            get { return this._Date; }
            set { this._Date = value; OnPropertyChanged("Date"); }
        }
        private DateTime _Date;

        public String Description
        {
            get { return this._Description; }
            set { this._Description = value; OnPropertyChanged("Description"); }
        }
        private String _Description;
        
        public double Total
        {
            get { return this._Total; }
            set { this._Total = value; OnPropertyChanged("Total"); }
        }
        private double _Total;

        public Charge(Charge oldCharge)
        {
            this.Key = oldCharge.Key;
            this.Type = oldCharge.Type;
            this.Include = oldCharge.Include;
            this.Date = oldCharge.Date;
            this.Description = oldCharge.Description;
            this.Total = oldCharge.Total;
        }

        public Charge(WorkDay day)
        {
            this.Key = day.WorkDayKey;
            this.Type = ChargeType.WorkDay;
            this.Include = day.Printed == null;
            this.Date = day.Date;
            this.Description = "Materials/yardage charges";
            this.Total = Math.Round((day.PoundsDistillers * day.PricePerTonDistillers
                                        + day.PoundsCorn * day.PricePerTonCorn
                                        + day.PoundsHay * day.PricePerTonHay
                                        + day.PoundsMinerals * day.PricePerTonMinerals) / 2000
                                        + day.NumberOfHeadYardage * day.PricePerHeadYardage, 2);
        }

        public Charge(MiscCharge misc)
        {
            this.Key = misc.MiscChargeKey;
            this.Type = ChargeType.Misc;
            this.Include = misc.Printed == null;
            this.Date = misc.Date;
            this.Description = misc.Description;
            this.Total = misc.Amount;
        }

        public static WorkDay Clone(WorkDay day)
        {
            WorkDay newDay = new WorkDay();

            return Clone(day, newDay);
        }

        public static WorkDay Clone(WorkDay oldDay, WorkDay newDay)
        {
            newDay.WorkDayKey = oldDay.WorkDayKey;
            newDay.CustomerID = oldDay.CustomerID;
            newDay.Date = oldDay.Date;
            newDay.PoundsDistillers = oldDay.PoundsDistillers;
            newDay.PricePerTonDistillers = oldDay.PricePerTonDistillers;
            newDay.PoundsHay = oldDay.PoundsHay;
            newDay.PricePerTonHay = oldDay.PricePerTonHay;
            newDay.PoundsCorn = oldDay.PoundsCorn;
            newDay.PricePerTonCorn = oldDay.PricePerTonCorn;
            newDay.PoundsMinerals = oldDay.PoundsMinerals;
            newDay.PricePerTonMinerals = oldDay.PricePerTonMinerals;
            newDay.NumberOfHeadYardage = oldDay.NumberOfHeadYardage;
            newDay.PricePerHeadYardage = oldDay.PricePerHeadYardage;
            newDay.Printed = oldDay.Printed;

            return newDay;
        }

        public static MiscCharge Clone(MiscCharge misc)
        {
            MiscCharge newMisc = new MiscCharge();

            return Clone(misc, newMisc);
        }

        public static MiscCharge Clone(MiscCharge oldMisc, MiscCharge newMisc)
        {
            newMisc.MiscChargeKey = oldMisc.MiscChargeKey;
            newMisc.CustomerID = oldMisc.CustomerID;
            newMisc.Date = oldMisc.Date;
            newMisc.Amount = oldMisc.Amount;
            newMisc.Description = oldMisc.Description;
            newMisc.Printed = oldMisc.Printed;

            return newMisc;
        }

        public static string GetWorkDayLines(List<WorkDay> workDays)
        {
            string workDayLines = "Materials Charges" + System.Environment.NewLine;
            double poundsSum = 0;
            string itemInfo;
            double totalAmount = 0;
            string totalAmountString;

            Action<string> UpdateWorkDayLines = (string type) =>
            {
                totalAmountString = "$" + String.Format("{0:0.00}", totalAmount);

                itemInfo = "   "
                    + poundsSum
                    + " lbs " + type + " at $"
                    + String.Format("{0:0.00}", Math.Round(totalAmount / poundsSum, 2))
                    + " per lb";
                workDayLines += itemInfo;

                for (int i = itemInfo.Length; i < RECEIPT_WIDTH - totalAmountString.Length; i++) { workDayLines += " "; }

                workDayLines += totalAmountString + System.Environment.NewLine;
            };

            poundsSum = workDays.Sum(o => o.PoundsDistillers);
            totalAmount = Math.Round(workDays.Sum(o => o.PricePerTonDistillers * o.PoundsDistillers) / 2000, 2);
            if (totalAmount > 0) { UpdateWorkDayLines("distillers"); }

            poundsSum = workDays.Sum(o => o.PoundsHay);
            totalAmount = Math.Round(workDays.Sum(o => o.PricePerTonHay * o.PoundsHay) / 2000, 2);
            if (totalAmount > 0) { UpdateWorkDayLines("hay"); }

            poundsSum = workDays.Sum(o => o.PoundsCorn);
            totalAmount = Math.Round(workDays.Sum(o => o.PricePerTonCorn * o.PoundsCorn) / 2000, 2);
            if (totalAmount > 0) { UpdateWorkDayLines("corn"); }

            poundsSum = workDays.Sum(o => o.PoundsMinerals);
            totalAmount = Math.Round(workDays.Sum(o => o.PricePerTonMinerals * o.PoundsMinerals) / 2000, 2);
            if (totalAmount > 0) { UpdateWorkDayLines("minerals"); }

            return workDayLines;
        }

        public static string GetYardageLine(List<WorkDay> workDays)
        {
            double numberOfHead = Math.Round(workDays.Average(o => o.NumberOfHeadYardage));
            double totalAmount = workDays.Sum(o => o.NumberOfHeadYardage * o.PricePerHeadYardage);
            string totalAmountString = "$" + String.Format("{0:0.00}", Math.Round(totalAmount, 2));

            string yardageLine =
                numberOfHead
                + " head at $"
                + string.Format("{0:0.00}", Math.Round(totalAmount / numberOfHead / workDays.Count(o => o.NumberOfHeadYardage > 0), 2))
                + " per head per day for yardage";

            for (int i = yardageLine.Length; i < RECEIPT_WIDTH - totalAmountString.Length; i++) { yardageLine += " "; }

            yardageLine += totalAmountString + System.Environment.NewLine;

            return yardageLine;
        }

        public static string GetMiscLines(List<MiscCharge> miscCharges)
        {
            string miscChargeLines = "Miscellaneous Charges" + System.Environment.NewLine;
            string miscInfo;
            int itemLength;

            foreach (MiscCharge misc in miscCharges.OrderByDescending(o => o.Date))
            {
                miscInfo = "   " + misc.Date.ToString("M/d/yy") + " " + misc.Description;
                itemLength = miscInfo.Length;

                if (itemLength > 48)
                {
                    miscInfo = miscInfo.Substring(0, 45) + "...";
                    itemLength = 48;
                }

                string totalAmount = "$" + String.Format("{0:0.00}", Math.Round(misc.Amount, 2));

                for (int i = itemLength; i < RECEIPT_WIDTH - totalAmount.Length; i++) { miscInfo += " "; }

                miscChargeLines += miscInfo + totalAmount + System.Environment.NewLine;
            }

            return miscChargeLines;
        }

        public static string GetTotalReceiptLine(List<Charge> charges)
        {
            string totalAmount = "$" + String.Format("{0:0.00}", charges.Where(o => o.Include).Sum(o => o.Total));
            
            string totalLine = "Total:";
            for (int i = 6; i < RECEIPT_WIDTH - totalAmount.Length; i++) { totalLine += " "; }
            totalLine += totalAmount;

            return totalLine;
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
    }

    public enum ChargeType
    {
        WorkDay,
        Misc
    }
}
