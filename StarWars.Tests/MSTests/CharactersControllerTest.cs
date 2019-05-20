using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StarWars.Controllers;
using StarWars.Models;
using StarWars.Services;
using System.Threading.Tasks;

namespace StarWars.Tests.MSTests
{
    [TestClass]
    public class CharactersControllerTest
    {

        [TestMethod]
        public async Task GetCharacter_ReturnsHttpNotFound_ForInvalidId()
        {
            // Arrange
            int testCharacterId = 1000;

            var mockRepo = new Mock<ICharacterServices>();
            mockRepo.Setup(repo => repo.GetCharacterAsync(testCharacterId))
                .ReturnsAsync((CharacterModel)null);
            var controller = new CharactersController(mockRepo.Object);

            // Act
            var result = await controller.GetCharacter(testCharacterId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
