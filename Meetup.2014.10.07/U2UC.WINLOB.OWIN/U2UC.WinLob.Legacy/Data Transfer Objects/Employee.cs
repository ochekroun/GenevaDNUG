namespace U2UC.WinLob.Legacy.Dto
{
    using System;

    public struct Employee
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public DateTime? BirthDate { get; set; }

        public byte[] Picture { get; set; }
    }
}
