namespace U2UC.WinLob.Legacy
{
    using System.Linq;
    using U2UC.WinLob.Legacy.Database;

    public static class Dal
    {
        public static Dto.Employee[] GetAllEmployees()
        {
            using (NorthwindDataContext northWind = new NorthwindDataContext())
            {
                var query = from e in northWind.Employees
                            select new Dto.Employee()
                            {
                                Name = e.FirstName + " " + e.LastName,
                                Title = e.Title,
                                BirthDate = e.BirthDate,
                                Picture = e.Photo.ToArray()
                            };

                return query.ToArray();
            }
        }
    }
}
