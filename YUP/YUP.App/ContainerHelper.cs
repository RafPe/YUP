using Autofac;

namespace YUP.App
{
    public class ContainerHelper
    {
        public static ContainerBuilder Builder { get; private set; }

        public static IContainer Container { get; private set; }

        public static void InitializeBuilder()
        {
            Builder = new ContainerBuilder();
        }

        public static bool IsBuilded { get; set; }

        public static void SetAutofacContainer()
        {
            if (!IsBuilded)
            {
                Container = Builder.Build();
                IsBuilded = true;
            }
            else
            {
                Builder.Update(Container);
            }
        }

        public static T GetService<T>(string name = null)
        {
            return string.IsNullOrEmpty(name) ? Container.Resolve<T>() : Container.ResolveNamed<T>(name);
        }
    }
}



//Console.WriteLine("Begin Working!");

//        //important service part
//        Bootstrapper.InitializeBuilder();
//        Bootstrapper.Builder.Register<IImportantService>(ctx => new ImportantService());
//        Bootstrapper.SetAutofacContainer();

//        var importantService = Bootstrapper.Container.Resolve<IImportantService>();
//var importantParameter = importantService.DoSomeInitializationWork();

////other services part
//Bootstrapper.InitializeBuilder();
//        Bootstrapper.Builder.Register<IService>(ctx => new ProductService(importantParameter)).Named<IService>("product");
//        Bootstrapper.Builder.Register<IService>(ctx => new CustomerService(importantParameter)).Named<IService>("customer");
//        Bootstrapper.SetAutofacContainer();

//        var productService = Bootstrapper.GetService<IService>("product");
//var customerService = Bootstrapper.GetService<IService>("customer");

//productService.DoSomeWork();
//        customerService.DoSomeWork();

//        Console.WriteLine("All Work Done!");