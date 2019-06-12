using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace AsyncBindingTest
{
    /// <summary>
    /// A helper class for creating a binding on an object that may be changed asynchronously from the bound UI thread.
    /// </summary>
    public class AsyncBindingHelper : INotifyPropertyChanged
    {
        private Control bindingControl;
        private INotifyPropertyChanged bindingSource;
        private string dataMember;

        private AsyncBindingHelper(Control bindingControl, INotifyPropertyChanged bindingSource, string dataMember)
        {
            this.bindingControl = bindingControl;
            this.bindingSource = bindingSource;
            this.dataMember = dataMember;
            bindingSource.PropertyChanged += new PropertyChangedEventHandler(bindingSource_PropertyChanged);
        }

        public static Binding AddBinding(Control bindingControl, string propertyName, INotifyPropertyChanged bindingSource, string dataMember)
        {
            AsyncBindingHelper helper = new AsyncBindingHelper(bindingControl, bindingSource, dataMember);
            return bindingControl.DataBindings.Add(propertyName, helper, "Value");
        }

        private void bindingSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (PropertyChanged != null && e.PropertyName == dataMember)
                {
                    if (bindingControl.InvokeRequired)
                    {
                        bindingControl.BeginInvoke(
                          new PropertyChangedEventHandler(bindingSource_PropertyChanged),
                          sender,
                          e);
                        return;
                    }
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }
            catch 
            { 
                //try-catch防止线程访问已经关闭的控件资源.
            }
        }

        /// <summary>
        /// The current value of the data sources' datamember
        /// </summary>
        public object Value
        {
            get
            {
                return bindingSource.GetType().GetProperty(dataMember).GetValue(bindingSource, null);
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Event fired when the dataMember property on the data source is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members
    }
}
