using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DrinkChocolate
{
    class PersistantData
    {
        public int MaxDrinks { get; set; }

        [XmlArray(ElementName ="DrinkList")]
        public List<DateTime> DrinkList { get; set; }

        // create temp undo and redo list.
        // should be persisted as long as the program is running
        private List<DateTime> TempItemList { get; set; }

        public PersistantData()
        {
            DrinkList = new List<DateTime>();
            TempItemList = new List<DateTime>();
        }

        private DateTime GetLastDrinkTime()
        {
            var date = DrinkList.Count == 0? DateTime.MinValue : DrinkList.Last();

            return date;
        }

        public string GetTimneSinceLastDrink()
        {
            var lastDrinkDate = GetLastDrinkTime();
            if(lastDrinkDate == DateTime.MinValue)
            {
                return "For evigt";
            }
            var time = DateTime.Now.Subtract(lastDrinkDate);
            return time.ToString(@"hh\:mm\:ss");
        }

        public void ClearList()
        {
            DrinkList = new List<DateTime>();
            // save the data, to clear persisted data.
            SaveData();
        }

        public void AddDrink()
        {
            DrinkList.Add(DateTime.Now);
            // Auto save data for persistency
            SaveData();
        }

        public void SaveData()
        {
            // serialize data and save it.
            XmlSerializer aSerializer = new XmlSerializer(typeof(List<DateTime>));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            aSerializer.Serialize(sw, DrinkList);
            string xmlResult = sw.GetStringBuilder().ToString();

            // write the xml to a file
            File.WriteAllText(AppContext.BaseDirectory + @"\saveData.txt", xmlResult,Encoding.UTF8);
        }

        public void UndoLastAdd()
        {
            // check size 
            if(DrinkList.Count < 1)
            {
                // Nothing to undo
                return;
            }
            var lastDateTime = DrinkList.Last();
            TempItemList.Add(lastDateTime);
            DrinkList.RemoveAt(DrinkList.Count - 1);
        }

        public void RedoLastAdd()
        {
            // check temp size
            if(TempItemList.Count < 1)
            {
                // no action to redo
                return;
            }
            var redoAdd = TempItemList.Last();
            DrinkList.Add(redoAdd);
            TempItemList.RemoveAt(TempItemList.Count - 1);
        }

        public void LoadData()
        {
            // Create an instance of the XmlSerializer.
            XmlSerializer serializer =
            new XmlSerializer(typeof(List<DateTime>));

            // Declare an object variable of the type to be deserialized.

            using (StreamReader reader = new StreamReader(AppContext.BaseDirectory + @"\saveData.txt",Encoding.UTF8))
            {
                // Call the Deserialize method to restore the object's state.
                DrinkList = (List<DateTime>)serializer.Deserialize(reader);
            }
        }
    }
}
