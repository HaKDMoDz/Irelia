using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

namespace Irelia.Render
{
    public abstract class Resource<T> : DisposableObject where T: class
    {
        protected void Save(string fileName, T obj)
        {
            IFormatter formatter = CreateFormatter(fileName);
            if (formatter == null)
                throw new ArgumentException("Failed to get formatter for resource: " + fileName, "fileName");

            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(stream, obj);
            }
        }

        protected static T Load(string fileName)
        {
            IFormatter formatter = CreateFormatter(fileName);
            if (formatter == null)
                throw new ArgumentException("Failed to get formatter for resource: " + fileName, "fileName");

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                return formatter.Deserialize(stream) as T;
            }
        }

        private static IFormatter CreateFormatter(string fileName)
        {
            string fileExt = Path.GetExtension(fileName);

            if (fileExt.EndsWith("b"))
                return new BinaryFormatter();
            else if (fileExt.EndsWith("s"))
                return new SoapFormatter();
            else
                return null;
        }

        #region Overrides DisposableObject
        protected override void Dispose(bool disposeManagedResources)
        {
            if (!IsDisposed)
            {
                OnDispose();
            }

            base.Dispose(disposeManagedResources);
        }

        protected virtual void OnDispose() 
        {
        }
        #endregion
    }
}
