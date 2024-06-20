using Xunit;
using Moq;
using Domain.Interfaces.Services.Usuarios;
using Infraestructure.Services.Usuarios;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Interfaces.Auth;
using Domain.Specifications;

namespace TestApi_Agilesoft.Tests.TestsUsuarios
{
    public class UsuarioServiceTest
    {
        private readonly Mock<IUsuarios> _mockUserRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEncriptar> _mockEncriptar;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTest()
        {
            _mockUserRepository = new Mock<IUsuarios>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockEncriptar = new Mock<IEncriptar>();
            _usuarioService = new UsuarioService(_mockUnitOfWork.Object, _mockEncriptar.Object);
        }

        [Fact]
        public async Task GetUsuarioByUsername_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var username = "testuser";
            var expectedUser = new Usuario { Username = username };

            var mockRepo = new Mock<IGenericRepository<Usuario>>();
            mockRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UsuarioSpecifications>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUser);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<Usuario>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _usuarioService.GetUsuarioByUsername(username);

            // Assert
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetUsuarioByUsername_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "nonexistentuser";

            var mockRepo = new Mock<IGenericRepository<Usuario>>();
            mockRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<UsuarioSpecifications>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Usuario)null);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<Usuario>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _usuarioService.GetUsuarioByUsername(username);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddUsuario_AddsUserAndCommits_WhenUserIsValid()
        {
            // Arrange
            var usuario = new Usuario { Password = "plaintextpassword" };
            var hashedPassword = "hashedpassword";
            var expectedResult = 1;

            var mockRepo = new Mock<IGenericRepository<Usuario>>();
            _mockEncriptar.Setup(enc => enc.Encriptar(usuario.Password)).Callback<string>(password => usuario.Password = hashedPassword);
            _mockEncriptar.Setup(enc => enc.ObtenerHash()).Returns(hashedPassword);
            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(default(Usuario)));
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Usuario>()).Returns(mockRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(expectedResult);

            // Act
            var result = await _usuarioService.AddUsuario(usuario);

            // Assert
            _mockEncriptar.Verify(enc => enc.Encriptar(It.IsAny<string>()), Times.Once);
            _mockEncriptar.Verify(enc => enc.ObtenerHash(), Times.Once);
            mockRepo.Verify(repo => repo.AddAsync(It.Is<Usuario>(u => u.Password == hashedPassword), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task AddUsuario_ThrowsException_WhenRepositoryThrowsException()
        {
            // Arrange
            var usuario = new Usuario { Password = "plaintextpassword" };
            var hashedPassword = "hashedpassword";

            var mockRepo = new Mock<IGenericRepository<Usuario>>();
            _mockEncriptar.Setup(enc => enc.Encriptar(usuario.Password)).Callback<string>(password => usuario.Password = hashedPassword);
            _mockEncriptar.Setup(enc => enc.ObtenerHash()).Returns(hashedPassword);
            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception());
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Usuario>()).Returns(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _usuarioService.AddUsuario(usuario));

            _mockEncriptar.Verify(enc => enc.Encriptar(It.IsAny<string>()), Times.Once);
            _mockEncriptar.Verify(enc => enc.ObtenerHash(), Times.Once);
            mockRepo.Verify(repo => repo.AddAsync(It.Is<Usuario>(u => u.Password == hashedPassword), It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task GetUsuarioById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new Usuario { Id = userId };

            var mockRepo = new Mock<IGenericRepository<Usuario>>();
            mockRepo.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUser);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<Usuario>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _usuarioService.GetUsuarioById(userId);

            // Assert
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetUsuarioById_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;

            var mockRepo = new Mock<IGenericRepository<Usuario>>();
            mockRepo.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Usuario)null);

            _mockUnitOfWork.Setup(uow => uow.GetRepository<Usuario>())
                .Returns(mockRepo.Object);

            // Act
            var result = await _usuarioService.GetUsuarioById(userId);

            // Assert
            Assert.Null(result);
        }
    }
}
