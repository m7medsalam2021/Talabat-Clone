using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Product;

namespace Talabat.Core.Specifications.Product_Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams productSpecs)
            : base(P =>
            (string.IsNullOrEmpty(productSpecs.Search) || P.Name.ToLower().Contains(productSpecs.Search)) &&
            (!productSpecs.BrandId.HasValue || P.ProductBrandId == productSpecs.BrandId) &&
            (!productSpecs.TypeId.HasValue || P.ProductTypeId == productSpecs.TypeId)
                  )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            if (!string.IsNullOrEmpty(productSpecs.Sort))
            {
                switch (productSpecs.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price); 
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }



            ApplyPagination(productSpecs.PageSize * (productSpecs.PageIndex - 1), productSpecs.PageSize);
        }

        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }

        public ProductWithBrandAndTypeSpecifications()
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
