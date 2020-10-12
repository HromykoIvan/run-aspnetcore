using AspnetRun.Application.Interfaces;
using AspnetRun.Application.Models;
using AspnetRun.Core.Entities;
using AspnetRun.Web.Services;
using AspnetRun.Web.ViewModels;
using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AspnetRun.Web.Tests.Services
{
    public class CategoryServiceTests
    {
        [Fact]
        public void Constructor_categoryAppServiceIsNull_ArgumentNullExceptionExpected()
        {
            //arrage
            var mapper = new Mock<IMapper>();

            //act
            Func<CategoryPageService> func = () => new CategoryPageService(null, mapper.Object);

            //assert
            using (new AssertionScope())
            {
                func.Should().ThrowExactly<ArgumentNullException>()
                    .And
                    .ParamName.Should().Be("categoryAppService");
            }
        }

        [Fact]
        public void Constructor_mappperIsNull_ArgumentNullExceptionExpected()
        {
            //arrage
            var categoryAppService = new Mock<ICategoryService>();

            //act
            Func<CategoryPageService> func = () => new CategoryPageService(categoryAppService.Object, null);

            //assert
            using (new AssertionScope())
            {
                func.Should().ThrowExactly<ArgumentNullException>()
                    .And
                    .ParamName.Should().Be("mapper");
            }
        }
        [Fact]
        public async void GetCategories_ReadDataFromDataSource_DataRead()
        {
            var categoryAppService = new Mock<ICategoryService>();
            var categoriesApp = new[]
            {
                new CategoryModel()
            };
            categoryAppService
                .Setup(x => x.GetCategoryList())
                .ReturnsAsync(categoriesApp);

            var mapper = new Mock<IMapper>();
            var categoryViewModels = new[]
            {
                new CategoryViewModel(){ CategoryName = "MyTestCategory"}
            };
            mapper
                .Setup(x => x.Map<IEnumerable<CategoryViewModel>>(It.Is<CategoryViewModel[]>(x => x.Length == 1 && x[0].CategoryName == "MyTestCategory")))
                .Returns(categoryViewModels);

            //mapper
            //.Setup(x => x.Map<IEnumerable<CategoryViewModel>>(It.IsAny<CategoryViewModel[]>()))
            //.Returns(categoryViewModels);
            var target = new CategoryPageService(categoryAppService.Object, mapper.Object);
            _ = await target.GetCategories();

            categoryAppService
                .Verify(x => x.GetCategoryList(), Times.Once);
            //mapper.Verify(x => x.Map<IEnumerable<CategoryViewModel>>(It.Is<CategoryViewModel[]>(x => x.Length == 1 && x[0].CategoryName == "MyTestCategory")), Times.Once);


        }

        [Fact]
        public async void GetCategories_MapModelFromData_DataMap()
        {
            var categoryAppService = new Mock<ICategoryService>();
            var categoriesApp = new[]
            {
                new CategoryModel()
            };
            categoryAppService
                .Setup(x => x.GetCategoryList())
                .ReturnsAsync(categoriesApp);

            var mapper = new Mock<IMapper>();
            var categoryViewModels = new[]
            {
                new CategoryViewModel()
            };
            mapper
                .Setup(x => x.Map<IEnumerable<CategoryViewModel>>(categoriesApp))
                .Returns(categoryViewModels);
            var target = new CategoryPageService(categoryAppService.Object, mapper.Object);
            _ = await target.GetCategories();

            mapper
                .Verify(x => x.Map<IEnumerable<CategoryViewModel>>(categoriesApp), Times.Once);


        }
        [Fact]
        public async void GetCategories_Return_MappedDataFromDataSource()
        {
            var categoryAppService = new Mock<ICategoryService>();
            var categoriesApp = new[]
            {
                new CategoryModel()
            };
            categoryAppService
                .Setup(x => x.GetCategoryList())
                .ReturnsAsync(categoriesApp);

            var mapper = new Mock<IMapper>();
            var categoryViewModels = new[]
            {
                new CategoryViewModel()
            };
            mapper
                .Setup(x => x.Map<IEnumerable<CategoryViewModel>>(categoriesApp))
                .Returns(categoryViewModels);

            var target = new CategoryPageService(categoryAppService.Object, mapper.Object);
            var result =await target.GetCategories();

            result.Should().BeSameAs(categoryViewModels);


        }

    }
}
