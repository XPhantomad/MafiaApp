//using System;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace MafiaApp
//{
//    public class AsyncLazyusw<T> : Lazy<Task<T>>
//    {
//        readonly Lazy<Task<T>> instance;

//        public AsyncLazyusw(Func<T> factory)
//        {
//            instance = new Lazy<Task<T>>(() => Task.Run(factory));
//        }

//        public AsyncLazyusw(Func<Task<T>> factory)
//        {
//            instance = new Lazy<Task<T>>(() => Task.Run(factory));
//        }

//        public TaskAwaiter<T> GetAwaiter()
//        {
//            return instance.Value.GetAwaiter();
//        }
//    }
//}
