namespace Pulse.FakeData.FakeData
{
    using Common.ResolverFactories;
    using Core.Dto.Entity;
    using System;
    using System.Collections.Generic;

    public class FakeDataDto
    {
        public IDictionary<string, IDictionary<DtoRetriveMethods, Func<object>>> FakeDataMethodDic;

        public FakeDataDto()
        {
            FakeDataMethodDic = new Dictionary<string, IDictionary<DtoRetriveMethods, Func<object>>>();
            FakeDataMethodDic.Add(typeof(GroupDto).FullName, InitCreateDtoMethods<GroupDto, FakeGroups>());
            FakeDataMethodDic.Add(typeof(CountryDto).FullName, InitCreateDtoMethods<CountryDto, FakeCountries>());
        }

        private Dictionary<DtoRetriveMethods, Func<object>> InitCreateDtoMethods<TDto, TDtoFaker>()
            where TDtoFaker : class, IDtoFaker<TDto>
            where TDto : class, IDto
        {
            TDtoFaker dtoFaker = ResolverFactory.CreateInstance<TDtoFaker>(typeof(TDtoFaker).AssemblyQualifiedName);
            var methodMap = new Dictionary<DtoRetriveMethods, Func<object>>();
            methodMap.Add(DtoRetriveMethods.FakeOne, () => dtoFaker.CreateDto());
            methodMap.Add(DtoRetriveMethods.FakeInvalidOne, () => dtoFaker.CreateInvalidDto());

            return methodMap;
        }
    }

    public enum DtoRetriveMethods
    {
        FakeOne,
        FakeInvalidOne
    }
}
