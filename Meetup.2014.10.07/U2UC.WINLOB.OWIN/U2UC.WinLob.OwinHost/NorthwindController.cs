namespace U2UC.WinLob.OwinHost
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    public class NorthwindController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            List<Employee> result = new List<Employee>();

            foreach (var emp in U2UC.WinLob.Legacy.Dal.GetAllEmployees())
            {
                result.Add(
                    new Employee() 
                        {
                            Name = emp.Name,
                            Title = emp.Title,
                            BirthDate = emp.BirthDate,
                            Picture = Convert.ToBase64String(emp.Picture)
                        }
                    );
            }

            return result;
        }
    }
}
