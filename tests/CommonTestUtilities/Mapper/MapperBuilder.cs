using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyRecipeBook.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper
{
    public class MapperBuilder
    {
        public static IMapper Build()
        {
            return new AutoMapper.MapperConfiguration(options => options.AddProfile(new AutoMapping())).CreateMapper();
        }
    }
}
