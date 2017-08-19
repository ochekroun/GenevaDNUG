namespace U2UC.WinLob.App
{
    using System;
using Windows.UI.Xaml.Media;


    public class Employee
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public DateTime? BirthDate { get; set; }

        public byte[] Picture { get; set; }

        public ImageSource ImageSource
        {
            get { return this.Picture.AsBitmapImage(); }
        }

        public int Age
        {
            get
            {
                if (this.BirthDate == null)
                {
                    return -1;
                }

                DateTime now = DateTime.Now;
                int age = now.Year - this.BirthDate.Value.Year;
                if (this.BirthDate > now.AddYears(-age)) age--;

                return age;
            }
        }
    }
}
