using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncBindingTest
{
    public class DataSource : INotifyPropertyChanged
    {
        private string value;

        public string Value
        {
            get { return this.value; }
            set
            {
                this.value = value;

                if (PropertyChanged != null)
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
