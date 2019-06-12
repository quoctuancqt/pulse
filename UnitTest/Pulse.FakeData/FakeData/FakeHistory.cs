namespace Pulse.FakeData
{
    using Domain;
    using FizzWare.NBuilder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class FakeHistory
    {
        public static IEnumerable<History> FakeDataKiosk(int record = 10)
        {
            IEnumerable<History> result = Builder<History>.CreateListOfSize(record)
                .All()
                .With(c => c.Comment = Faker.Name.First())
                .Build();
            return result;
        }
    }
}
