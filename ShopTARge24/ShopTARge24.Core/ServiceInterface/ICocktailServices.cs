using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShopTARge24.Core.Dto.CocktailDtos;


namespace ShopTARge24.Core.ServiceInterface
{
    public interface ICocktailServices
    {
        Task<CocktailRootDto> GetCocktails(CocktailResultDto dto);
    }
}