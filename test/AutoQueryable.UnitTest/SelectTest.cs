﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoQueryable.Extensions;
using AutoQueryable.UnitTest.Mock.Dtos;
using AutoQueryable.Core.Models;
using FluentAssertions;
using Xunit;

namespace AutoQueryable.UnitTest
{
    public class SelectTest
    {
        [Fact]
        public void SelectAllProducts()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("") as IQueryable<object>;
                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithSelectProjection()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=name,productcategory.name") as IQueryable<object>;

                var properties = query.First().GetType().GetProperties();
                properties.Length.Should().Be(2);

                properties.Should().Contain(p => p.Name == "name");
                properties.Should().Contain(p => p.Name == "productcategory");

                var productcategoryProperty = properties.FirstOrDefault(p => p.Name == "productcategory");
                productcategoryProperty.PropertyType.GetProperties().Should().Contain(p => p.Name == "name");



                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithSelectProjectionWithStarSelection()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query =
                    context.Product.AutoQueryable("select=name,productcategory.*,productcategory.name") as
                        IQueryable<object>;

                var properties = query.First().GetType().GetProperties();
                properties.Length.Should().Be(2);

                properties.Should().Contain(p => p.Name == "name");
                properties.Should().Contain(p => p.Name == "productcategory");

                var productcategoryProperty = properties.FirstOrDefault(p => p.Name == "productcategory");
                productcategoryProperty.PropertyType.GetProperties().Should().Contain(p => p.Name == "Name");
                productcategoryProperty.PropertyType.GetProperties().Should()
                    .Contain(p => p.Name == "ProductCategoryId");
                productcategoryProperty.PropertyType.GetProperties().Should().Contain(p => p.Name == "Rowguid");
                productcategoryProperty.PropertyType.GetProperties().Should().Contain(p => p.Name == "ModifiedDate");
                productcategoryProperty.PropertyType.GetProperties().Should()
                    .Contain(p => p.Name == "ParentProductCategoryId");
                productcategoryProperty.PropertyType.GetProperties().Should().Contain(p => p.Name == "Product");
                productcategoryProperty.PropertyType.GetProperties().Should()
                    .Contain(p => p.Name == "ParentProductCategory");
                productcategoryProperty.PropertyType.GetProperties().Should()
                    .Contain(p => p.Name == "InverseParentProductCategory");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }


        [Fact]
        public void SelectAllProductsWithSelectProjection0()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable(
                        "select=ProductCategory.Product.name,ProductCategory.Product.name,ProductCategory.Product.ProductId,ProductCategory.name")
                    as IQueryable<object>;

                var properties = query.First().GetType().GetProperties();
                properties.Length.Should().Be(1);
                properties.Should().Contain(p => p.Name == "ProductCategory");
                var productCategoryProperties = properties.FirstOrDefault(p => p.Name == "ProductCategory").PropertyType
                    .GetProperties();

                productCategoryProperties.Should().Contain(x => x.Name == "name");
                productCategoryProperties.Should().Contain(x => x.Name == "Product");
                var productProperties = productCategoryProperties.FirstOrDefault(p => p.Name == "Product").PropertyType
                    .GenericTypeArguments[0].GetProperties();

                productProperties.Should().Contain(x => x.Name == "name");
                productProperties.Should().Contain(x => x.Name == "ProductId");
                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithSelectProjection1()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query =
                    context.Product.AutoQueryable("select=name,productcategory,productcategory.name") as
                        IQueryable<object>;

                var properties = query.First().GetType().GetProperties();
                properties.Length.Should().Be(2);

                properties.Should().Contain(p => p.Name == "name");
                properties.Should().Contain(p => p.Name == "productcategory");

                var productcategoryProperty = properties.FirstOrDefault(p => p.Name == "productcategory");
                productcategoryProperty.PropertyType.GetProperties().Should().Contain(x => x.Name == "Name");
                productcategoryProperty.PropertyType.GetProperties().Should()
                    .Contain(x => x.Name == "ProductCategoryId");
                productcategoryProperty.PropertyType.GetProperties().Should().Contain(x => x.Name == "Rowguid");
                productcategoryProperty.PropertyType.GetProperties().Should().Contain(x => x.Name == "ModifiedDate");
                productcategoryProperty.PropertyType.GetProperties().Should()
                    .Contain(x => x.Name == "ParentProductCategoryId");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithSelectProjection2()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=SalesOrderDetail.LineTotal") as IQueryable<object>;

                var properties = query.First().GetType().GetProperties();
                properties.Length.Should().Be(1);
                properties.Should().Contain(p => p.Name == "SalesOrderDetail");

                var salesOrderDetailProperty = properties.FirstOrDefault(p => p.Name == "SalesOrderDetail").PropertyType
                    .GenericTypeArguments[0];
                salesOrderDetailProperty.GetProperties().Should().Contain(x => x.Name == "LineTotal");


                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithSelectProjection3()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query =
                    context.Product.AutoQueryable("select=SalesOrderDetail.Product.ProductId") as IQueryable<object>;

                var properties = query.First().GetType().GetProperties();
                properties.Length.Should().Be(1);

                var salesOrderDetailProperty = properties.FirstOrDefault(p => p.Name == "SalesOrderDetail").PropertyType
                    .GenericTypeArguments[0];
                var productProperty = salesOrderDetailProperty.GetProperties().FirstOrDefault(x => x.Name == "Product");
                productProperty.Should().NotBeNull();
                productProperty.PropertyType.GetProperties().Should().Contain(x => x.Name == "ProductId");
                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithSelectProjection4()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable(
                        "select=name,productcategory.name,ProductCategory.ProductCategoryId,SalesOrderDetail.LineTotal")
                    as
                    IQueryable<object>;

                var properties = query.First().GetType().GetProperties();
                properties.Length.Should().Be(3);

                properties.Should().Contain(p => p.Name == "name");
                properties.Should().Contain(p => p.Name == "productcategory");
                properties.Should().Contain(p => p.Name == "SalesOrderDetail");

                var productcategoryProperty = properties.FirstOrDefault(p => p.Name == "productcategory");
                productcategoryProperty.PropertyType.GetProperties().Should().Contain(x => x.Name == "name");
                productcategoryProperty.PropertyType.GetProperties().Should()
                    .Contain(x => x.Name == "ProductCategoryId");

                var salesOrderDetailProperty = properties.FirstOrDefault(p => p.Name == "SalesOrderDetail").PropertyType
                    .GenericTypeArguments[0];
                salesOrderDetailProperty.GetProperties().Should().Contain(x => x.Name == "LineTotal");



                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithNameAndColor()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=name,color") as IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Length.Should().Be(2);

                properties.Should().Contain(p => p.Name == "name");
                properties.Should().Contain(p => p.Name == "color");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithNameAndColorIgnoreCase()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=Name,COLOR") as IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Length.Should().Be(2);

                properties.Should().Contain(p => p.Name == "Name");
                properties.Should().Contain(p => p.Name == "COLOR");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithUnselectableProperties()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("",
                        new AutoQueryableProfile {UnselectableProperties = new[] {"productid", "rowguid"}}) as
                    IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Should().NotContain(p => p.Name == "ProductId");
                properties.Should().NotContain(p => p.Name == "Rowguid");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllWithNameAndColorWithUnselectableProperties()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=Name,COLOR",
                    new AutoQueryableProfile {UnselectableProperties = new[] {"color"}}) as IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Length.Should().Be(1);

                properties.Should().Contain(p => p.Name == "Name");
                properties.Should().NotContain(p => p.Name == "COLOR");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectSkip50()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=ProductId,name,color&skip=50") as IQueryable<dynamic>;
                var first = query.First();
                Type type = first.GetType();
                int value = type.GetProperty("ProductId").GetValue(first);

                value.Should().Be(51);
                query.Count().Should().Be(DataInitializer.ProductSampleCount - 50);
            }
        }

        [Fact]
        public void SelectTake50()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=ProductId,name,color&take=50") as IQueryable<dynamic>;
                query.Count().Should().Be(50);
            }
        }

        [Fact]
        public void SelectSkipAndTake50()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query =
                    context.Product.AutoQueryable("select=ProductId,name,color&skip=50&take=50") as IQueryable<dynamic>;
                var first = query.First();
                Type type = first.GetType();
                int value = type.GetProperty("ProductId").GetValue(first);

                value.Should().Be(51);
                query.Count().Should().Be(50);
            }
        }

        [Fact]
        public void SelectOrderByColor()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = (context.Product.AutoQueryable("select=name,color&orderby=color") as IEnumerable<dynamic>)
                    .ToList();
                var first = query.First();
                var second = query.Skip(1).First();

                var last = query.Last();
                var preLast = query.Skip(DataInitializer.ProductSampleCount - 2).First();

                Type type = first.GetType();
                string firstValue = type.GetProperty("color").GetValue(first);
                string secondValue = type.GetProperty("color").GetValue(second);

                string lastValue = type.GetProperty("color").GetValue(last);
                string preLastValue = type.GetProperty("color").GetValue(preLast);



                firstValue.Should().Be("black");
                secondValue.Should().Be("black");
                lastValue.Should().Be("red");
                preLastValue.Should().Be("red");
            }
        }

        [Fact]
        public void SelectOrderById()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query =
                    (context.Product.AutoQueryable("select=productid,name,color&orderby=productid") as
                        IEnumerable<dynamic>).ToList();
                var first = query.First();
                var second = query.Skip(1).First();

                var last = query.Last();
                var preLast = query.Skip(DataInitializer.ProductSampleCount - 2).First();

                Type type = first.GetType();
                int firstValue = type.GetProperty("productid").GetValue(first);
                int secondValue = type.GetProperty("productid").GetValue(second);

                int lastValue = type.GetProperty("productid").GetValue(last);
                int preLastValue = type.GetProperty("productid").GetValue(preLast);



                firstValue.Should().Be(1);
                secondValue.Should().Be(2);
                lastValue.Should().Be(DataInitializer.ProductSampleCount);
                preLastValue.Should().Be(DataInitializer.ProductSampleCount - 1);
            }
        }

        [Fact]
        public void SelectOrderByIdDesc()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query =
                    (context.Product.AutoQueryable("select=productid,name,color&orderbydesc=productid") as
                        IEnumerable<dynamic>).ToList();
                var first = query.First();
                var second = query.Skip(1).First();

                var last = query.Last();
                var preLast = query.Skip(DataInitializer.ProductSampleCount - 2).First();

                Type type = first.GetType();
                int firstValue = type.GetProperty("productid").GetValue(first);
                int secondValue = type.GetProperty("productid").GetValue(second);

                int lastValue = type.GetProperty("productid").GetValue(last);
                int preLastValue = type.GetProperty("productid").GetValue(preLast);


                lastValue.Should().Be(1);
                preLastValue.Should().Be(2);
                firstValue.Should().Be(DataInitializer.ProductSampleCount);
                secondValue.Should().Be(DataInitializer.ProductSampleCount - 1);
            }
        }

        [Fact]
        public void SelectOrderByColorDesc()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query =
                    (context.Product.AutoQueryable("select=name,color&orderbydesc=color") as IEnumerable<dynamic>)
                    .ToList();
                var first = query.First();
                var second = query.Skip(1).First();

                var last = query.Last();
                var preLast = query.Skip(DataInitializer.ProductSampleCount - 2).First();

                Type type = first.GetType();
                string firstValue = type.GetProperty("color").GetValue(first);
                string secondValue = type.GetProperty("color").GetValue(second);

                string lastValue = type.GetProperty("color").GetValue(last);
                string preLastValue = type.GetProperty("color").GetValue(preLast);

                firstValue.Should().Be("red");
                secondValue.Should().Be("red");
                lastValue.Should().Be("black");
                preLastValue.Should().Be("black");
            }
        }

        [Fact]
        public void SelectOrderBySellStartDate()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query =
                    (context.Product.AutoQueryable("select=SellStartDate&orderby=SellStartDate") as IEnumerable<dynamic>
                    ).ToList();
                var currentDate = DateTime.MinValue;
                foreach (var product in query)
                {
                    var date = (DateTime) product.GetType().GetProperty("SellStartDate").GetValue(product);
                    date.Should().BeAfter(currentDate);
                    currentDate = date;
                }
            }
        }

        [Fact]
        public void SelectOrderBySellStartDateDesc()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query =
                    (context.Product.AutoQueryable("select=SellStartDate&orderbydesc=SellStartDate") as
                        IEnumerable<dynamic>).ToList();
                var currentDate = DateTime.MaxValue;
                foreach (var product in query)
                {
                    var date = (DateTime) product.GetType().GetProperty("SellStartDate").GetValue(product);
                    date.Should().BeBefore(currentDate);
                    currentDate = date;
                }
            }
        }

        [Fact]
        public void SelectFirst()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var product = context.Product.AutoQueryable("first=true");
                PropertyInfo[] properties = product.GetType().GetProperties();
                properties.Should().Contain(p => p.Name == "ProductId");
                ((int) properties.First(p => p.Name == "ProductId").GetValue(product)).Should().Be(1);
            }
        }

        // TODO : Ef core 2 does not return single value anymore for last or default. See in next release.
        //[Fact]
        //public void SelectLast()
        //{
        //    using (AutoQueryableContext context = new AutoQueryableContext())
        //    {
        //        DataInitializer.InitializeSeed(context);
        //        dynamic productLast = context.Product.AutoQueryable("last=true");
        //        var product = productLast.FirstOrDefault() as Product;
        //        PropertyInfo[] properties = product.GetType().GetProperties();

        //        //Assert.IsTrue(properties.First(p => p.Name == "ProductId").GetValue(product) == DataInitializer.ProductSampleCount);
        //    }
        //}



        [Fact]
        public void SelectFirstOrderbyIdDesc()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var product = context.Product.AutoQueryable("first=true&orderbydesc=productid");

                PropertyInfo[] properties = product.GetType().GetProperties();
                ((int) properties.First(p => p.Name == "ProductId").GetValue(product)).Should()
                    .Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllWithSelectInclude()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("top=50&select=name,SalesOrderDetail,productcategory",
                    new AutoQueryableProfile {UnselectableProperties = new[] {"color"}}) as IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Length.Should().Be(3);
                properties.Should().Contain(p => p.Name == "name");
                properties.Should().Contain(p => p.Name == "SalesOrderDetail");
                properties.Should().Contain(p => p.Name == "productcategory");

                query.Count().Should().Be(50);
            }
        }

        [Fact]
        public void SelectWithIncludeNavigationProperties()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable(
                    "top=50&select=name,SalesOrderDetail.Product.ProductId,productcategory",
                    new AutoQueryableProfile {UnselectableProperties = new[] {"color"}}) as IQueryable<object>;
                var firstResult = query.First();
                var properties = firstResult.GetType().GetProperties();
                properties.Length.Should().Be(3);

                properties.Should().Contain(p => p.Name == "name");
                properties.Should().Contain(p => p.Name == "SalesOrderDetail").Which.Should().NotBeNull();
                properties.Should().Contain(p => p.Name == "productcategory");

                var salesOrderDetailProperty = properties.FirstOrDefault(p => p.Name == "SalesOrderDetail").PropertyType
                    .GenericTypeArguments[0];
                var productProperty = salesOrderDetailProperty.GetProperties().FirstOrDefault(x => x.Name == "Product");
                productProperty.Should().NotBeNull();
                productProperty.PropertyType.GetProperties().Should().Contain(x => x.Name == "ProductId");
                query.Count().Should().Be(50);
            }
        }



        [Fact]
        public void SelectAllProductsWithDtoProjection()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.Select(p => new ProductDto
                {
                    Name = p.Name
                }).AutoQueryable("") as IQueryable<object>;
                var properties = query.First().GetType().GetProperties();
                properties.Length.Should().Be(1);

                properties.Should().Contain(p => p.Name == "Name");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithDtoProjectionAndSelectProjection()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.Select(p => new ProductDto
                {
                    Name = p.Name,
                    Category = new ProductCategoryDto
                    {
                        Name = p.ProductCategory.Name
                    }
                }).AutoQueryable("select=name,category.name") as IQueryable<object>;

                var properties = query.First().GetType().GetProperties();
                properties.Length.Should().Be(2);

                properties.Should().Contain(p => p.Name == "name");
                properties.Should().Contain(p => p.Name == "category");

                var categoryProperty = properties.FirstOrDefault(p => p.Name == "category");
                categoryProperty.PropertyType.GetProperties().Should().Contain(x => x.Name == "name");
                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithSelectProjectionWithUnselectableProperty()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable(
                        "select=name,productcategory.name,ProductCategory.ProductCategoryId",
                        new AutoQueryableProfile
 {
                            UnselectableProperties = new[] {"ProductCategory.ProductCategoryId"}
                        }) as
                    IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Length.Should().Be(2);

                properties.Should().Contain(p => p.Name == "name");
                properties.Should().Contain(p => p.Name == "productcategory");

                var productcategoryProperty = properties.FirstOrDefault(p => p.Name == "productcategory");
                productcategoryProperty.PropertyType.GetProperties().Should().Contain(x => x.Name == "name");
                productcategoryProperty.PropertyType.GetProperties().Should()
                    .NotContain(x => x.Name == "ProductCategoryId");
            }
        }

        [Fact]
        public void SelectAllProductsWithNameAndColorWithDtoProjection()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.Select(p => new
                {
                    p.Name,
                    p.Color,
                    categoryName = p.ProductCategory.Name
                }).AutoQueryable("select=name,color,categoryName") as IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Length.Should().Be(3);

                properties.Should().Contain(p => p.Name == "name");
                properties.Should().Contain(p => p.Name == "color");
                properties.Should().Contain(p => p.Name == "categoryName");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithNameAndColorIgnoreCaseWithDtoProjection()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.Select(p => new
                {
                    p.Name,
                    p.Color,
                    categoryName = p.ProductCategory.Name
                }).AutoQueryable("select=Name,COLOR") as IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Length.Should().Be(2);

                properties.Should().Contain(p => p.Name == "Name");
                properties.Should().Contain(p => p.Name == "COLOR");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllProductsWithUnselectablePropertiesWithDtoProjection()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.Select(p => new
                    {
                        p.ProductId,
                        p.Rowguid,
                        p.Name,
                        p.Color,
                        categoryName = p.ProductCategory.Name
                    }).AutoQueryable("",
                        new AutoQueryableProfile {UnselectableProperties = new[] {"productid", "rowguid"}}) as
                    IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Should().NotContain(p => p.Name == "ProductId");
                properties.Should().NotContain(p => p.Name == "Rowguid");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }


        [Fact]
        public void CountWithNullForeignKey()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=name,productextension.name") as IQueryable<object>;
                query?.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllPropertiesWithoutRelations()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=_") as IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Length.Should().Be(17);

                properties.Should().Contain(p => p.Name == "ProductId");
                properties.Should().Contain(p => p.Name == "Name");
                properties.Should().Contain(p => p.Name == "ProductNumber");
                properties.Should().Contain(p => p.Name == "Color");
                properties.Should().Contain(p => p.Name == "StandardCost");
                properties.Should().Contain(p => p.Name == "ListPrice");
                properties.Should().Contain(p => p.Name == "Size");
                properties.Should().Contain(p => p.Name == "Weight");
                properties.Should().Contain(p => p.Name == "ProductCategoryId");
                properties.Should().Contain(p => p.Name == "ProductModelId");
                properties.Should().Contain(p => p.Name == "SellStartDate");
                properties.Should().Contain(p => p.Name == "SellEndDate");
                properties.Should().Contain(p => p.Name == "DiscontinuedDate");
                properties.Should().Contain(p => p.Name == "ThumbNailPhoto");
                properties.Should().Contain(p => p.Name == "ThumbnailPhotoFileName");
                properties.Should().Contain(p => p.Name == "Rowguid");
                properties.Should().Contain(p => p.Name == "ModifiedDate");
                ;

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllPropertiesWithOneRelation()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=_,ProductModel") as IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Length.Should().Be(18);

                properties.Should().Contain(p => p.Name == "ProductId");
                properties.Should().Contain(p => p.Name == "Name");
                properties.Should().Contain(p => p.Name == "ProductNumber");
                properties.Should().Contain(p => p.Name == "Color");
                properties.Should().Contain(p => p.Name == "StandardCost");
                properties.Should().Contain(p => p.Name == "ListPrice");
                properties.Should().Contain(p => p.Name == "Size");
                properties.Should().Contain(p => p.Name == "Weight");
                properties.Should().Contain(p => p.Name == "ProductCategoryId");
                properties.Should().Contain(p => p.Name == "ProductModelId");
                properties.Should().Contain(p => p.Name == "SellStartDate");
                properties.Should().Contain(p => p.Name == "SellEndDate");
                properties.Should().Contain(p => p.Name == "DiscontinuedDate");
                properties.Should().Contain(p => p.Name == "ThumbNailPhoto");
                properties.Should().Contain(p => p.Name == "ThumbnailPhotoFileName");
                properties.Should().Contain(p => p.Name == "Rowguid");
                properties.Should().Contain(p => p.Name == "ModifiedDate");
                properties.Should().Contain(p => p.Name == "ProductModel");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }

        [Fact]
        public void SelectAllPropertiesFromLevelZero()
        {
            using (var context = new Mock.AutoQueryableContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable("select=*") as IQueryable<object>;
                var properties = query.First().GetType().GetProperties();

                properties.Length.Should().Be(21);

                properties.Should().Contain(p => p.Name == "ProductId");
                properties.Should().Contain(p => p.Name == "Name");
                properties.Should().Contain(p => p.Name == "ProductNumber");
                properties.Should().Contain(p => p.Name == "Color");
                properties.Should().Contain(p => p.Name == "StandardCost");
                properties.Should().Contain(p => p.Name == "ListPrice");
                properties.Should().Contain(p => p.Name == "Size");
                properties.Should().Contain(p => p.Name == "Weight");
                properties.Should().Contain(p => p.Name == "ProductCategoryId");
                properties.Should().Contain(p => p.Name == "ProductModelId");
                properties.Should().Contain(p => p.Name == "SellStartDate");
                properties.Should().Contain(p => p.Name == "SellEndDate");
                properties.Should().Contain(p => p.Name == "DiscontinuedDate");
                properties.Should().Contain(p => p.Name == "ThumbNailPhoto");
                properties.Should().Contain(p => p.Name == "ThumbnailPhotoFileName");
                properties.Should().Contain(p => p.Name == "Rowguid");
                properties.Should().Contain(p => p.Name == "ModifiedDate");
                properties.Should().Contain(p => p.Name == "SalesOrderDetail");
                properties.Should().Contain(p => p.Name == "ProductExtension");
                properties.Should().Contain(p => p.Name == "ProductCategory");
                properties.Should().Contain(p => p.Name == "ProductModel");

                query.Count().Should().Be(DataInitializer.ProductSampleCount);
            }
        }
    }
}