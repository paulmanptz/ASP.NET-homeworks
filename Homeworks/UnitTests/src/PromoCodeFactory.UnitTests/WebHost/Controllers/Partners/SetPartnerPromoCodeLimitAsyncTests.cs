using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System;
using AutoFixture.AutoMoq;
using AutoFixture;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly PartnersController _partnersController;
        private readonly SetPartnerPromoCodeLimitRequest _setPartnerPromoCodeLimitRequest;
        private readonly Mock<IRepository<Partner>> _mockRepository;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockRepository = fixture.Freeze<Mock<IRepository<Partner>>>();
            _setPartnerPromoCodeLimitRequest = fixture.Freeze<SetPartnerPromoCodeLimitRequest>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }

        public Partner GetPaetnerForTest()
        {
            return new Partner()
            {
                Id = Guid.NewGuid(),
                Name = "Parner Name",
                IsActive = true,
                NumberIssuedPromoCodes = 5,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.NewGuid(),
                        CreateDate = new DateTime(2017, 1, 1),
                        EndDate = new DateTime(2017, 12, 31),
                        Limit = 10
                    }
                }
            };

        }

        /// <summary>
        /// Если партнер не найден, то также нужно выдать ошибку 404;
        /// </summary>
        [Fact]
        public async Task GetParner_IsEmpty_NotFound()
        {
            //Arrange
            var id = Guid.NewGuid();
            Partner p = null;

            //Act
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((p));
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(id, _setPartnerPromoCodeLimitRequest);

            //Assert 
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        /// <summary>
        /// Если партнер заблокирован, то есть поле IsActive=false в классе Partner, то также нужно выдать ошибку 400;
        /// </summary>
        [Fact]
        public async Task PartnerIsLock_IfActiveFalse_return400code()
        {
            //Arrange
            Partner p = GetPaetnerForTest();
            p.IsActive = false;

            //Act
            _mockRepository.Setup(r => r.GetByIdAsync(p.Id)).ReturnsAsync((p));
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(p.Id, _setPartnerPromoCodeLimitRequest);

            //Assert 
            result.Should().BeAssignableTo<BadRequestObjectResult>();

        }

        /// <summary>
        /// При установке лимита нужно отключить предыдущий лимит;
        /// </summary>
        [Fact]
        public async Task SwitchOffLimit_If_SetNewLimit()
        {
            //Arrange
            var partner = GetPaetnerForTest();
            partner.PartnerLimits.FirstOrDefault(x => !x.CancelDate.HasValue).EndDate = DateTime.Now.AddDays(-1);
            _mockRepository.Setup(r => r.GetByIdAsync(partner.Id)).ReturnsAsync(partner);

            //Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, _setPartnerPromoCodeLimitRequest);

            //Assert
            partner.PartnerLimits.Where(x => !x.CancelDate.HasValue).ToList().Should().HaveCount(1);
        }

        /// <summary>
        /// Лимит должен быть больше 0 - ошибка;
        /// </summary>
        [Fact]
        public async Task SettedNotPositivePersonLimitPromocode_is_error()
        {
            //Arrange
            var partner = GetPaetnerForTest();
            _mockRepository.Setup(r => r.GetByIdAsync(partner.Id)).ReturnsAsync(partner);
            var request = new SetPartnerPromoCodeLimitRequest()
            { EndDate = DateTime.Now,
              Limit = -2};

            //Act
            _mockRepository.Setup(r => r.GetByIdAsync(partner.Id)).ReturnsAsync((partner));
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            //Assert 
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        /// <summary>
        /// Лимит должен быть больше 0 - ошибка;
        /// </summary>
        [Fact]
        public async Task SettedPositivePersonLimitPromocode_is_succes()
        {
            //Arrange
            var partner = GetPaetnerForTest();
            _mockRepository.Setup(r => r.GetByIdAsync(partner.Id)).ReturnsAsync(partner);
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now,
                Limit = 2
            };

            //Act
            _mockRepository.Setup(r => r.GetByIdAsync(partner.Id)).ReturnsAsync((partner));
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            //Assert 
            partner.PartnerLimits.Where(x => !x.CancelDate.HasValue).ToList().Should().HaveCount(1);
        }

        /// <summary>
        ///Нужно убедиться, что сохранили новый лимит в базу данных
        /// </summary>
        [Fact]
        public async Task SettedNewLimit_isEquilToDataBaseOne_True()
        {
            //Arrange
            var partner = GetPaetnerForTest();
            _mockRepository.Setup(r => r.GetByIdAsync(partner.Id)).ReturnsAsync(partner);

            //Act 
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, _setPartnerPromoCodeLimitRequest);

            //Assert
            _mockRepository.Object.GetByIdAsync(partner.Id).Result
                .PartnerLimits.FirstOrDefault(l => l.Id.Equals(((CreatedAtActionResult)result).RouteValues["limitId"]))
                .Should().BeEquivalentTo(_setPartnerPromoCodeLimitRequest, options => options.Including(x => x.EndDate).Including(x => x.Limit));
        }
    }

}
