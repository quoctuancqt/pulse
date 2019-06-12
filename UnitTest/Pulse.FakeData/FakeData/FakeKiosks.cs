namespace Pulse.FakeData
{
    using Common.Helpers;
    using Core.Dto.Entity;
    using Domain;
    using FizzWare.NBuilder;
    using System;
    using System.Collections.Generic;
    public static class FakeKiosks
    {
        public static IEnumerable<KioskDto> FakeDataKioskDto(int record = 10)
        {
            IEnumerable<KioskDto> result = Builder<KioskDto>.CreateListOfSize(record)
                .All()
                .With(c => c.MachineId = NewGuid())
                .With(c => c.IpAddress = UnitHelper.GetLocalIPAddress())
                .With(c => c.Name = Faker.Name.First())
                .Build();
            return result;
        }

        public static IEnumerable<Kiosk> FakeDataKiosk(int record = 10)
        {
            IEnumerable<Kiosk> result = Builder<Kiosk>.CreateListOfSize(record)
                .All()
                .With(c => c.MachineId = NewGuid())
                .With(c => c.IpAddress = UnitHelper.GetLocalIPAddress())
                .With(c => c.Name = Faker.Name.First())
                .Build();
            return result;
        }

        private static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
