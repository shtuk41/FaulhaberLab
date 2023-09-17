using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaulhaberPlayer
{
    public class ProfilePositionPage : INotifyPropertyChanged
    {
        #region fields

        private int setPoint1 = 0;
        private int setPoint2 = 0;
        private int actualValue = 0;

        #endregion

        #region events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region properties

        public int SetPoint1
        {
            get
            {
                return setPoint1;
            }

            set
            {
                if (value != setPoint1)
                {
                    setPoint1 = value;
                    OnPropertyChanged("SetPoint1");
                }
            }
        }

        public int SetPoint2
        {
            get
            {
                return setPoint2;
            }

            set
            {
                if (value != setPoint2)
                {
                    setPoint2 = value;
                    OnPropertyChanged("SetPoint2");
                }
            }
        }

        public int ActualValue
        {
            get
            {
                return actualValue;
            }

            set
            {
                if (value != actualValue)
                {
                    actualValue = value;
                    OnPropertyChanged("ActualValue");
                }
            }
        }

        #endregion


        #region PropertyChanged

        /// <summary>
        /// Called when a property is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
