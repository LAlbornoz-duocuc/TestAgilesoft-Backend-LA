using Autofac;
using Domain.Interfaces;
using Domain.Interfaces.Services.Auth;
using Domain.Interfaces.Services.TareasUsuarios;
using Domain.Interfaces.Services.Usuarios;
using Infraestructure.Interfaces.Auth;
using Infraestructure.Models;
using Infraestructure.Services.Auth;
using Infraestructure.Services.Tareas;
using Infraestructure.Services.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Module = Autofac.Module;

namespace Infraestructure
{
    public class InfraestructureModule : Module
    {
        private bool _esDesarrollo = false;
        private List<Assembly> _ensamblados = new List<Assembly>();

        public InfraestructureModule(bool esDesarrollo, Assembly llamarEnsamblado = null)
        {
            _esDesarrollo = esDesarrollo;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //se puede separar por si hay dependencias solo de desarrollo
            RegistrarDependenciasComunes(builder);
        }

        private void RegistrarDependenciasComunes(ContainerBuilder builder)
        {
            /* Define que alcance tendra la interfaz al momento de ser implementada */
            builder.RegisterType<SaltPbkdf2>()
                .As<IGenerarSalt>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HashPbkdf2>()
                .As<IGenerarHash>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EncriptarPbkdf2>()
                .As<IEncriptar>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(RepositoryImplements<>))
                .As(typeof(IGenericRepository<>));

            builder.RegisterType<TokenJWTService>()
                .As<ITokenJWT>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LoginService>()
                .As<ILogin>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TareasUsuarioService>()
                .As<ITareasUsuario>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TareasGeneralesService>()
                .As<ITareasGenerales>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UsuarioService>()
                .As<IUsuarios>()
                .InstancePerLifetimeScope();
        }
    }
}
