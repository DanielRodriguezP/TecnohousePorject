namespace Sanofi.DT.Athena
{
    using System;
    using System.Collections.Generic;
    public class DTAthena
    {
        public int Item { get; set; }
        public string Document { get; set; }
        public DateTime Date { get; set; }
        public string Material { get; set; }
        public string ShortText { get; set; }
        public decimal OrderQty { get; set; }
        public decimal DeliveryQty { get; set; }
        public string OrderUnit { get; set; }
        public decimal NetPrice { get; set; }
        public decimal NetOrderValue { get; set; }
        public string Vendor { get; set; }
        public string Currency { get; set; }
        public string ReleaseState { get; set; }
        public bool Delete { get; set; }
        public string POHistory { get; set; }
        public int Fila { get; set; }
        public int Recent { get; set; }
    }
    

}