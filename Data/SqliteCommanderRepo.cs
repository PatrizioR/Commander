using System;
using System.Collections.Generic;
using System.Linq;
using Commander.Models;

namespace Commander.Data
{
    public class SqliteCommanderRepo : ICommanderRepo
    {
        private readonly CommanderContext _context;
        public SqliteCommanderRepo(CommanderContext context)
        {
            _context = context;
        }

        public void CreateCommand(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            _context.Commands.Add(command);
        }

        //DELETE /api/commands/{id}
        public void DeleteCommand(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            _context.Remove(command);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _context.Commands.AsEnumerable();
        }

        public Command GetCommandById(int id)
        {
            var searchedCommand = _context.Commands.Find(id);
            return searchedCommand;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void UpdateCommand(Command command)
        {
            // Nothing to do
        }
    }
}