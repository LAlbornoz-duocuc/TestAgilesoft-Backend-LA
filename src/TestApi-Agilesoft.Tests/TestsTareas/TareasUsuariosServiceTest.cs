using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Infraestructure.Services.Tareas;
using Infraestructure.Services.Usuarios;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApi_Agilesoft.Tests.TestsTareas
{
    public class TareasUsuariosServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly TareasUsuarioService _tareasUsuarioService;

        public TareasUsuariosServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _tareasUsuarioService = new TareasUsuarioService(_mockUnitOfWork.Object);
        }


        [Fact]
        public async Task GetTareasByUserId_ReturnsTaskList_WhenTasksExist()
        {
            // Arrange
            var userId = 1;
            var tareaId = 1;
            var id = 1;
            var tareasUsuarioSpecifications = new TareasUsuarioSpecifications(userId);
            var expectedTasks = new List<TareasUsuario>{
                new TareasUsuario { Id = id, TareasId = tareaId, UsuarioId = userId },
                new TareasUsuario { Id = id, TareasId = tareaId, UsuarioId = userId }
            };

            var mockRepo = new Mock<IGenericRepository<TareasUsuario>>();
            mockRepo.Setup(repo => repo.ListAsync(tareasUsuarioSpecifications, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasUsuario>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _tareasUsuarioService.GetTareasByUserId(tareasUsuarioSpecifications);

            // Assert
            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetTareasByUserId_ReturnsEmptyList_WhenNoTasksExist()
        {
            // Arrange
            var userId = 1;
            var tareasUsuarioSpecifications = new TareasUsuarioSpecifications(userId);
            var expectedTasks = new List<TareasUsuario>();

            var mockRepo = new Mock<IGenericRepository<TareasUsuario>>();
            mockRepo.Setup(repo => repo.ListAsync(tareasUsuarioSpecifications, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasUsuario>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _tareasUsuarioService.GetTareasByUserId(tareasUsuarioSpecifications);

            // Assert
            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetTareasByUserIdAndTaskId_ReturnsTaskList_WhenTasksExist()
        {
            // Arrange
            var userId = 1;
            var tareaId = 1;
            var id = 1;
            var tareasUsuarioSpecifications = new TareasUsuarioSpecifications(userId,tareaId);
            var expectedTasks = new List<TareasUsuario>{
                new TareasUsuario { Id = id, TareasId = tareaId, UsuarioId = userId },
                new TareasUsuario { Id = id, TareasId = tareaId, UsuarioId = userId }
            };

            var mockRepo = new Mock<IGenericRepository<TareasUsuario>>();
            mockRepo.Setup(repo => repo.ListAsync(tareasUsuarioSpecifications, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasUsuario>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _tareasUsuarioService.GetTareasByUserId(tareasUsuarioSpecifications);

            // Assert
            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetTareasByUserIdAndTaskId_ReturnsEmptyList_WhenNoTasksExist()
        {
            // Arrange
            var userId = 1;
            var tareaId = 1;
            var tareasUsuarioSpecifications = new TareasUsuarioSpecifications(userId, tareaId);
            var expectedTasks = new List<TareasUsuario>();

            var mockRepo = new Mock<IGenericRepository<TareasUsuario>>();
            mockRepo.Setup(repo => repo.ListAsync(tareasUsuarioSpecifications, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasUsuario>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _tareasUsuarioService.GetTareasByUserId(tareasUsuarioSpecifications);

            // Assert
            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task AddTareaUsuario_AddsUserAndCommits_WhenUserIsValid()
        {
            // Arrange
            var userId = 1;
            var tareaId = 1;
            var id = 1;
            var tareasUsuarios = new TareasUsuario { Id = id, TareasId = tareaId, UsuarioId = userId };
            var expectedResult = 1;

            var mockRepo = new Mock<IGenericRepository<TareasUsuario>>();
            mockRepo.Setup(repo => repo.AddAsync(tareasUsuarios, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(default(TareasUsuario)));
            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasUsuario>()).Returns(mockRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(expectedResult);

            // Act
            var result = await _tareasUsuarioService.AddTareaUsuario(tareasUsuarios);

            // Assert
            mockRepo.Verify(repo => repo.AddAsync(tareasUsuarios, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task AddTareaUsuario_ThrowsException_WhenRepositoryThrowsException()
        {
            // Arrange
            var userId = 1;
            var tareaId = 1;
            var id = 1;
            var tareasUsuarios = new TareasUsuario { Id = id, TareasId = tareaId, UsuarioId = userId };

            var mockRepo = new Mock<IGenericRepository<TareasUsuario>>();
            mockRepo.Setup(repo => repo.AddAsync(tareasUsuarios, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception());
            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasUsuario>()).Returns(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _tareasUsuarioService.AddTareaUsuario(tareasUsuarios));

            mockRepo.Verify(repo => repo.AddAsync(tareasUsuarios, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteTareaUsuario_AddsUserAndCommits_WhenUserIsValid()
        {
            // Arrange
            var userId = 1;
            var tareaId = 1;
            var id = 1;
            var tareasUsuarios = new TareasUsuario { Id = id, TareasId = tareaId, UsuarioId = userId };
            var expectedResult = 1;

            var mockRepo = new Mock<IGenericRepository<TareasUsuario>>();
            mockRepo.Setup(repo => repo.DeleteAsync(tareasUsuarios, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(default(TareasUsuario)));
            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasUsuario>()).Returns(mockRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(expectedResult);

            // Act
            var result = await _tareasUsuarioService.DeleteTareaUsuario(tareasUsuarios);

            // Assert
            mockRepo.Verify(repo => repo.DeleteAsync(tareasUsuarios, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task DeleteTareaUsuario_ThrowsException_WhenRepositoryThrowsException()
        {
            // Arrange
            var userId = 1;
            var tareaId = 1;
            var id = 1;
            var tareasUsuarios = new TareasUsuario { Id = id, TareasId = tareaId, UsuarioId = userId };

            var mockRepo = new Mock<IGenericRepository<TareasUsuario>>();
            mockRepo.Setup(repo => repo.DeleteAsync(tareasUsuarios, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception());
            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasUsuario>()).Returns(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _tareasUsuarioService.DeleteTareaUsuario(tareasUsuarios));

            mockRepo.Verify(repo => repo.DeleteAsync(tareasUsuarios, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }
    }
}
