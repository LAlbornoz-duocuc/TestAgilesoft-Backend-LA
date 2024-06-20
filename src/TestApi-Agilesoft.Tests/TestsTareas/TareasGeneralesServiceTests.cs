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
using System.Threading.Tasks;

namespace TestApi_Agilesoft.Tests.TestsTareas
{
    public class TareasGeneralesServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly TareasGeneralesService _tareasGeneralesService;
        private readonly TareasGenerales _tareasGeneralesUpdate;
        private readonly TareasGenerales _tareasGeneralesAdd;

        public TareasGeneralesServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _tareasGeneralesService = new TareasGeneralesService(_mockUnitOfWork.Object);

            _tareasGeneralesUpdate = new TareasGenerales
            {
                Id = 1,
                Descripcion = "test",
                Estado = 1,
                Nombre = "test"
            };

            _tareasGeneralesAdd = new TareasGenerales
            {
                Id = 1,
                Descripcion = "test",
                Estado = 1,
                FechaCreacion = DateTime.Now,
                FechaUltimaActualizacion = DateTime.Now,
                Nombre = "test"

            };
        }

        [Fact]
        public async Task AddTarea_AddsUserAndCommits_WhenUserIsValid()
        {
            // Arrange
            var id = 1;
            var tareasGenerales = new TareasGenerales
            {
                Id = id,
                Descripcion = "test",
                Estado = 1,
                FechaCreacion = DateTime.Now,
                FechaUltimaActualizacion = DateTime.Now,
                Nombre = "test"

            };
            var expectedResult = 1;

            var mockRepo = new Mock<IGenericRepository<TareasGenerales>>();
            mockRepo.Setup(repo => repo.AddAsync(tareasGenerales, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(default(TareasGenerales)));
            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasGenerales>()).Returns(mockRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(expectedResult);

            // Act
            var result = await _tareasGeneralesService.AddTarea(tareasGenerales);

            // Assert
            mockRepo.Verify(repo => repo.AddAsync(tareasGenerales, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task AddTarea_ThrowsException_WhenRepositoryThrowsException()
        {
            // Arrange

            var mockRepo = new Mock<IGenericRepository<TareasGenerales>>();
            mockRepo.Setup(repo => repo.AddAsync(_tareasGeneralesAdd, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception());
            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasGenerales>()).Returns(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _tareasGeneralesService.AddTarea(_tareasGeneralesAdd));

            mockRepo.Verify(repo => repo.AddAsync(_tareasGeneralesAdd, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateTarea_AddsUserAndCommits_WhenUserIsValid()
        {
            // Arrange
            var expectedResult = 1;

            var mockRepo = new Mock<IGenericRepository<TareasGenerales>>();
            mockRepo.Setup(repo => repo.UpdateAsync(_tareasGeneralesUpdate, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(default(TareasGenerales)));
            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasGenerales>()).Returns(mockRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(expectedResult);

            // Act
            var result = await _tareasGeneralesService.UpdateTarea(_tareasGeneralesUpdate);

            // Assert
            mockRepo.Verify(repo => repo.UpdateAsync(_tareasGeneralesUpdate, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task UpdateTarea_ThrowsException_WhenRepositoryThrowsException()
        {
            // Arrange

            var mockRepo = new Mock<IGenericRepository<TareasGenerales>>();
            mockRepo.Setup(repo => repo.UpdateAsync(_tareasGeneralesUpdate, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception());
            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasGenerales>()).Returns(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _tareasGeneralesService.UpdateTarea(_tareasGeneralesUpdate));

            mockRepo.Verify(repo => repo.UpdateAsync(_tareasGeneralesUpdate, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task GetTareasGenerales_ReturnsTaskList_WhenTasksExist()
        {
            // Arrange
            var id = 1;
            var expectedTasks = new List<TareasGenerales>
            {
                _tareasGeneralesAdd,
                _tareasGeneralesAdd
            };

            var mockRepo = new Mock<IGenericRepository<TareasGenerales>>();
            mockRepo.Setup(repo => repo.ListAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasGenerales>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _tareasGeneralesService.GetTareasGenerales();

            // Assert
            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetTareasGenerales_ReturnsEmptyList_WhenNoTasksExist()
        {

            // Arrange

            var expectedTasks = new List<TareasGenerales>();

            var mockRepo = new Mock<IGenericRepository<TareasGenerales>>();
            mockRepo.Setup(repo => repo.ListAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasGenerales>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _tareasGeneralesService.GetTareasGenerales();

            // Assert
            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetTareasGeneralesByIds_ReturnsTaskList_WhenTasksExist()
        {


            // Arrange
            var expectedTasks = new List<TareasGenerales>
            {
                new TareasGenerales { Id = 1, Descripcion = "test", Estado = 1, Nombre = "test" },
                new TareasGenerales { Id = 2, Descripcion = "test2", Estado = 1, Nombre = "test2" }
            };

            var tareaIds = new List<int> { 1, 2 };
            var tareaGeneralSpec = new TareasGeneralesSpecifications(tareaIds);

            var mockRepo = new Mock<IGenericRepository<TareasGenerales>>();
            mockRepo.Setup(repo => repo.ListAsync(It.IsAny<TareasGeneralesSpecifications>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedTasks);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasGenerales>())
                           .Returns(mockRepo.Object);

            // Act
            var result = await _tareasGeneralesService.GetTareasGeneralesByIds(tareaIds);

            // Assert
            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetTareasGeneralesByIds_ReturnsEmptyList_WhenNoTasksExist()
        {
            // Arrange

            var expectedTasks = new List<TareasGenerales>();
            var TareaIds = new List<int> { 1, 2, 3, 4 };

            var tareaGeneralSpec = new TareasGeneralesSpecifications(TareaIds);

            var mockRepo = new Mock<IGenericRepository<TareasGenerales>>();
            mockRepo.Setup(repo => repo.ListAsync(It.IsAny<TareasGeneralesSpecifications>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasGenerales>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _tareasGeneralesService.GetTareasGeneralesByIds(TareaIds);

            // Assert
            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetByTareasGeneralesId_ReturnsTaskList_WhenTasksExist()
        {


            // Arrange
            var expectedTasks = _tareasGeneralesAdd;
            var TareaGeneralId = 1;

            var mockRepo = new Mock<IGenericRepository<TareasGenerales>>();
            mockRepo.Setup(repo => repo.GetByIdAsync(TareaGeneralId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedTasks);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasGenerales>())
                           .Returns(mockRepo.Object);

            // Act
            var result = await _tareasGeneralesService.GetByTareasGeneralesId(TareaGeneralId);

            // Assert
            Assert.Equal(expectedTasks, result);
        }

        [Fact]
        public async Task GetByTareasGeneralesId_ReturnsEmptyList_WhenNoTasksExist()
        {
            // Arrange
            var expectedTasks = new TareasGenerales();
            var TareaGeneralId = 1;

            var mockRepo = new Mock<IGenericRepository<TareasGenerales>>();
            mockRepo.Setup(repo => repo.GetByIdAsync(TareaGeneralId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<TareasGenerales>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _tareasGeneralesService.GetByTareasGeneralesId(TareaGeneralId);

            // Assert
            Assert.Equal(expectedTasks, result);
        }
    }
}
