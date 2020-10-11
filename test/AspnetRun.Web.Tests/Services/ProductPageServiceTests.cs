using AspnetRun.Application.Interfaces;
using AspnetRun.Application.Models;
using AspnetRun.Web.Services;
using AspnetRun.Web.ViewModels;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AspnetRun.Web.Tests.Services
{
    public class ProductPageServiceTests
    {
        [Fact]
        public async void GetProducts_ReadDataFromDataSourceIfNameWhiteSpace_ReadData()
        {
            var productAppService = new Mock<IProductService>();
            var categoryAppService = new Mock<ICategoryService>();
            var mapper = new Mock<IMapper>();
            var logger = new Mock<ILogger<ProductPageService>>();
            const string productName = "";

            var productPageService = new ProductPageService(productAppService.Object, categoryAppService.Object, mapper.Object, logger.Object);
            _ = await productPageService.GetProducts(productName);
            productAppService
                .Verify(x => x.GetProductList(), Times.Once);

        }
        [Fact]
        public async void GetProducts_ReadDataFromDataSourceIfNameNull_ReadData()
        {
            var productAppService = new Mock<IProductService>();
            var categoryAppService = new Mock<ICategoryService>();
            var mapper = new Mock<IMapper>();
            var logger = new Mock<ILogger<ProductPageService>>();
            const string productName = null;

            var productPageService = new ProductPageService(productAppService.Object, categoryAppService.Object, mapper.Object, logger.Object);
            _ = await productPageService.GetProducts(productName);
            productAppService
                .Verify(x => x.GetProductList(), Times.Once);

        }
        [Fact]
        public async void GetProducts_MappedDataFromDataSourceIfNameNull_MapData()
        {
            var productAppService = new Mock<IProductService>();
            var products = new[]
            {
                new ProductModel()
            };
            productAppService
                .Setup(x => x.GetProductList())
                .ReturnsAsync(products);


            var categoryAppService = new Mock<ICategoryService>();
            var mapper = new Mock<IMapper>();
            var productsViewModels = new[]
            {
                new ProductViewModel()
            };
            mapper
                .Setup(x => x.Map<IEnumerable<ProductViewModel>>(products))
                .Returns(productsViewModels);

            var logger = new Mock<ILogger<ProductPageService>>();
            const string productName = null;

            var productPageService = new ProductPageService(productAppService.Object, categoryAppService.Object, mapper.Object, logger.Object);
            _ = await productPageService.GetProducts(productName);
            mapper
                .Verify(x => x.Map<IEnumerable<ProductViewModel>>(products), Times.Once);

        }
        [Fact]
        public async void GetProducts_ReturnDataIfNameNull_MappedData()
        {
            var productAppService = new Mock<IProductService>();
            var products = new[]
            {
                new ProductModel()
            };
            productAppService
                .Setup(x => x.GetProductList())
                .ReturnsAsync(products);


            var categoryAppService = new Mock<ICategoryService>();
            var mapper = new Mock<IMapper>();
            var productsViewModels = new[]
            {
                new ProductViewModel()
            };
            mapper
                .Setup(x => x.Map<IEnumerable<ProductViewModel>>(products))
                .Returns(productsViewModels);

            var logger = new Mock<ILogger<ProductPageService>>();
            const string productName = null;

            var productPageService = new ProductPageService(productAppService.Object, categoryAppService.Object, mapper.Object, logger.Object);
            var result = await productPageService.GetProducts(productName);
            result.Should().BeSameAs(productsViewModels);
        }

        [Fact]
        public async void GetProducts_ReturnDataByName_MappedData()
        {
            const string productName = "Apple";
            //var productsArray = new[]
            //{
            //    new ProductModel()
            //    {
            //    ProductName = "Apple"
            //    }
            //};
            //var productAppService = new Mock<IProductService>();

            //var product1 = new ProductModel()
            //{
            //    ProductName = "Apple"
            //};
            //var product2 = new ProductModel()
            //    {
            //    ProductName = "Samsung"
            //    };
            //await productAppService.Object.Create(product1);
            //await productAppService.Object.Create(product2);

            //var foundProduct = await productAppService.Object.GetProductByName(productName);
            //foundProduct.Should().BeSameAs(productsArray);

            var productAppService = new Mock<IProductService>();
            var products = new[]
            {
                new ProductModel(){ ProductName = "Apple"}
            };
            productAppService
                .Setup(x => x.GetProductByName(productName))
                .ReturnsAsync(products);

            var categoryAppService = new Mock<ICategoryService>();
            var mapper = new Mock<IMapper>();
            var productsViewModels = new[]
            {
                new ProductViewModel(){ ProductName = "Apple"}
            };
            mapper
                .Setup(x => x.Map<IEnumerable<ProductViewModel>>(products))
                .Returns(productsViewModels);

            var logger = new Mock<ILogger<ProductPageService>>();

            var productPageService = new ProductPageService(productAppService.Object, categoryAppService.Object, mapper.Object, logger.Object);
            var result = await productPageService.GetProducts(productName);
            result.Should().BeSameAs(productsViewModels);
        }
    }
}
