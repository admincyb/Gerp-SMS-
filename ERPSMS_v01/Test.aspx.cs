using Telerik.Web.UI;
using System;
using System.Collections;
using System.Web;
using Telerik.Web.UI.PersistenceFramework;
//using Model.ReadWrite.Telerik;
using System.Linq;
using System.Collections.Generic;

namespace ERPSMS_v01
{
    
    public partial class Test : System.Web.UI.Page
    {

        protected string NormalizeValue(object inputValue) 
        {
            return inputValue.ToString().Replace(" ", "");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // RadPersistenceManager1.StorageProvider = new SessionStorageProvider(RadPersistenceManager1.StorageProviderKey);

            //GridFilterMenu filterMenu = RadGrid1.FilterMenu;

            //int currentItemIndex = 0;
            //while (currentItemIndex < filterMenu.Items.Count)
            //{
            //    RadMenuItem item = filterMenu.Items[currentItemIndex];
            //    if (item.Text.Contains("Empty") || item.Text.Contains("Null"))
            //    {
            //        filterMenu.Items.Remove(item);
            //    }
            //    else currentItemIndex++;
            //}
        }

        //protected string GetCarImageUrl(object container)
        //{
        //    Hashtable values = new Hashtable();
        //    (container as GridNestedViewItem).ParentItem.ExtractValues(values);
        //    return String.Format("~/Grid/Examples/Overview/Images/Cars/{0}_{1}.png", NormalizeValue(values["BrandName"]), NormalizeValue(values["Model"]));
        //}

        //protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
        //{
        //    if (e.CommandName == "RowClick" || e.CommandName == "ExpandCollapse")
        //    {
        //        bool lastState = e.Item.Expanded;

        //        if (e.CommandName == "ExpandCollapse")
        //        {
        //            lastState = !lastState;
        //        }

        //        CollapseAllRows();
        //        e.Item.Expanded = !lastState;
        //    }
        //    if (e.CommandName == "UpdateCount")
        //    {
        //        UpdateCarRentCounter(e.CommandArgument.ToString());
        //    }
        //}

        //private void CollapseAllRows()
        //{
        //    foreach (GridItem item in RadGrid1.MasterTableView.Items)
        //    {
        //        item.Expanded = false;
        //    }
        //}

        //protected void BrandNameCombo_DataBound(object sender, EventArgs e)
        //{
        //    RadComboBox combo = sender as RadComboBox;
        //    foreach (RadComboBoxItem item in combo.Items)
        //    {
        //        item.ImageUrl = String.Format("~/Grid/Examples/Overview/Images/SmallLogos/{0}.png", NormalizeValue(item.Text));
        //    }
        //}

        //protected void SaveSettingsButton_Click(object sender, EventArgs e)
        //{
        //    RadPersistenceManager1.SaveState();
        //}

        //protected void LoadSettingsButton_Click(object sender, EventArgs e)
        //{
        //    RadPersistenceManager1.LoadState();
        //    RadGrid1.Rebind();
        //}

        private void UpdateCarRentCounter(string carID)
        {
            //int carIdValue;
            //if (!Int32.TryParse(carID, out carIdValue))
            //{
            //    return;
            //}

            //Car targetCar = EFContext.Cars
            //    .Where(car => car.CarID == carIdValue)
            //    .Single();
            //if (targetCar != null)
            //{
            //    targetCar.RentedCount++;
            //}

            //EFContext.SaveChanges();

        //    RadGrid1.Rebind();
        }

        //private TelerikReadWriteEntities _dataContext = null;
        //private TelerikReadWriteEntities EFContext
        //{
        //    get
        //    {
        //        if (_dataContext == null)
        //            _dataContext = new TelerikReadWriteEntities();
        //        return _dataContext;
        //    }
        //}

        //protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        //{
        //  //  RadGrid1.DataSource = EFContext.Cars;
        //}

        protected void Page_Unload(object sender, EventArgs e)
        {
            //if (EFContext != null)
            //{
            //    EFContext.Dispose();
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // NumberToWordConvertion x = new NumberToWordConvertion();
            //string r= x.ConvertNumberToWords("12.12");

            //((HiddenField)Master.FindControl("hdfCurrencyGroup1")).Value = "2";
            //((HiddenField)Master.FindControl("hdfCurrencyGroup2")).Value = "3";

            //ucAmountControl1.OnTextChanged += new EventHandler(TextChanged);
            //ucAmountControl1.Text = "745673465786.9328478";
            //string str = ucAmountControl1.Text;
            //  BindRadGrid();
            Geek g = new Geek();
            g.M1();
            g.M2();
            g.M3();
            g.M4();
            g.M5("Method Name: M5");
        }
        protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            (sender as RadGrid).DataSource = MyData;
        }

        public IEnumerable<SampleData> MyData = Enumerable.Range(1, 30).Select(x => new SampleData
        {
            CarID = x,
            Price=x,
            BrandName = "BrandName " + x,
            Classification = "Classification " + x ,
            Transmission = "Transmission " + x,
            Fuel = "Fuel " + x,
            Model = "Model " + x,
            RentedCount = "RentedCount " + x,
            Year = DateTime.Now.AddDays(-x * 3).Date
        });

        public class SampleData
        {
            public int CarID { get; set; }
            public float Price { get; set; }
            public string BrandName { get; set; }
            public string Classification { get; set; }
            public string Transmission { get; set; }
            public string Fuel { get; set; }
            public string Model { get; set; }
            public string RentedCount { get; set; }

            public DateTime Year { get; set; }
        }


        private void BindRadGrid()
        {
        }
        protected void TextChanged(object sender, EventArgs e)
        {
            int a = 0;


        }
     
     
    }
    static class NewMethodClass
    {

        // Method 4
        public static void M4(this Geek g)
        {
            Console.WriteLine("Method Name: M4");
        }

        // Method 5
        public static void M5(this Geek g, string str)
        {
            Console.WriteLine(str);
        }
    }
    class Geek
    {

        // Method 1
        public void M1()
        {
            Console.WriteLine("Method Name: M1");
        }

        // Method 2
        public void M2()
        {
            Console.WriteLine("Method Name: M2");
        }

        // Method 3
        public void M3()
        {
            Console.WriteLine("Method Name: M3");
        }

    }
}