using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
namespace Dix.Services
{
    public interface IService
    {
        int Type { get; }
        string GetName();
        string Say();
    }

    public abstract class Service : IService
    {
        private readonly string _id;

        public virtual int Type => 0;

        public Service()
        {
            _id = System.Guid.NewGuid().ToString();
        }

        public virtual string Say()
        {
            return $"{GetName()}{_id}";
        }

        public virtual string GetName()
        {
            return "我是";
        }
    }

    public interface ISingleton : IService
    {
    }

    public class Singleton : Service, ISingleton
    {
        public override string GetName()
        {
            return $"{base.GetName()}单例：";
        }
    }

    public class Singleton1 : Service, ISingleton
    {
        public override string GetName()
        {
            return $"{base.GetName()}单例1：";
        }
    }

    public class Singleton2 : Service, ISingleton
    {
        public override string GetName()
        {
            return $"{base.GetName()}单例2：";
        }
    }

    public interface IScope : IService
    {
    }

    public class Scope : Service, IScope
    {
        public override string GetName()
        {
            return $"{base.GetName()}范围：";
        }
    }

    public class Scope1 : Service, IScope
    {
        public override int Type => 1;

        public override string GetName()
        {
            return $"{base.GetName()}范围1：";
        }
    }

    public class Scope2 : Service, IScope
    {
        public override int Type => 2;

        public override string GetName()
        {
            return $"{base.GetName()}范围2：";
        }
    }

    public class Scope3 : Service, IScope
    {
        public override int Type => 3;

        public override string GetName()
        {
            return $"{base.GetName()}范围3：";
        }
    }

    public class Scope4 : Service, IScope
    {
        public override int Type => 4;

        public override string GetName()
        {
            return $"{base.GetName()}范围4：";
        }
    }

    public interface IAllScope : IService
    {
    }

    public class AllScope : Service, IAllScope
    {
        private readonly IServiceProvider _provider;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEnumerable<IScope> _services;
        private readonly IScope _scope;

        public AllScope(IServiceProvider provider, IServiceScopeFactory scopeFactory, IEnumerable<IScope> services, IScope scope)
        {
            _provider = provider;
            _scopeFactory = scopeFactory;
            _services = services;
            _scope = scope;
        }

        public override string Say()
        {
            var s = _provider.GetService<IScope>();
            var f = _scopeFactory.CreateScope().ServiceProvider.GetService<IScope>();
            string r = $"AllScope{System.Environment.NewLine}";
            r += $"定位器：{s.Say()}{System.Environment.NewLine}";
            r += $"factory：{f.Say()}{System.Environment.NewLine}";
            r += $"集合：{ string.Join(",", _services.Select(x => x.Say()))}{System.Environment.NewLine}";
            r += $"构造：{_scope.Say()}{System.Environment.NewLine}";
            return r;
        }
    }

    public interface ITransit : IService
    {
    }

    public class Transit : Service, ITransit
    {
        public override string GetName()
        {
            return $"{base.GetName()}瞬时：";
        }
    }

    public class Transit1 : Service, ITransit
    {
        public override string GetName()
        {
            return $"{base.GetName()}瞬时：";
        }
    }

    public interface IServiceFactory
    {
        IService[] GetService(int[] ints);
    }

    public class ServiceFactory : IServiceFactory
    {
        private readonly IEnumerable<IScope> _services;
        public ServiceFactory(IEnumerable<IScope> services)
        {
            _services = services;
        }

        public IService[] GetService(int[] ints)
        {
            return _services.Where(x => ints.Contains(x.Type)).ToArray();
        }
    }

}
