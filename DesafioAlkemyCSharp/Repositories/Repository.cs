using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DesafioAlkemyCSharp.Repositories
{
    public interface IRepository
    {
        public void Agregar<T>(T entity) where T : class;
        public void Borrar<T>(T entity) where T : class;
        public void BorrarPor<T>(int id) where T : class;
        public void BorrarPorGuid<T>(string id) where T : class;
        public T EncontrarPor<T>(int id) where T : class;
        public T EncontrarPorGuid<T>(string id) where T : class;
        public void Actualizar<T>(T entity) where T : class;
        public void ActualizarPor<T>(int id) where T : class;
        public void ActualizarPorGuid<T>(string id) where T : class;
        public List<T> Traer<T>() where T : class;

    }

    public class Repository
    {
        private readonly Context.DesafioContext _context;
        
        public Repository(Context.DesafioContext context)
        {
            _context = context;
        }

        public void Agregar<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Borrar<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void BorrarPor<T>(int id) where T : class
        {
            T entity = EncontrarPor<T>(id);
            Borrar<T>(entity);
        }

        public void BorrarPorGuid<T>(string id) where T : class
        {
            T entity = EncontrarPorGuid<T>(id);
            Borrar<T>(entity);
        }

        public T EncontrarPor<T>(int id) where T : class
        {
            return _context.Set<T>().Find(id);
        }
        public T EncontrarPorGuid<T>(string id) where T : class
        {
            return _context.Set<T>().Find(id);
        }

        public void Actualizar<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void ActualizarPor<T>(int id) where T : class
        {
            T entity = EncontrarPor<T>(id);
            Actualizar<T>(entity);
        }

        public void ActualizarPorGuid<T>(string id) where T : class
        {
            T entity = EncontrarPorGuid<T>(id);
            Actualizar<T>(entity);
        }

        public List<T> Traer<T>() where T : class
        {
            List<T> query = _context.Set<T>().ToList();
            return query;
        }
    }
}
